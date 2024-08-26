using RabbitMQ.Client;
using System.Text;

namespace Example.Common.RabbitMQ.Factory
{
    public class MqSender : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeType;

        //public MqSender(RabbitParameters rabbitParameters)
        public MqSender()
        {
            //_connection = MqConnectionHelper.GetConnection(rabbitParameters.HostName, rabbitParameters.UserName, rabbitParameters.Password);

            //_channel = MqConnectionHelper.GetModel(_connection);

            //_exchangeType = rabbitParameters.ExchangeType;

            //var exchangeName = rabbitParameters.ExchangeName;

            //_channel.ExchangeDeclare(exchangeName, rabbitParameters.ExchangeType.ToString().ToLower(), true, false, null);
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
        ~MqSender()
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
