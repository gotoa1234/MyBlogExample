using StackExchange.Redis;
namespace RedisNetLockExample.Redis
{
    public class RedisLockService : IRedixLockService
    {
        private readonly IDatabase _db;

        public RedisLockService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        /// <summary>
        /// 取得鎖
        /// </summary>
        public async Task<(bool Acquired, string LockValue)> AcquireAsync(
            string lockKey,
            TimeSpan ttl)
        {
            // 必須唯一，用來做 unlock ownership 驗證
            string lockValue = Guid.NewGuid().ToString("N");

            bool acquired = await _db.StringSetAsync(
                lockKey,
                lockValue,
                expiry: ttl,
                when: When.NotExists
            );

            return (acquired, lockValue);
        }

        /// <summary>
        /// 釋放鎖
        /// </summary>
        public async Task<bool> ReleaseAsync(string lockKey, string lockValue)
        {
            const string lua = @"
if redis.call('GET', KEYS[1]) == ARGV[1] then
    return redis.call('DEL', KEYS[1])
end
return 0
";
            var result = await _db.ScriptEvaluateAsync(
                lua,
                new RedisKey[] { lockKey },
                new RedisValue[] { lockValue }
            );

            return (int)result == 1;
        }
    }
}
