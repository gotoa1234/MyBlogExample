using DistributedeSAGAWithMysql.Repository.Interface;
using Framework.Database.Interfaces;
using Dapper;
using DistributedeSAGAWithMysql.Repository.Dao;

namespace DistributedeSAGAWithMysql.Repository.Instance
{
    public class LogRepository: ILogRepository
    {
        private readonly IUnitOfWorkAccessor _uowAccessor;

        public LogRepository(IUnitOfWorkAccessor uowAccessor)
        {
            _uowAccessor = uowAccessor;
        }

        public async Task<string> CreateLog(SagaTransactionDao insertData)
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");

            var insertSql = $@"
INSERT INTO SagaTransaction(`SagaId`, `MemberId`, `ProductId`, `Amount`, `Status`, `CreatedAt`, `UpdatedAt`)
                     VALUES(@SagaId, @MemberId, @ProductId, @Amount, @Status, NOW(), NOW());
";
            var guid = Guid.NewGuid().ToString("D");
            await uow.Connection.ExecuteAsync(insertSql, new {
                SagaId = guid,
                MemberId = insertData.MemberId,
                ProductId = insertData.ProductId,
                Amount = insertData.Amount,
                Status = insertData.Status
            });
            return guid;
        }

        public async Task UpdateLogStatus(string sagaId, string status)
        {
            var uow = _uowAccessor.Current
          ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");

            var updateSql = $@"
UPDATE SagaTransaction
   SET  Status = '{status}'
 WHERE SagaId = @SagaId
;";
            await uow.Connection.ExecuteAsync(updateSql, new
            {
                @SagaId = sagaId
            });
        }
    }
}
