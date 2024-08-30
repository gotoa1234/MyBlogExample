using Example.Common.RabbitMQ.Common;
using Example.Common.RabbitMQ.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Example.Common.RabbitMQ
{
    public class RabbitMqMessageReceiver<TMessage> : RabbitMQBaseParameterModel, IDisposable
    {
        protected ConnectionFactory _factory;
        protected Action<TMessage, RabbitMqMessageReceiver<TMessage>, ulong, long> _receivedAction;

        /// <summary>
        /// 1. 建構式 - 消費者接收器
        /// </summary>        
        public RabbitMqMessageReceiver(
            ExchangeModel rabbitParameters,
            int concurrentCount,
            Action<TMessage, RabbitMqMessageReceiver<TMessage>, ulong, long> action,
            bool autoAck = false)
        {
            SettingValue();
            Connect();

            // 設定消費者接收器 - 基本設定
            void SettingValue()
            {
                _concurrentCount = concurrentCount;
                _receivedAction = action;
                _autoAck = autoAck;
                _factory = RabbitMQHelper.GetConstructFactory(rabbitParameters);
                _exchangeType = rabbitParameters.ExchangeType;
                _exchangeName = rabbitParameters.ExchangeName;
            }
        }

        #region 連線事件

        /// <summary>
        /// 2. 設定消費者接收器 - 連線
        /// </summary>
        public void Connect()
        {
            // 建立連線
            _connection = _factory.CreateConnection();
            _connection.ConnectionShutdown += ConnectionForConnectionShutdown;
            _channel = _connection.CreateModel();

            // 宣告交換機 - 確保存在
            _channel.ExchangeDeclare(_exchangeName, _exchangeType.ToString().ToLower(), true, false, null);

            // 消費者接收事件
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += OnConsumerForReceived;
        }

        /// <summary>
        /// 3. 連線結束事件
        /// </summary>        
        private void ConnectionForConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            ReleaseResource();
            RetryProcess();

            // 釋出資源
            void ReleaseResource()
            {
                try
                {
                    _consumer = null!;
                    _channel?.Dispose();
                    _connection?.Dispose();
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLineAsync(ex.Message);
                    throw;
                }
            }

            // 重連機制
            void RetryProcess()
            {
                var retryMaxTimes = 10;
                var retryTimes = 0;
                while (retryTimes <= retryMaxTimes)
                {
                    try
                    {
                        retryTimes++;
                        Connect();
                        AddQueue(_queueName, _routingKey);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLineAsync(ex.Message);
                    }

                    // 指數延遲 : 2 的 tryTimes 次方 最大 10 秒
                    var delaySeconds = Math.Min(10000, 1000 * (int)Math.Pow(2, retryTimes));
                    Task.Delay(delaySeconds);
                }

                // 避免無限循環
                if (retryTimes > retryMaxTimes)
                {
                    Console.Out.WriteLineAsync($@"Exceeded maximum {retryMaxTimes} retry attempts.");
                }
            }
        }

        /// <summary>
        /// 4. 消費者接收器
        /// </summary>
        public void OnConsumerForReceived(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var bodyStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var message = JsonSerializer.Deserialize<TMessage>(bodyStr);
                _receivedAction.Invoke(message, this, e.DeliveryTag, e.BasicProperties.Timestamp.UnixTime);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 5. 增加消費者訂閱的隊列
        /// </summary>
        public void AddQueue(string queueName, string routingKey)
        {
            _queueName = queueName;
            _routingKey = routingKey;

            _channel.QueueDeclare(queueName, true, false, false, null);
            _channel.QueueBind(queueName, _exchangeName, routingKey, null);

            _channel.BasicQos(0, (ushort)_concurrentCount, false);
            _channel.BasicConsume(queueName, _autoAck, _consumer);
        }
        #endregion

        #region RabbitMQ 反饋 Ack 標記

        /// <summary>
        /// 6. 標記訊息已處理
        /// </summary>        
        public async Task BasicAck(ulong deliveryTag)
        {
            try
            {
                if (!_autoAck)
                {
                    _channel.BasicAck(deliveryTag, false);
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($@"RabbitMqMessageReceiver BasicAck error:{ex}");
                throw;
            }
        }

        #endregion

        #region 解構式 - 7. 釋放資源

        ~RabbitMqMessageReceiver()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                {
                    _channel?.Dispose();
                    _connection?.Dispose();
                }
                _disposed = true;
            }

            #endregion
        }
    }
}