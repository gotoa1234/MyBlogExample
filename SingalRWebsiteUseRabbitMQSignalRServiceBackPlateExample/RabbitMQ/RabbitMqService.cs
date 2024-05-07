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
        private readonly string _exchangeName = "my-fanout-exchange";

        /// <summary>
        /// 1-1. 建立基本資訊
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
            // 1-2. 初始化 RabbitMQ 連線
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // 1-3. 選擇 Fanout 類型的 Exchange ，註冊此 Exchange 的 RabbitMQ 客戶端(Queue)，都會收到廣播訊息
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);
        }

        /// <summary>
        /// 2-1. 增加一個接口，可以將資料發送到 RabbitMQ
        /// </summary>        
        public void SendMessage(string message)
        {
            // 2-2. 配置傳送到 RabbitMQ 上 Queue 的內容            
            var body = Encoding.UTF8.GetBytes(message);            

            // 2-3. FanOut 基本配置 (關注 Exchange 位置)
            _channel.BasicPublish(exchange: _exchangeName,
                                  routingKey: string.Empty,
                                  basicProperties: null,
                                  body: body);
        }

        /// <summary>
        /// 3-1. 增加一個接口，可以接收RabbitMQ 訊息
        /// </summary>        
        public void StartReceiving(Action<string> handleMessage)
        {   
            // 3-2. 建立一個匿名委派
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, ea) =>
            {
                // 每當從 Exchange 上收到資料時都會自動觸發 Received 事件
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                // 並將資料回傳給 BackgroundService 上的註冊事件
                handleMessage(message);
            };

            // 3-3. 創建一個臨時隊列，並將其與設定的 FanOut Exchange 綁定
            // ※臨時佇列 - 釋放資源即刪除，可以動態擴容站點
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: string.Empty);
            _channel.BasicConsume(queue: queueName,
                                  autoAck: true,// True: 消費者收到視為處理完畢
                                  consumer: consumer);// 訂閱的結果事件傳給消費者
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
