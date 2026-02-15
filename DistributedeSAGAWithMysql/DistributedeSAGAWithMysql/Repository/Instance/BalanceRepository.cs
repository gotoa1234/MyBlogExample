using Dapper;
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
        public async Task CreateBalanceTransaction(long memberId, decimal totalCost, string sagaId)
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");
            var sql = $@"
INSERT BalanceTransaction(`MemberId`, `Amount`,`SagaId`, `CreatedAt`)
                   VALUES(@MemberId, @Amount, @SagaId, NOW() )
;";
            try
            {
                await uow.Connection.ExecuteAsync(sql, new
                {
                    @MemberId = memberId,
                    @Amount = totalCost,
                    @SagaId = sagaId,                    
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
