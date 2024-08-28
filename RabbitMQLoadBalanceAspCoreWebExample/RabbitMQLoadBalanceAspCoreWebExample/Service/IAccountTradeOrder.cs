using Example.Common.FakeDataBase.Model;

namespace RabbitMQLoadBalanceAspCoreWebExample.Service
{
    public interface IAccountTradeOrder
    {
        IEnumerable<AccountTradeOrderModel> GetAccountTraderOrder();

        Task FinishAccountTradeOrder(AccountTradeOrderModel tradeOrder);

        Task BuildAccountTradeOrder();
    }
}
