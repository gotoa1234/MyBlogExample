using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Example.Common.RabbitMQ
{
    public class RabbitMqMessageReceiver<TMessage> : IDisposable
    {
        private ConnectionFactory _factory;
        private bool _disposed;
        private IModel _channel;
        private IConnection _connection;
        private EventingBasicConsumer _consumer;

        public RabbitMqMessageReceiver()
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

            //var type = rabbitParameters.ExchangeType.ToString().ToLower();
            //if (rabbitParameters.ExchangeType != ExchangeTypeEnum.Fanout)
            //{
            //    rabbitParameters.ExchangeName += $"_{type}";
            //}

            _exchangeType = rabbitParameters.ExchangeType;
            _exchangeName = rabbitParameters.ExchangeName;

            Connect();
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
