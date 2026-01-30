using DistributedeSAGAWithMysql.Models;
using DistributedeSAGAWithMysql.Repository.Interface;
using DistributedeSAGAWithMysql.Service.Interface;
using Framework.Database.Enum;
using Framework.Database.Interfaces;

namespace DistributedeSAGAWithMysql.Service.Instance
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IUnitOfWorkAccessor _uowAccessor;
        private readonly ILogger<ShoppingCartService> _logger;
        private readonly IBalanceRepository _balanceRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMemberRepository _memberRespository;

        public ShoppingCartService(IUnitOfWorkFactory uowFactory,
            IUnitOfWorkAccessor uowAccessor,
            ILogger<ShoppingCartService> logger,
            IBalanceRepository balanceRepository,
            ILogRepository logRepository,
            IMemberRepository memberRespository)
        {
            _uowFactory = uowFactory;
            _uowAccessor = uowAccessor;
            _logger = logger;
            _balanceRepository = balanceRepository;
            _logRepository = logRepository;
            _memberRespository = memberRespository;
        }

        public async Task Shoppinng(RequestModel shoppingData)
        {
            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Log);

            // 設定 Accessor
            _uowAccessor.Current = uow;

            // 1.Log DB：建立交易紀錄(Pending)
            await BuildLog();
            // 2.Balance DB：扣款
            Deduction();
            // 3.Member DB：更新會員消費 / 訂單狀態
            UpdateMemberAndProudct();
            // 4.Log DB：更新交易狀態 = Completed
            FinishLog();
        }

        /// <summary>
        /// 1. 建立交易紀錄 => DB : Log
        /// </summary>
        public async Task BuildLog()
        {
            await _logRepository.BuildLog();
        }

        /// <summary>
        /// 2. 扣款 => DB : Balance
        /// </summary>
        public void Deduction()
        {

        }

        /// <summary>
        /// 3. 更新會員消費 / 訂單狀態 => DB : Member
        /// </summary>
        public void UpdateMemberAndProudct()
        {

        }

        /// <summary>
        // /4. 更新交易狀態 = Completed => DB : Log
        /// </summary>
        public void FinishLog()
        {

        }
    }
}
