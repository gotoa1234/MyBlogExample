using DistributedeSAGAWithMysql.Enum;
using DistributedeSAGAWithMysql.Models;
using DistributedeSAGAWithMysql.Repository.Dao;
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
            var prdouctInfo = await GetProductInfo(shoppingData.ProductId);
            var memberInfo = await GetMemberInfo(shoppingData.MemberId);

            // 簡易驗證
            if (memberInfo == null || prdouctInfo == null)
                throw new Exception("傳入參數錯誤");

            // 1.Log DB：建立交易紀錄(Pending)
            var sagaId = await CreateLog(shoppingData, prdouctInfo);

            // 2.Balance DB：扣款
            await Deduction(shoppingData, prdouctInfo, sagaId);

            // 3.Member DB：更新會員消費 / 訂單狀態
            await UpdateMemberAndProudct(shoppingData, prdouctInfo, sagaId);

            // 4.Log DB：更新交易狀態 = Completed
            await FinishLog(sagaId);
        }

        /// <summary>
        /// 1. 建立交易紀錄 => DB : Log
        /// </summary>
        public async Task<string> CreateLog(RequestModel shoppingData, ProductDao product)
        {
            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Log);            
            _uowAccessor.Current = uow;

            var insertData = new SagaTransactionDao() 
            {
                 Amount = product.Price * shoppingData.Count,
                 MemberId = shoppingData.MemberId,
                 ProductId = product.ProductId,
                 Status = SagaTransactionStatusEnum.PENDING.ToString(),                  
            };
            var sagaId = await _logRepository.CreateLog(insertData);

            return sagaId;
        }

        /// <summary>
        /// 2. 扣款 => DB : Balance
        /// </summary>
        public async Task Deduction(RequestModel shoppingData, ProductDao product, string sagaId)
        {            
            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Balance);
            _uowAccessor.Current = uow;
            var totalCost = product.Price * shoppingData.Count;
            try
            {
                await uow.BeginTransactionAsync();

                await _balanceRepository.UpdateBalance(shoppingData.MemberId, totalCost);                
                
                await _balanceRepository.CreateBalanceTransaction(shoppingData.MemberId, totalCost, sagaId);

                await uow.CommitAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await uow.RollbackAsync();
            }

        }

        /// <summary>
        /// 3. 更新會員消費 / 訂單狀態 => DB : Member
        /// </summary>
        public async Task UpdateMemberAndProudct(RequestModel shoppingData, ProductDao product, string sagaId)
        {
            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Member);
            _uowAccessor.Current = uow;
            var totalCost = product.Price * shoppingData.Count;

            var insertData = new PurchaseDao()
            {
                SagaId = sagaId,
                Amount = totalCost,
                MemberId = shoppingData.MemberId,
                ProductId = product.ProductId,
                Status = SagaTransactionStatusEnum.PENDING.ToString(),
            };

            try
            {
                await uow.BeginTransactionAsync();

                await _memberRespository.InsertPurchase(insertData);
                await _memberRespository.UpdateMemberSpentMoney(shoppingData.MemberId, totalCost);

                await uow.CommitAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await uow.RollbackAsync();
            }

        }

        /// <summary>
        // /4. 更新交易狀態 = Completed => DB : Log
        /// </summary>
        public async Task FinishLog(string sagaId)
        {
            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Log);
            _uowAccessor.Current = uow;

            try
            {
                await _logRepository.UpdateLogStatus(sagaId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        /// <summary>
        /// 取得產品資訊
        /// </summary>
        private async Task<ProductDao> GetProductInfo(long productId)
        {
            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Member);            
            _uowAccessor.Current = uow;
            return await _memberRespository.GetProduct(productId);
        }

        /// <summary>
        /// 取得系統內會員資料
        /// </summary>
        private async Task<MemberDao> GetMemberInfo(long memberId)
        {
            using var uow = await _uowFactory.CreateAsync(MysqlDbConnectionEnum.Member);
            _uowAccessor.Current = uow;
            return await _memberRespository.GetMember(memberId);
        }
    }
}
