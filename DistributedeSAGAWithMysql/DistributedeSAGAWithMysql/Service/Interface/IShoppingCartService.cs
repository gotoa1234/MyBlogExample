using DistributedeSAGAWithMysql.Models;

namespace DistributedeSAGAWithMysql.Service.Interface
{
    public interface IShoppingCartService
    {
        Task Shoppinng(RequestModel shoppingData);
    }
}
