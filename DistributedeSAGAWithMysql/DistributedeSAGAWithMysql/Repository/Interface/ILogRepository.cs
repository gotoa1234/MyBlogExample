using DistributedeSAGAWithMysql.Repository.Dao;

namespace DistributedeSAGAWithMysql.Repository.Interface
{
    public interface ILogRepository
    {
        Task<string> CreateLog(SagaTransactionDao insertData);

        Task UpdateLogStatus(string sagaId);
    }
}
