using DistributedeSAGAWithMysql.Repository.Interface;
using Framework.Database.Enum;
using Framework.Database.Interfaces;
using Quartz;

namespace DistributedeSAGAWithMysql.BackGround
{
    public class QuartzJobForCompensatory : IJob
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly ILogger<QuartzJobForCompensatory> _logger;
        private readonly IBalanceRepository _balanceRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMemberRepository _memberRespository;


        public QuartzJobForCompensatory(
            IUnitOfWorkFactory uowFactory,
            ILogger<QuartzJobForCompensatory> logger,
            IBalanceRepository balanceRepository,
            ILogRepository logRepository,
            IMemberRepository memberRespository)
        {
            _uowFactory = uowFactory;
            _logger = logger;
            _balanceRepository = balanceRepository;
            _logRepository = logRepository;
            _memberRespository = memberRespository;
        }

        /// <summary>
        /// 實際執行
        /// </summary>
        public async Task Execute(IJobExecutionContext context)
        {
            //await InsertData();
        }

        /// <summary>
        /// 寫入到資料庫
        /// </summary>
        private async Task InsertData(string sourceTag)
        {
//            var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Member);
//            try
//            {
//                await uow.BeginTransactionAsync();

//                var sql = $@"
//INSERT INTO WorkerFlowMessage (ServiceName, Message) 
//     VALUES (@ServiceName, @Message);                         
//";

//                var result = await uow.Connection.ExecuteAsync(
//                    sql,
//                    new
//                    {
//                        ServiceName = sourceTag,
//                        Message = $@"{sourceTag} : 紀錄成功",
//                    },
//                    transaction: uow.Transaction
//                );

//                await uow.CommitAsync();
//            }
//            catch (Exception ex)
//            {
//                await uow.RollbackAsync();
//                Console.WriteLine(ex);
//            }
        }
    }
}
