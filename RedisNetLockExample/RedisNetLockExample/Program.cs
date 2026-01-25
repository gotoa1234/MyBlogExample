using RedisNetLockExample.Redis;
using StackExchange.Redis;

await RedisOfficialLock();
Console.WriteLine("Example Finish");

async Task RedisOfficialLock()
{
    var redis = ConnectionMultiplexer.Connect("127.0.0.1:32770");
    var redisLock = new RedisLockService(redis);

    var (acquired, lockValue) = await redisLock.AcquireAsync(
        "order:lock:123",
        TimeSpan.FromSeconds(100)
    );

    if (!acquired)
    {
        Console.WriteLine("未取得鎖");
        return;
    }

    try
    { 
        // 模擬在工作
        Console.WriteLine("取得鎖，執行臨界區");
        await Task.Delay(3000);
    }
    finally
    {
       await redisLock.ReleaseAsync("order:lock:123", lockValue);
    }
}
