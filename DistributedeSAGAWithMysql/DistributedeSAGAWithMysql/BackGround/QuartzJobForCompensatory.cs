using Dapper;
using DistributedeSAGAWithMysql.Enum;
using DistributedeSAGAWithMysql.Repository.Dao;
using DistributedeSAGAWithMysql.Repository.Dto;
using DistributedeSAGAWithMysql.Repository.Interface;
using DistributedeSAGAWithMysql.Service.Interface;
using Framework.Database.Enum;
using Framework.Database.Interfaces;
using Quartz;

namespace DistributedeSAGAWithMysql.BackGround
{
    public class QuartzJobForCompensatory : IJob
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IUnitOfWorkAccessor _uowAccessor;
        private readonly ILogger<QuartzJobForCompensatory> _logger;
        private readonly IBalanceRepository _balanceRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMemberRepository _memberRespository;
        private readonly IShoppingCartService _shoppingCartService;

        public QuartzJobForCompensatory(
            IUnitOfWorkFactory uowFactory,
            IUnitOfWorkAccessor uowAccessor,
            ILogger<QuartzJobForCompensatory> logger,
            IBalanceRepository balanceRepository,
            ILogRepository logRepository,
            IMemberRepository memberRespository,
            IShoppingCartService shoppingCartService)
        {
            _uowFactory = uowFactory;
            _uowAccessor = uowAccessor;
            _logger = logger;
            _balanceRepository = balanceRepository;
            _logRepository = logRepository;
            _memberRespository = memberRespository;
            _shoppingCartService = shoppingCartService;
        }

        /// <summary>
        /// 1. 實際執行
        /// </summary>
        public async Task Execute(IJobExecutionContext context)
        {
            var currentTiem = DateTime.Now;
            var seqTime = currentTiem.ToString("yyyyMMddHHmmss");
            Console.WriteLine("===== 開始處理 =====");
            Console.WriteLine("時間紀錄 : " + currentTiem.ToString("yyyy/MM/dd HH:mm:ss") + " 流水時間序 : "+ seqTime);

            // 1-1. 取出補償需處理的資料
            var compensatoryInfo = await GetSagaIdCompensatoryStatus();

            // 1-2. 補償1 : 捨棄未扣款的資料 - 影響 Log 庫
            if (compensatoryInfo.notDeductionSagaIds.Any())
            {
                await CompensatoryNotDeduction(compensatoryInfo.notDeductionSagaIds);
                Console.WriteLine($@"[{seqTime}] 補償1 : SagaId : {string.Join(", " , compensatoryInfo.notDeductionSagaIds)}");
            }

            // 1-3. 補償2 : 繼續將 Balance 已扣款資料統計到 Purchase - 影響 Member 庫
            if (compensatoryInfo.notPurchaseSagaIds.Any())
            {
                await CompensatoryNotPurchase(compensatoryInfo.notPurchaseSagaIds);
                Console.WriteLine($@"[{seqTime}] 補償2 : SagaId : {string.Join(", ", compensatoryInfo.notPurchaseSagaIds)}");

            }

            // 1-4. 補償3 : 將尚未標記 Complate 的 SagaId 處理完成 - 影響 Log 庫
            // 標記要包含本次有執行 (寫入 Log + 扣款 Balance 表 結束) 的資料
            var executeMarkIds = compensatoryInfo.notMarkComplateSagaIds;
            executeMarkIds.AddRange(compensatoryInfo.notPurchaseSagaIds);
            if (executeMarkIds.Any())
            {
                await CompensatorynotMarkComplate(executeMarkIds);
                Console.WriteLine($@"[{seqTime}] 補償3  : SagaId : {string.Join(", ", executeMarkIds)}");
            }

            Console.WriteLine("===== 結束處理 =====");
            Console.WriteLine("");
            // 上述補償 1~3 每個都是獨立的庫 EX:
            // 1. 當補償2 有 SagaId = 'A' 完成寫入 Member 庫後，這時發生中斷
            // 2. 下次服務再次觸發，會發現只差標記 Log 已完成，會直接進入 補償3
            // ※ 補償1 直接捨棄，若上次未執行，則下次服務觸發仍會在補償1 讓對應 SagaId 標記 FAILED
        }

