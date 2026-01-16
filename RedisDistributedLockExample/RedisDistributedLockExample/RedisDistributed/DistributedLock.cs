using RedLockNet;

namespace RedisDistributedLockExample.RedisDistributed
{
    public class DistributedLock : IDistributedLock
    {
        private readonly IDistributedLockFactory _lockFactory;
        private readonly string _keyPrefix;

        public delegate void LockWaitTimeoutHandler(string message);

        public event LockWaitTimeoutHandler? LockWaitTimeoutEvent;

        public DistributedLock(IDistributedLockFactory lockFactory, string lockKeyPrefix)
        {
            this._lockFactory = lockFactory ?? throw new ArgumentNullException(nameof(lockFactory));
            this._keyPrefix = string.IsNullOrEmpty(lockKeyPrefix) ? throw new ArgumentException("lockKeyPrefix cannot be empty") : lockKeyPrefix;
        }

        /// <summary>
        /// 阻塞式
        /// </summary>
        public async Task WaitLockExecuteAsync(string redisKey, Func<Task> act, TimeSpan expiry = default, TimeSpan wait = default, TimeSpan retry = default)
        {
            if (expiry == default(TimeSpan))// 未傳入時，預設 expire 時間
                expiry = TimeSpan.FromSeconds(2);

            if (wait == default(TimeSpan)) // 未傳入時，預設放棄重試時間
                wait = TimeSpan.FromSeconds(1);

            if (retry == default(TimeSpan)) // 未傳入時，預設重試間隔時間
                retry = TimeSpan.FromMilliseconds(20);

            var redisLockKey = $"{_keyPrefix}_{redisKey}";

            // 使用 await using 自動管理生命週期
            await using var redLock = await _lockFactory.CreateLockAsync(
                resource: redisLockKey,
                expiryTime: expiry,                
                waitTime: wait,// 存在最小排隊時間，形成阻塞式鎖
                retryTime: retry
            );

            if (redLock.IsAcquired)
            {
                await act();
            }
        }

        /// <summary>
        /// 非阻塞式
        /// <summary>
        public async Task TryLockExecuteAsync(string redisKey, Func<Task> act, TimeSpan expiry)
        {
            var redisLockKey = $"{_keyPrefix}_{redisKey}";

            await using var redLock = await _lockFactory.CreateLockAsync(
                resource: redisLockKey,
                expiryTime: expiry,
                // 非阻塞式強制等待時間為 0 ，任何訪問者使用時若正在執行，直接跳過  
                waitTime: TimeSpan.Zero,
                retryTime: TimeSpan.Zero
            );

            if (redLock.IsAcquired)
            {
                await act();
                await Task.Delay(1000);//至少1秒處理時間，避免執行太快，另一個業務也能拿到
            }
        }
    }

}
