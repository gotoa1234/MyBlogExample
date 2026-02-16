using DistributedeSAGAWithMysql.Repository.Dao;

namespace DistributedeSAGAWithMysql.Repository.Interface
{
    public interface IBalanceRepository
    {
        Task UpdateBalance(long memberId, decimal totalCost);

        Task CreateBalanceTransaction(long memberId, decimal totalCost, long productId, string sagaId);

        Task<IEnumerable<BalanceTransactionDao>> GetBalanceTransactionItems(List<string> sagaIds);
    }
}
