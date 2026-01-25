namespace RedisNetLockExample.Redis
{
    public interface IRedixLockService
    {
        Task<(bool Acquired, string LockValue)> AcquireAsync(string lockKey, TimeSpan ttl);

        Task<bool> ReleaseAsync(string lockKey, string lockValue);
    }
}
