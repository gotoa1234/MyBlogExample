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
        /// 不使用 XA 的轉賬方法 - 可能出現的問題：
        /// 1. 當 Bank_A 扣款成功，但 Bank_B 加款失敗時，資金會丟失
        /// 2. 系統崩潰或網絡中斷時，無法確保數據一致性
        /// 3. 無法進行有效的回滾操作
        /// </summary>
        public async Task<TransferResult> TransferWithoutXAAsync(TransferRequest request)
        {
            try
            {
                // 步驟1: 從 Bank_A 扣款
                using (var connA = new MySqlConnection(_connectionStringBankA))
                {
                    await connA.OpenAsync();
                    using (var cmd = connA.CreateCommand())
                    {
                        cmd.CommandText = "UPDATE accounts " +
                            "                 SET balance = balance - @amount " +
                            "               WHERE account_number = @account";
                        cmd.Parameters.AddWithValue("@amount", request.Amount);
                        cmd.Parameters.AddWithValue("@account", request.FromAccount);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // 危險點1：如果在這裡系統崩潰或者網絡中斷
                // Bank_A 的錢已經扣除，但 Bank_B 還沒收到
                // 這時資金就會丟失，而且無法自動恢復

                // 步驟2: 在 Bank_B 增加餘額
                using (var connB = new MySqlConnection(_connectionStringBankB))
                {
                    await connB.OpenAsync();
                    using (var cmd = connB.CreateCommand())
                    {
                        cmd.CommandText = $@"UPDATE accounts
                                                SET balance = balance + @amount
                                              WHERE account_number = @account";
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
        /// 使用 XA 的轉賬方法 - 解決的問題：
        /// 1. 確保轉賬的原子性：要麼全部成功，要麼全部失敗
        /// 2. 支持分布式事務回滾：出錯時可以安全回滾
        /// 3. 保證跨庫數據一致性：兩個銀行的數據始終保持一致
        /// </summary>
        public async Task<TransferResult> TransferWithXAAsync(TransferRequest request)
        {
            string xaId = Guid.NewGuid().ToString();
            MySqlConnection connA = null;
            MySqlConnection connB = null;

            try
            {
                _logger.LogInformation($"Starting XA transaction {xaId}");

                // 同時打開兩個連接
                connA = new MySqlConnection(_connectionStringBankA);
                connB = new MySqlConnection(_connectionStringBankB);
                await connA.OpenAsync();
                await connB.OpenAsync();

                // 安全點1：開始 XA 事務
                // 這確保了後續的所有操作都在一個分布式事務中
                await ExecuteXaCommandAsync(connA, $"XA START '{xaId}'");
                await ExecuteXaCommandAsync(connB, $"XA START '{xaId}'");

                // 安全點2：檢查餘額
                // 如果餘額不足，整個事務會被回滾
                if (!await CheckBalanceAsync(connA, request.FromAccount, request.Amount))
                {
                    throw new InvalidOperationException("Insufficient funds");
                }

                // 安全點3：執行轉賬操作
                // 這些操作要麼全部成功，要麼全部失敗
                await ExecuteTransferAsync(connA, request.FromAccount, -request.Amount);
                await ExecuteTransferAsync(connB, request.ToAccount, request.Amount);

                // 安全點4：準備階段
                // 確保所有參與者都準備好提交
                await ExecuteXaCommandAsync(connA, $"XA END '{xaId}'");
                await ExecuteXaCommandAsync(connB, $"XA END '{xaId}'");
                await ExecuteXaCommandAsync(connA, $"XA PREPARE '{xaId}'");
                await ExecuteXaCommandAsync(connB, $"XA PREPARE '{xaId}'");

                // 安全點5：提交階段
                // 只有當所有準備工作都成功後才提交
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
                    // 安全點6：錯誤回滾
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
        }

        #region 私有方法

        /// <summary>
        /// 執行 XA 事務相關命令
        /// </summary>
        private async Task ExecuteXaCommandAsync(MySqlConnection connection, string command)
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
        /// 檢查賬戶餘額是否足夠
        /// </summary>
        private async Task<bool> CheckBalanceAsync(MySqlConnection connection, string accountNumber, decimal amount)
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

                decimal balance = reader.GetDecimal(0);
                _logger.LogInformation($"Current balance for {accountNumber}: {balance}");

                return balance >= amount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking balance for account: {accountNumber}");
                throw new Exception($"Balance check failed for account: {accountNumber}", ex);
            }
        }

        /// <summary>
        /// 執行轉賬操作
        /// </summary>
        private async Task ExecuteTransferAsync(MySqlConnection connection, string accountNumber, decimal amount)
        {
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                UPDATE accounts 
                SET balance = balance + @amount,
                    last_updated = CURRENT_TIMESTAMP
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

        /// <summary>
        /// 檢查 XA 事務狀態
        /// </summary>
        private async Task<string> CheckXaTransactionStatusAsync(MySqlConnection connection, string xaId)
        {
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "XA RECOVER";

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    string formatId = reader.GetString(0);
                    string gtrid = reader.GetString(1);
                    string bqual = reader.GetString(2);

                    if (gtrid == xaId)
                    {
                        return "PREPARED"; // 事務在準備狀態
                    }
                }

                return "COMPLETED"; // 事務已完成
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking XA transaction status: {xaId}");
                throw new Exception($"Failed to check XA transaction status: {xaId}", ex);
            }
        }

        #endregion
    }
}
