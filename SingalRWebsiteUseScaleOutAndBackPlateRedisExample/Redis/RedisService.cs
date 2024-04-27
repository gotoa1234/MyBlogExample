using StackExchange.Redis;

namespace SingalRWebsiteUseScaleOutAndBackPlateRedisExample.Redis
{
    /// <summary>
    /// 實作 Redis SingleTon 的類
    /// </summary>
    public class RedisService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ISubscriber _redisSubscriber;

        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _redisSubscriber = _redis.GetSubscriber();
        }

        public IDatabase GetDb(int dbIndex = 0) => _redis.GetDatabase(dbIndex);
        public ISubscriber GetSubscriber() => _redis.GetSubscriber();
        public void Publish(RedisChannel channel, RedisValue message)
        {
            _redisSubscriber.Publish(channel, message);
        }
    }
}