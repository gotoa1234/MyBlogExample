namespace RedisDistributedLockExample.RedisDistributed
{
    public interface IDistributedLock
    {
        /// <summary>
        /// 阻塞式
        /// </summary>
        public Task WaitLockExecuteAsync(
            string redisKey,
            Func<Task> act,
            TimeSpan expiryTime = default(TimeSpan),
            TimeSpan waitTime = default(TimeSpan),
            TimeSpan retryTime = default(TimeSpan)
            );

        /// <summary>
        /// 非阻塞式 
        /// <para>冪等性鎖 (expiry 到期前不開放進入)</para>        
        /// </summary>
        public Task TryLockExecuteAsync(
            string redisKey, 
            Func<Task> act, 
            TimeSpan expiryTime);
    }
}
