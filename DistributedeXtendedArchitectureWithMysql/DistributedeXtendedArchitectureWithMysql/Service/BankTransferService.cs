using DistributedeXtendedArchitectureWithMysql.Model;
using MySql.Data.MySqlClient;

namespace DistributedeXtendedArchitectureWithMysql.Service
{
    public class BankTransferService : IBankTransferService
    {
        private readonly string _connectionStringBankA;
        private readonly string _connectionStringBankB;
        private readonly ILogger<BankTransferService> _logger;

        public BankTransferService(IConfiguration configuration,
            ILogger<BankTransferService> logger)
        {
            _connectionStringBankA = configuration.GetConnectionString("ConnectionStringBankA");
            _connectionStringBankB = configuration.GetConnectionString("ConnectionStringBankB");
            _logger = logger;
        }

        /// <summary>        
        /// 一、不使用 XA 的轉賬方法 - 可能出現的問題：
        /// 1. 當 Bank_A 扣款成功，但 Bank_B 加款失敗時，資金會丟失
        /// 2. 系統崩潰或網絡中斷時，無法確保數據一致性
        /// 3. 無法進行有效的回滾操作
        /// </summary>
        public async Task<TransferResult> TransferWithoutXAAsync(TransferRequest request)
        {
            try
            {
                // Step 1-1 : 從 Bank_A 扣款
                using (var connA = new MySqlConnection(_connectionStringBankA))
                {
                    await connA.OpenAsync();
                    using (var cmd = connA.CreateCommand())
                    {
                        var sql = $@"
UPDATE accounts
   SET balance = balance - @amount
 WHERE account_number = @account
";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@amount", request.Amount);
                        cmd.Parameters.AddWithValue("@account", request.FromAccount);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // 危險點1：如果在這裡系統崩潰或者網絡中斷
                // Bank_A 的錢已經扣除，但 Bank_B 還沒收到
                // 這時資金就會丟失，而且無法自動恢復

                // Step 1-2 : 在 Bank_B 增加餘額
                using (var connB = new MySqlConnection(_connectionStringBankB))
                {
                    await connB.OpenAsync();
                    using (var cmd = connB.CreateCommand())
                    {
                        var sql = $@"
UPDATE accounts
   SET balance = balance + @amount
 WHERE account_number = @account
";
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@amount", request.Amount);
                        cmd.Parameters.AddWithValue("@account", request.ToAccount);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // 危險點2：如果在這裡出錯
                // 雖然兩個操作都完成了，但我們無法確認是否都成功
                // 可能會出現數據不一致的情況

                return new TransferResult
                {
                    Success = true,
                    Message = "Transfer completed",
                    TransactionId = "NO_XA_" + Guid.NewGuid().ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transfer failed without XA");

                // Step 2: 回滾階段
                // 危險點3：即使捕獲到異常，我們也無法確保能夠正確地回滾之前的操作
                // 可能會導致數據不一致
                return new TransferResult
                {
                    Success = false,
                    Message = $"Transfer failed: {ex.Message}",
                    TransactionId = "NO_XA_" + Guid.NewGuid().ToString()
                };
            }
        }

        /// <summary>
        /// 二、使用 XA 的轉賬方法 - 解決的問題：
        /// 1. 確保轉賬的原子性：要麼全部成功，要麼全部失敗
        /// 2. 支持分布式事務回滾：出錯時可以安全回滾
        /// 3. 保證跨庫數據一致性：兩個銀行的數據始終保持一致
        /// </summary>
        public async Task<TransferResult> TransferWithXAAsync(TransferRequest request)
        {
            // Step 1. 產生本次 XA 的唯一碼
            string xaId = Guid.NewGuid().ToString();
            MySqlConnection connA = null;
            MySqlConnection connB = null;

            try
            {
                _logger.LogInformation($"Starting XA transaction {xaId}");

                // Step 2-1. 同時打開兩個連接
                connA = new MySqlConnection(_connectionStringBankA);
                connB = new MySqlConnection(_connectionStringBankB);
                await connA.OpenAsync();
                await connB.OpenAsync();

                // Step 2-2：開始 XA 事務
                // 說明：這確保了後續的所有操作都在一個分布式事務中
                await ExecuteXaCommandAsync(connA, $"XA START '{xaId}'");
                await ExecuteXaCommandAsync(connB, $"XA START '{xaId}'");

                // Step 2-3：行鎖
                // 說明：將此帳號鎖定，避免同時間被異動
                await ForUpdateAccount(connA, request.FromAccount);
                await ForUpdateAccount(connB, request.ToAccount);

                // Step 2-4：執行轉賬操作(業務邏輯)
                // 說明：這些操作要麼全部成功，要麼全部失敗
                await ExecuteTransferAsync(connA, request.FromAccount, -request.Amount);//A 扣款
                await ExecuteTransferAsync(connB, request.ToAccount, request.Amount);//B 增加餘額

                // Step 2-5：準備階段
                // 說明：確保所有參與者都準備好提交
                await ExecuteXaCommandAsync(connA, $"XA END '{xaId}'");
                await ExecuteXaCommandAsync(connB, $"XA END '{xaId}'");
                await ExecuteXaCommandAsync(connA, $"XA PREPARE '{xaId}'");
                await ExecuteXaCommandAsync(connB, $"XA PREPARE '{xaId}'");

                // Step 2-6：提交階段
                // 說明：只有當所有準備工作都成功後才提交
                await ExecuteXaCommandAsync(connA, $"XA COMMIT '{xaId}'");
                await ExecuteXaCommandAsync(connB, $"XA COMMIT '{xaId}'");

                _logger.LogInformation($"XA transaction completed successfully: {xaId}");

                return new TransferResult
                {
                    Success = true,
                    Message = "Transfer completed successfully with XA",
                    TransactionId = xaId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"XA transaction failed: {xaId}");

                try
                {
                    // Step 3：回滾階段
                    // XA 確保了即使在錯誤情況下也能正確回滾
                    if (connA?.State == System.Data.ConnectionState.Open)
                        await ExecuteXaCommandAsync(connA, $"XA ROLLBACK '{xaId}'");
                    if (connB?.State == System.Data.ConnectionState.Open)
                        await ExecuteXaCommandAsync(connB, $"XA ROLLBACK '{xaId}'");
                }
                catch (Exception rollbackEx)
                {
                    _logger.LogError(rollbackEx, $"XA rollback failed: {xaId}");
                }

                return new TransferResult
                {
                    Success = false,
                    Message = $"Transfer failed with XA: {ex.Message}",
                    TransactionId = xaId
                };
            }
            finally
            {
                await connA?.CloseAsync();
                await connB?.CloseAsync();
            }

            #region [閉包] 私有方法

            /// <summary>
            /// 執行 XA 事務相關命令
            /// </summary>
            async Task ExecuteXaCommandAsync(MySqlConnection connection, string command)
            {
                try
                {
                    using var cmd = connection.CreateCommand();
                    cmd.CommandText = command;
                    cmd.CommandTimeout = 30; // 設置超時時間
                    _logger.LogDebug($"Executing XA command: {command}");
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (MySqlException ex)
                {
                    _logger.LogError(ex, $"Error executing XA command: {command}");
                    throw new Exception($"XA command failed: {command}", ex);
                }
            }

            /// <summary>
            /// 鎖定行防止併發修改
            /// </summary>
            async Task ForUpdateAccount(MySqlConnection connection, string accountNumber)
            {
                try
                {
                    using var cmd = connection.CreateCommand();
                    cmd.CommandText = @"
                SELECT balance 
                  FROM accounts 
                 WHERE account_number = @account
                   FOR UPDATE";  // 鎖定行防止並發修改

                    cmd.Parameters.AddWithValue("@account", accountNumber);

                    using var reader = await cmd.ExecuteReaderAsync();
                    if (!await reader.ReadAsync())
                    {
                        throw new Exception($"Account not found: {accountNumber}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error ForUpdateAccount for account: {accountNumber}");
                    throw new Exception($"ForUpdateAccount failed for account: {accountNumber}", ex);
                }
            }

            /// <summary>
            /// 執行轉賬操作
            /// </summary>
            async Task ExecuteTransferAsync(MySqlConnection connection, string accountNumber, decimal amount)
            {
                try
                {
                    using var cmd = connection.CreateCommand();
                    cmd.CommandText = @"
                UPDATE accounts 
                   SET balance = balance + @amount
                 WHERE account_number = @account";

                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@account", accountNumber);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Account not found or update failed: {accountNumber}");
                    }

                    _logger.LogInformation($"Transfer completed for account {accountNumber}: {amount}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error executing transfer for account: {accountNumber}");
                    throw new Exception($"Transfer failed for account: {accountNumber}", ex);
                }
            }

            #endregion
        }
    }
}
