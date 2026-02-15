namespace DistributedeSAGAWithMysql.Repository.Interface
{
    public interface IBalanceRepository
    {
        Task UpdateBalance(long memberId, decimal totalCost);

        Task CreateBalanceTransaction(long memberId, decimal totalCost, string sagaId);
    }
}
