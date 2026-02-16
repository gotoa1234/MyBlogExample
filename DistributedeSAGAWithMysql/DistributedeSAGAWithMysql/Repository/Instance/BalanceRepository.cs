using Dapper;
using DistributedeSAGAWithMysql.Repository.Dao;
using DistributedeSAGAWithMysql.Repository.Interface;
using Framework.Database.Interfaces;

namespace DistributedeSAGAWithMysql.Repository.Instance
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly IUnitOfWorkAccessor _uowAccessor;

        public BalanceRepository(IUnitOfWorkAccessor uowAccessor)
        {
            _uowAccessor = uowAccessor;
        }

        /// <summary>
        /// 扣除會員額度
        /// </summary>
        public async Task UpdateBalance(long memberId, decimal totalCost)
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");
            var sql = $@"
UPDATE AccountBalance
   SET Balance = Balance - @TotalCost
 WHERE MemberId = @MemberId
;";
            try
            {
                await uow.Connection.ExecuteAsync(sql, new
                {
                    @MemberId = memberId,
                    @TotalCost = totalCost
                });                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 紀錄餘額庫的交易
        /// </summary>        
        public async Task CreateBalanceTransaction(long memberId, decimal totalCost,long productId, string sagaId)
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");
            var sql = $@"
INSERT BalanceTransaction(`MemberId`, `Amount`,`SagaId`,`ProductId`, `CreatedAt`)
                   VALUES(@MemberId, @Amount, @SagaId, @ProductId, NOW() )
;";
            try
            {
                await uow.Connection.ExecuteAsync(sql, new
                {
                    @MemberId = memberId,
                    @Amount = totalCost,
                    @SagaId = sagaId,
                    @ProductId = productId,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 取得對應的 SagaIds 的資料
        /// </summary>
        public async Task<IEnumerable<BalanceTransactionDao>> GetBalanceTransactionItems(List<string> sagaIds)
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");
            var sql = $@"
SELECT SagaId,
       MemberId,
       Amount,
       ProductId
  FROM BalanceTransaction
 WHERE SagaId = @SagaIds
;";
            try
            {
                return await uow.Connection.QueryAsync<BalanceTransactionDao>(sql, new
                {
                    SagaIds = sagaIds
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
