using Example.Common.RabbitMQ.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Example.Common.RabbitMQ
{
    public class BaseParameter
    {
        protected bool _disposed;
        protected IModel _channel;
        protected IConnection _connection;
        protected EventingBasicConsumer _consumer;
        protected int _concurrentCount;
        protected bool _autoAck;
        protected string _exchangeType;
        protected string _exchangeName;
    }
    
    public class RabbitMqMessageReceiver<TMessage> : BaseParameter, IDisposable
    {
        protected ConnectionFactory _factory;
        protected Action<TMessage, RabbitMqMessageReceiver<TMessage>, ulong, long> _receivedAction;

        public RabbitMqMessageReceiver(
            BaseModel rabbitParameters, 
            int concurrentCount, 
            Action<TMessage, RabbitMqMessageReceiver<TMessage>, ulong, long> action, 
            bool autoAck = false)
        {
            _concurrentCount = concurrentCount;
            _receivedAction = action;
            _autoAck = autoAck;

            _factory = new ConnectionFactory
            {
                HostName = rabbitParameters.HostName,
                UserName = rabbitParameters.UserName,
                Password = rabbitParameters.Password,

                RequestedHeartbeat = TimeSpan.FromSeconds(30),
                AutomaticRecoveryEnabled = true, // 自動重連
                TopologyRecoveryEnabled = true, // 
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _exchangeType = rabbitParameters.ExchangeType;
            _exchangeName = rabbitParameters.ExchangeName;

            Connect();
        }

        public void Connect()
        {
            _connection = _factory.CreateConnection();
            _connection.ConnectionShutdown += Connection_ConnectionShutdown;
            _channel = _connection.CreateModel();

            // 这个主要防止在先启动消费端时候找不到交换机的问题
            _channel.ExchangeDeclare(_exchangeName, _exchangeType.ToString().ToLower(), true, false, null);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += OnConsumerReceived;
        }

        private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {            
            try
            {
                _consumer = null!;
                _channel?.Dispose();
                _connection?.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            int tryTimes = 0;
            while (true)
            {
                try
                {
                    tryTimes++;
                    Log.Information($"RabbitCommonReceiver try reconnect,TryTimes={tryTimes},ExchangeName={_exchangeName},Queue={_queueName}");
                    Connect();
                    AddQueue(_queueName, _routingKey);

                    break;
                }
                catch (Exception ex)
                {
                    Log.Error($"RabbitCommonReceiver reconnect error:{ex}");
                }

                if (tryTimes > 100)
                {
                    Log.Error($"RabbitCommonReceiver reconnect tried {tryTimes} times,but failed.ExchangeName={_exchangeName}");
                    break;
                }
                var sleepMs = 1000 * (tryTimes > 10 ? 10 : tryTimes);
                Thread.Sleep(sleepMs);
            }
        }


        public void OnConsumerReceived<T>(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var bodyStr = Encoding.UTF8.GetString(e.Body.ToArray());
                var message = JsonSerializer.Deserialize<TMessage>(bodyStr);
                _receivedAction.Invoke(message, this, e.DeliveryTag, e.BasicProperties.Timestamp.UnixTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                _factory = null!;
            }
            _channel?.Dispose();
            _connection?.Dispose();
            _disposed = true;
        }
    }
}
