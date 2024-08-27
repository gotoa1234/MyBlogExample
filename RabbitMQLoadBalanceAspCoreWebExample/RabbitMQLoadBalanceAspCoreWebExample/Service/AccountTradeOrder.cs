using Bogus;
using Example.Common.FakeDataBase;
using Example.Common.FakeDataBase.Model;
using Example.Common.RabbitMQ.Consts;
using Example.Common.RabbitMQ.Factory;
using System.Text.Json;

namespace RabbitMQLoadBalanceAspCoreWebExample.Service
{
    public class AccountTradeOrder : IAccountTradeOrder
    {
        private ILogger<AccountTradeOrder> _logger;
        private readonly IRabbitMqFactory _mqFactory;
        private readonly FakeDataBase _fakeDb;        

        public AccountTradeOrder(ILogger<AccountTradeOrder> logger,
            IRabbitMqFactory mqFactory)
        {
            _logger = logger;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountTradeOrderModel> GetAccountTraderOrder()
        {
            var result = _fakeDb.GetAccountTradeOrderAll().Values;
            return result;
        }

        /// <summary>
        /// 建立訂單
        /// </summary>
        public Task BuildAccountTradeOrder()
        {
            // Bogus 套件，目的是產生假資料
            var faker = new Faker("zh_CN");
            var newItem = new AccountTradeOrderModel()
            {
                AccountName = faker.Name.FullName(),
                AccountTradeOrderId = Guid.NewGuid().GetHashCode(),
                IsSuccessful = false,
                Remark = $@"建立時間 : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"
            };
            _fakeDb.AddOrUpdate(newItem);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 更新訂單
        /// </summary>        
        public Task FinishAccountTradeOrder(AccountTradeOrderModel tradeOrder)
        {
            // 模擬處理為成功
            tradeOrder.IsSuccessful = true;
            _fakeDb.AddOrUpdate(tradeOrder);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 提交到 RabbitMQ 隊列
        /// </summary>
        private void SendToRabbitMQ(AccountTradeOrderModel tradeOrder)
        {
            var json = JsonSerializer.Serialize(tradeOrder);
            var sender = _mqFactory.Get(RabbitMQConsts.MYEXCHANGENAME, RabbitMQConsts.MYEXCHANGENAME);
            sender.Send(json, RabbitMQConsts.MYEXCHANGENAME, RabbitMQConsts.MYEXCHANGENAME);
        }
    }
}
