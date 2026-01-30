using DistributedeSAGAWithMysql.Repository.Interface;
using Framework.Database.Interfaces;
using Dapper;

namespace DistributedeSAGAWithMysql.Repository.Instance
{
    public class LogRepository: ILogRepository
    {
        private readonly IUnitOfWorkAccessor _uowAccessor;

        public LogRepository(IUnitOfWorkAccessor uowAccessor)
        {
            _uowAccessor = uowAccessor;
        }

        public async Task BuildLog()
        {
            var uow = _uowAccessor.Current
                  ?? throw new InvalidOperationException("UoW 未初始化，請檢查 Service 層。");

            var temp = await uow.Connection.ExecuteScalarAsync<int>(
                    "Select * FROM SagaTransaction"
                );

        }
    }
}
