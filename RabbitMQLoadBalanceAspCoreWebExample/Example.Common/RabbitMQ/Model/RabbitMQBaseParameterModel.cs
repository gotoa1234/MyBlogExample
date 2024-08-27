using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Example.Common.RabbitMQ.Model
{
    public class RabbitMQBaseParameterModel
    {
        protected bool _disposed;
        protected IModel _channel;
        protected IConnection _connection;
        protected EventingBasicConsumer _consumer;
        protected int _concurrentCount;
        protected bool _autoAck;
        protected string _exchangeType;
        protected string _exchangeName;
        protected string _queueName;
        protected string _routingKey;
    }
}
