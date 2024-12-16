using StackExchange.Redis;

namespace RedisProducerAndComsumerForPubAndSubExample.Service
{
    public class RedisPubSubService
    {
        private readonly ISubscriber _subscriber;
        private readonly ConnectionMultiplexer _redis;

        public RedisPubSubService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _subscriber = _redis.GetSubscriber();
        }

        public void Publish(string channel, string message)
        {
            _subscriber.Publish(channel, message);
        }

        public void Subscribe(string channel, Action<string> messageHandler)
        {
            _subscriber.Subscribe(channel, (redisChannel, value) =>
            {
                messageHandler(value);
            });
        }
    }
}
