using DistributedeSAGAWithMysql.Repository.Dao;

namespace DistributedeSAGAWithMysql.Repository.Interface
{
    public interface IMemberRepository
    {
        Task<MemberDao> GetMember(long memberId);
        Task<ProductDao> GetProduct(long productId);
        Task InsertPurchase(PurchaseDao insertData);
        Task UpdateMemberSpentMoney(long memberId, decimal spentMoney);
    }
}