        /// <summary>
        /// 1-1. 取得補償狀態
        /// </summary>
        private async Task<CompensatoryDto> GetSagaIdCompensatoryStatus()
        {
            var reuslt = new CompensatoryDto();

            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Log);
            _uowAccessor.Current = uow;
            try
            {
                var getPendingSagaIds = await GetOverTimeSagaId();

                // 流程中尚需 : 扣款 + 購買統計 + 標記完成 (取消整筆訂單)
                reuslt.notDeductionSagaIds = await GetNotDeductionSagaIds(getPendingSagaIds);

                // 流程中尚需 : 購買統計 + 標記完成 (繼續處理，因為已經扣錢)
                reuslt.notPurchaseSagaIds = await GetNotPurchaseSagaIds(getPendingSagaIds.Except(reuslt.notDeductionSagaIds)
                                                                                                       .ToList());
                // 流程中尚需 : 標記完成 (繼續處理，已經完成只差在標記)
                reuslt.notMarkComplateSagaIds = getPendingSagaIds.Except(reuslt.notDeductionSagaIds)
                                                                 .Except(reuslt.notPurchaseSagaIds)
                                                                 .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return reuslt;

            /// 取出超時要處理的資料 - SagaId + Status
            async Task<List<string>> GetOverTimeSagaId()
            {
                var getResult = new List<string>();
                var sql = $@"
SELECT SagaId 
  FROM SagaTransaction 
 WHERE SagaTransaction.STATUS = 'PENDING'
   AND NOW() > DATE_ADD(CreatedAt, INTERVAL 5 MINUTE)
;";
                try
                {
                    getResult = (await uow.Connection.QueryAsync<string>(sql)).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return getResult;
            }

            // 取得尚未執行扣款的資料
            async Task<List<string>> GetNotDeductionSagaIds(List<string> sourcePendingSagaIds)
            {
                using var uowb = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Balance);
                _uowAccessor.Current = uowb;
                var missingIds = new List<string>();
                var sql = $@"
SELECT SagaId 
  FROM BalanceTransaction 
 WHERE sagaId IN @SagaIds
;";
                try
                {
                    var getResult = (await uowb.Connection.QueryAsync<string>(sql, new
                    {
                        @SagaIds = sourcePendingSagaIds
                    })).ToList();
                    // 排除法
                    missingIds = sourcePendingSagaIds.Except(getResult).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                return missingIds;
            }

            // 取得尚未執行統計-添加到 Purchase 的資料
            async Task<List<string>> GetNotPurchaseSagaIds(List<string> sourcePendingSagaIds)
            {
                using var uowm = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Member);
                _uowAccessor.Current = uowm;
                var missingIds = new List<string>();
                var sql = $@"
SELECT SagaId 
  FROM Purchase 
 WHERE sagaId IN @SagaIds
;";
                try
                {
                    var getResult = (await uowm.Connection.QueryAsync<string>(sql, new
                    {
                        @SagaIds = sourcePendingSagaIds
                    })).ToList();
                    // 排除法
                    missingIds = sourcePendingSagaIds.Except(getResult).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return missingIds;
            }
        }

        /// <summary>
        /// 1-2. 補償機制 -> 第二步執行前中斷 (寫入 Log 結束) -> 取消此筆訂單
        /// </summary>
        /// <returns></returns>
        private async Task CompensatoryNotDeduction(List<string> notDeductionSagaIds)
        {
            // 因為在 Balance 表中並未建立對應資料，沒有對實際的錢扣款，因此取消後續流程
            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Log);
            _uowAccessor.Current = uow;
            try
            {
                await uow.BeginTransactionAsync();

                // 範例說明每筆皆會處理，實際上應改用 Bulk Update SQL 語法
                foreach (var sagaId in notDeductionSagaIds)
                {
                    await _logRepository.UpdateLogStatus(sagaId, "FAILED");
                }

                await uow.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await uow.RollbackAsync();
            }
        }

        /// <summary>
        /// 1-3. 補償機制 -> 第三步執行前中斷 (寫入 Log + 扣款 Balance 表 結束) -> 繼續處理，因為已經扣錢
        /// </summary>
        /// <returns></returns>
        private async Task CompensatoryNotPurchase(List<string> notPurchaseSagaIds)
        {
            using var uowb = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Balance);
            _uowAccessor.Current = uowb;
            var collectionItems = (await _balanceRepository.GetBalanceTransactionItems(notPurchaseSagaIds)).ToList();

            // 1. 繼續處理，因為已經扣錢
            using var uowm = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Member);
            _uowAccessor.Current = uowm;

            // 2. 處理 Member 庫
            try
            {
                
                await uowm.BeginTransactionAsync();

                // 範例說明每筆皆會處理，實際上應改用 Bulk Update SQL 語法
                foreach (var sagaId in notPurchaseSagaIds)
                {
                    var processItem = collectionItems.FirstOrDefault(item => item.SagaId == sagaId);

                    if (processItem != null)
                    {
                        var insertData = new PurchaseDao()
                        {
                            SagaId = sagaId,
                            Amount = processItem.Amount,
                            MemberId = processItem.MemberId,
                            ProductId = processItem.ProductId,
                            Status = SagaTransactionStatusEnum.COMPLETED.ToString(),
                        };

                        await _memberRespository.InsertPurchase(insertData);
                        await _memberRespository.UpdateMemberSpentMoney(insertData.MemberId, insertData.Amount);
                    }
                }

                await uowm.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await uowm.RollbackAsync();
            }
        }

        /// <summary>
        /// 1-4. 補償機制 -> 第四步執行前中斷(寫入 Log + 扣款 Balance 表 + 本地 Member 表 結束) -> 繼續處理，已經完成只差在標記
        /// </summary>
        /// <returns></returns>
        private async Task CompensatorynotMarkComplate(List<string> notMarkComplateSagaIds)
        {
            // 1. 繼續處理，已經完成只差在標記
            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Log);
            _uowAccessor.Current = uow;
            try
            {
                await uow.BeginTransactionAsync();

                // 2. 範例說明每筆皆會處理，實際上應改用 Bulk Update SQL 語法
                foreach (var sagaId in notMarkComplateSagaIds)
                {
                    await _logRepository.UpdateLogStatus(sagaId, "COMPLETED");
                }

                await uow.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await uow.RollbackAsync();
            }
        }

    }
}
