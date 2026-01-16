using Dapper;
using Framework.Database.Enum;
using Framework.Database.Interfaces;
using Quartz;
using RedisDistributedLockExample.RedisDistributed;

namespace RedisDistributedLockExample.BackGround
{
    /// <summary>
    /// Quartz.NET 排程工作
    /// </summary>
    public class QuartzJobForDistributed : IJob
    {
        private readonly IDistributedLock _distributedLock;
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly ILogger<QuartzJobForDistributed> _logger;

        public QuartzJobForDistributed(IDistributedLock distributedLock,
            IUnitOfWorkFactory uowFactory,
            ILogger<QuartzJobForDistributed> logger)
        {
            _distributedLock = distributedLock;
            _uowFactory = uowFactory;
            _logger = logger;
        }

        /// <summary>
        /// 實際執行
        /// </summary>
        public async Task Execute(IJobExecutionContext context)
        {
            var redisKey = $@"{nameof(QuartzJobForDistributed)}";

            // 1. 當前運行的 Job (標記判斷)
            var sourceTag = context.MergedJobDataMap.GetString("SourceTag") ?? string.Empty;

            //// 2. Redis 阻塞鎖 :  WaitLockExecuteAsync
            //Console.WriteLine(DateTime.Now.ToLongTimeString());
            //await _distributedLock.WaitLockExecuteAsync(redisKey, async() =>
            //{
            //    // 3. 寫入資料庫，模擬併發碰撞
            //    await InsertData(sourceTag);

            //}, expiryTime: TimeSpan.FromSeconds(10),
            //   waitTime: TimeSpan.FromSeconds(2),
            //   retryTime: TimeSpan.FromSeconds(1));

            // 2. Redis 非阻塞鎖 : TryLockExecuteAsync
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            await _distributedLock.TryLockExecuteAsync(redisKey, async () =>
            {
                // 3. 寫入資料庫，模擬併發碰撞
                await InsertData(sourceTag);

            }, expiryTime: TimeSpan.FromSeconds(60) // 要給定合理時間，避免過久鎖住事務，未釋放
            );
        }

        /// <summary>
        /// 寫入到資料庫
        /// </summary>
        private async Task InsertData(string sourceTag)
        {
            var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.MilkTeaGreen);
            try
            {
                await uow.BeginTransactionAsync();

                var sql = $@"
INSERT INTO WorkerFlowMessage (ServiceName, Message) 
     VALUES (@ServiceName, @Message);                         
";

                var result = await uow.Connection.ExecuteAsync(
                    sql,
                    new
                    {
                        ServiceName = sourceTag,
                        Message = $@"{sourceTag} : 紀錄成功",
                    },
                    transaction: uow.Transaction
                );

                await uow.CommitAsync();
            }
            catch (Exception ex)
            {
                await uow.RollbackAsync();
                Console.WriteLine(ex);
            }
        }
    }
}
