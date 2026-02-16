using DistributedeSAGAWithMysql.Enum;
using DistributedeSAGAWithMysql.Models;
using DistributedeSAGAWithMysql.Repository.Dao;

namespace DistributedeSAGAWithMysql.Service.Interface
{
    public interface IShoppingCartService
    {
        Task Shoppinng(RequestModel shoppingData, InterruptStepEnum stepEnum = InterruptStepEnum.None);
        Task<string> CreateLog(RequestModel shoppingData, ProductDao product);
        Task Deduction(RequestModel shoppingData, ProductDao product, string sagaId);
        Task UpdateMemberAndProudct(RequestModel shoppingData, ProductDao product, string sagaId);
        Task FinishLog(string sagaId);
        Task<ProductDao> GetProductInfo(long productId);
        Task<MemberDao> GetMemberInfo(long memberId);
    }
}
