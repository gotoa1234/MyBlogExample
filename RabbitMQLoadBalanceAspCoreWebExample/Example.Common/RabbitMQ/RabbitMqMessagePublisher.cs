using Example.Common.RabbitMQ.Common;
using Example.Common.RabbitMQ.Model;
using RabbitMQ.Client;
using System.Text;

namespace Example.Common.RabbitMQ
{
    public class RabbitMqMessagePublisher : RabbitMQBaseParameterModel, IDisposable
    {
        public RabbitMqMessagePublisher(ExchangeModel rabbitParameters)
        {
            _connection = RabbitMQHelper.GetConnection(rabbitParameters);

            _channel = RabbitMQHelper.GetModel(_connection);

            _exchangeType = rabbitParameters.ExchangeType;

            var exchangeName = rabbitParameters.ExchangeName;

            _channel.ExchangeDeclare(exchangeName, rabbitParameters.ExchangeType.ToString().ToLower(), true, false, null);
        }

        /// <summary>
        /// Returns true if the connection is still in a state where it can be used.
        /// </summary>
        public bool IsOpen => _connection?.IsOpen == true;

        /// <summary>
        /// Published a message.
        /// </summary>        
        public void Send(string message, string exchangeName, string routingKey)
        {
            var properties = _channel.CreateBasicProperties();
            properties.Timestamp = new AmqpTimestamp(DateTime.Now.Ticks);
            properties.Persistent = true;
            _channel.BasicPublish(exchangeName, routingKey, false, properties, Encoding.UTF8.GetBytes(message));
        }

        #region Disposing

        private bool _disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~RabbitMqMessagePublisher()
        {
            Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _channel?.Dispose();
            _connection?.Dispose();
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
