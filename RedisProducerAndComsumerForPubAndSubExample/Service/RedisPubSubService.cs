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

        /// <summary>
        /// 1. 提供立即推送到 Redis 上 Subscribe 的對象
        /// </summary>
        public void Publish(string channel, string message)
        {
            _subscriber.Publish(channel, message);
        }

        /// <summary>
        /// 2. 提供對象進行訂閱，在這篇範例是背景服務
        /// </summary>
        public void Subscribe(string channel, Action<string> messageHandler)
        {
            _subscriber.Subscribe(channel, (redisChannel, value) =>
            {
                messageHandler(value);
            });
        }
    }
}
