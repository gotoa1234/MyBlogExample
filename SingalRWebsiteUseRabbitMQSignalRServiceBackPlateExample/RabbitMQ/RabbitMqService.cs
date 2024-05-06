using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.RabbitMQ
{
    /// <summary>
    /// 實作 RabbitMQ 單例模式
    /// </summary>
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueKey = "我是Queue的Key";
        private readonly string _exchangeName = "my-fanout-exchange";

        /// <summary>
        /// 1. 建立基本資訊
        /// </summary>
        public RabbitMqService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "127.0.0.1", // RabbitMQ 主機名
                UserName = "admin",
                Password = "123fff",
                Port = 5672 // RabbitMQ 連接埠
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            //宣告一個 fanout 類型的 Exchange
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, durable: true);
        }

        /// <summary>
        /// 2. 增加一個接口，可以將資料發送到 RabbitMQ
        /// </summary>        
        public void SendMessage(string message)
        {
            //Queue基本設置
            _channel.QueueDeclare(queue: _queueKey,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // 配置傳送到 RabbitMQ 上 Queue 的內容            
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: _exchangeName,
                                  routingKey: _queueKey,
                                  basicProperties: null,
                                  body: body);
        }

        /// <summary>
        /// 3. 增加一個接口，可以接收RabbitMQ 訊息
        /// </summary>        
        public void StartReceiving(Action<string> handleMessage)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                handleMessage(message);
            };

            // 創建一個臨時隊列，並將其與 fanout Exchange 綁定
            _channel.QueueDeclare(_queueKey, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(_queueKey, _exchangeName, "");

            _channel.BasicConsume(queue: _queueKey,
                                  autoAck: true,
                                  consumer: consumer);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
