using Framework.Database.Enum;
using Framework.Database.Implementations.Mysql;
using Framework.Database.Interfaces;
using Quartz;
using RedisDistributedLockExample.BackGround;
using RedisDistributedLockExample.RedisDistributed;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace RedisDistributedLockExample.Extension
{
    public static class ServiceCollectionExtension
    {       
        public static IServiceCollection AddDistributedLock(this IServiceCollection services, IConfiguration configuration, string? lockKeyPrefix = null)
        {
            try
            {
                // 取得 Redis 位置
                var connectionStrings = configuration["Redis:LockEndPoints"]?.Split(',') ?? Array.Empty<string>();

                var multiplexers = connectionStrings.Select(conn =>
                {
                    var config = ConfigurationOptions.Parse(conn);
                    config.AbortOnConnectFail = false; // 重要：避免單一台掛掉導致整支程式啟動失敗
                    config.ConnectTimeout = 1000;      // 鎖的操作要快，逾時設定不宜過長
                    return new RedLockMultiplexer(ConnectionMultiplexer.Connect(config));
                }).ToList();

                services.AddSingleton<IDistributedLockFactory, RedLockFactory>(_ => RedLockFactory.Create(multiplexers));
                services.AddSingleton<IDistributedLock>(item =>
                {
                    var logger = item.GetRequiredService<ILogger<IDistributedLock>>();
                    var redisInstance = new DistributedLock(
                       lockFactory: item.GetRequiredService<IDistributedLockFactory>(),
                       lockKeyPrefix: string.IsNullOrEmpty(lockKeyPrefix)
                                      ? nameof(DistributedLock)
                                      : lockKeyPrefix);

                    redisInstance.LockWaitTimeoutEvent += (message) =>
                    {
                        logger.LogError($"RedLock Error:{message}");
                    };

                    return redisInstance;
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"RedLock DI Error:{ex.Message}");
            }

            return services;
        }
        
        /// <summary>
        /// Quartz.NET 排程 DI        
        /// </summary>
        public static IServiceCollection AddQuartzNETJob(this IServiceCollection services, DateTime startAt)
        {
            try
            {
                services.AddQuartz(q =>
                {
                    // 1. 建立 Job 
                    var selfKey = new JobKey(nameof(QuartzJobForDistributed));
                    q.AddJob<QuartzJobForDistributed>(opts => opts.WithIdentity(selfKey));

                    // 2. 啟動時間，目的是模擬高併發精準到微秒
                    var startedDateTime = new DateTimeOffset(startAt,
                                                TimeSpan.FromHours(8));//Tw 時間

                    // 3-1. 建立 Trigger 1
                    q.AddTrigger(opts => opts
                        .ForJob(selfKey)
                        .WithIdentity("WorkerFlowJob-trigger1")
                        .UsingJobData("SourceTag", "Server_A") // 標記 - 假設架設在機器A
                        .StartAt(startedDateTime)
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(10)// 模擬同時每 10s 併發碰撞
                            .RepeatForever()));

                    // 3-2. 建立 Trigger 2
                    q.AddTrigger(opts => opts
                        .ForJob(selfKey)
                        .WithIdentity("WorkerFlowJob-trigger2")
                        .UsingJobData("SourceTag", "Server_B") // 標記 - 假設架設在機器B
                        .StartAt(startedDateTime)
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(10)// 模擬同時每 10s 併發碰撞
                            .RepeatForever()));
                });

                // 4. 新增 Quartz 託管服務，這會負責啟動/停止排程器
                services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Quartz.NET DI Error:{ex.Message}");
            }

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services,
             Dictionary<MysqlDbConnectionEnum, string> dbSettings)
        {
            // 驗證配置是否有值
            if (dbSettings == null || dbSettings.Count == 0)
            {
                throw new ArgumentException("資料庫配置字典不可為空", nameof(dbSettings));
            }

            // 註冊 IDbConnectionFactory 介面及其對應的實作
            services.AddSingleton<IDbConnectionFactory>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DbConnectionFactory>>();
                return new DbConnectionFactory(dbSettings, logger);
            });

            // 註冊 UnitOfWork Factory
            services.AddSingleton<IUnitOfWorkFactory, MySqlUnitOfWorkFactory>();

            return services;
        }

    }
}
