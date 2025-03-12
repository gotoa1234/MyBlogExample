using StackExchange.Redis;

namespace RedisProducerAndComsumerForListExample.Service
{
    public class RedisQueueService
    {
        private readonly IDatabase _database;
        private readonly ConnectionMultiplexer _redis;

        public RedisQueueService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _database = _redis.GetDatabase();
        }

        /// <summary>
        /// 1. 將資料送到 Redis 的 Queue 中
        /// </summary>
        public void Enqueue(string queueName, string message)
        {
            _database.ListRightPush(queueName, message);
        }

        /// <summary>
        /// 2. 背景服務會從 Redis 的 Queue 中取出
        /// </summary>
        public string Dequeue(string queueName)
        {
            return _database.ListLeftPop(queueName);
        }
    }
}
