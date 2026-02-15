using Dapper;
using DistributedeSAGAWithMysql.Repository.Dao;
using DistributedeSAGAWithMysql.Repository.Interface;
using Framework.Database.Interfaces;

namespace DistributedeSAGAWithMysql.Repository.Instance
{
    public class MemberRepository: IMemberRepository
    {
        private readonly IUnitOfWorkAccessor _uowAccessor;

        public MemberRepository(IUnitOfWorkAccessor uowAccessor)
        {
            _uowAccessor = uowAccessor;
        }

        /// <summary>
        /// 取得會員資訊
        /// </summary>
        public async Task<MemberDao> GetMember(long memberId)
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");
            var sql = $@"
SELECT *
  FROM Member
 WHERE MemberId = @MemberId
;";
            try
            {
                var getResult = await uow.Connection.QueryFirstAsync<MemberDao>(sql, new
                {
                    @MemberId = memberId
                });
                return getResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new MemberDao();
            }
        }

        /// <summary>
        /// 取得產品資訊
        /// </summary>
        public async Task<ProductDao> GetProduct(long productId)
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");
            var sql = $@"
SELECT ProductId,
       ProductName,
       Price
  FROM Product
 WHERE ProductId = @ProductId
;";
            try
            {
                var getResult = await uow.Connection.QueryFirstAsync<ProductDao>(sql, new
                {
                    @ProductId = productId
                });
                return getResult;
            }
            catch (Exception ex)
            {
                return new ProductDao();
            }
        }

        /// <summary>
        /// 寫入產品紀錄
        /// </summary>
        public async Task InsertPurchase(PurchaseDao insertData)
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");
            var sql = $@"
INSERT INTO Purchase(`SagaId`, `MemberId`, `ProductId`, `Amount`, `Status`,`CreatedAt` )
              VALUES(@SagaId, @MemberId, @ProductId, @Amount, @Status, NOW());
;";
            try
            {
                await uow.Connection.ExecuteAsync(sql, new
                {
                    @SagaId = insertData.SagaId,
                    @MemberId = insertData.MemberId,
                    @ProductId = insertData.ProductId,
                    @Amount = insertData.Amount,
                    @Status = insertData.Status
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 更新會員消費金額
        /// </summary>
        public async Task UpdateMemberSpentMoney(long memberId, decimal spentMoney)
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");
            var sql = $@"
UPDATE Member
   SET TotalSpent = TotalSpent + @SpentMoney
 WHERE   MemberId = @MemberId
;";
            try
            {
                await uow.Connection.ExecuteAsync(sql, new
                {
                    @SpentMoney = spentMoney,
                    @MemberId = memberId,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
