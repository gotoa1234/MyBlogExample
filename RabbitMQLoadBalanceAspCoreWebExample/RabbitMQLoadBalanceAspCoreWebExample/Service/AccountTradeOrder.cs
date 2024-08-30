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
        private readonly IRabbitMqFactory _mqFactory;
        private readonly FakeDataBase _fakeDb;        

        public AccountTradeOrder(
            IRabbitMqFactory mqFactory,
            FakeDataBase fakeDb)
        {
            _mqFactory = mqFactory;
            _fakeDb = fakeDb;
        }

        /// <summary>
        /// 1. 查詢訂單
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AccountTradeOrderModel> GetAccountTraderOrder()
        {
            var result = _fakeDb.GetAccountTradeOrderAll().Values;
            return result;
        }

        /// <summary>
        /// 2-1. 建立訂單
        /// </summary>
        public Task BuildAccountTradeOrder()
        {
            // Bogus 套件，目的是產生假資料
            var faker = new Faker("zh_CN");
            var now = DateTime.Now;
            var newItem = new AccountTradeOrderModel()
            {
                AccountName = faker.Name.FullName(),
                AccountTradeOrderId = Guid.NewGuid().GetHashCode(),
                IsSuccessful = false,
                DateTimeValue = now,
                Remark = $@"建立時間 : {now.ToString("yyyy-MM-dd HH:mm:ss")}"
            };
            // 新增至資料庫
            _fakeDb.AddOrUpdate(newItem);

            // 發送至 RabbitMQ
            SendToRabbitMQ(newItem);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 3. 更新訂單
        /// </summary>        
        public Task FinishAccountTradeOrder(AccountTradeOrderModel tradeOrder)
        {
            // 模擬處理為成功
            tradeOrder.IsSuccessful = true;
            tradeOrder.MechineName = tradeOrder.MechineName;//從消費者處理時才寫入
            // 更新至資料庫
            _fakeDb.AddOrUpdate(tradeOrder);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 2-2. 提交到 RabbitMQ 隊列
        /// </summary>
        private void SendToRabbitMQ(AccountTradeOrderModel tradeOrder)
        {
            var json = JsonSerializer.Serialize(tradeOrder);
            var publisher = _mqFactory.Get(RabbitMQConsts.MY_EXCHANGE_NAME);
            publisher.PblisherSend(json, RabbitMQConsts.MY_EXCHANGE_NAME, RabbitMQConsts.MY_ROUTING_KEY);
        }
    }
}
