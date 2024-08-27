using Example.Common.FakeDataBase.Model;

namespace RabbitMQLoadBalanceAspCoreWebExample.Service
{
    public interface IAccountTradeOrder
    {
        Task FinishAccountTradeOrder(AccountTradeOrderModel tradeOrder);

        Task BuildAccountTradeOrder();
    }
}
