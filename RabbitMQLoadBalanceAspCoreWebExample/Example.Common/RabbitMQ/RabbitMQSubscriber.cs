using Example.Common.RabbitMQ.Model;
using Microsoft.Extensions.Configuration;

namespace Example.Common.RabbitMQ
{
    public class RabbitMQSubscriber
    {
        private readonly IConfiguration _configuration;
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;

        /// <summary>
        /// 建構式
        /// </summary>        
        public RabbitMQSubscriber(string rabbitMqHostName, string rabbitMqUserName, string rabbitMqPassword)
        {
            var rabbitParam = _configuration.GetSection("RabbitMQ").Get<BaseModel>();
            _hostName = rabbitMqHostName;
            _userName = rabbitMqUserName;
            _password = rabbitMqPassword;
        }

        /// <summary>
        /// 建立接收器
        /// </summary>
        public void BuildReceive()
        {
            // 建立批次訂單接收器
            _orderBatchSequenceReceiver = new(new RabbitParameters
            {
                HostName = _rabbitMqHostName,
                UserName = _rabbitMqUserName,
                Password = _rabbitMqPassword,
                ExchangeName = MqConsts.ExchangeTradeOrderBatchSequence,
                ExchangeType = ExchangeTypeEnum.Direct
            }, 5, OnBatchOrderReceived);

            // Direct 模式要帶Key
            _orderBatchSequenceReceiver.AddQueue(MqConsts.QueueTradeOrderBatchSequence, MqConsts.RoutingKeyTradeOrderBatchSequence);
        }

    }
}
