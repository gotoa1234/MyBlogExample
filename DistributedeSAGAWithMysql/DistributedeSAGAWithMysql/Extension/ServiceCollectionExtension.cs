using Framework.Database.Enum;
using Framework.Database.Implementations.Mysql;
using Framework.Database.Interfaces;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using DistributedeSAGAWithMysql.BackGround;

namespace DistributedeSAGAWithMysql.Extension
{
    public static class ServiceCollectionExtension
    {
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
                    var selfKey = new JobKey(nameof(QuartzJobForCompensatory));
                    q.AddJob<QuartzJobForCompensatory>(opts => opts.WithIdentity(selfKey));

                    // 2. 啟動時間
                    var startedDateTime = new DateTimeOffset(startAt,
                                                TimeSpan.FromHours(8));//Tw 時間

                    // 3-1. 建立 Trigger 1
                    q.AddTrigger(opts => opts
                        .ForJob(selfKey)
                        .WithIdentity("CompensatoryServices")
                        .UsingJobData("Compensatory", "ServerA") // 標記 - 假設架設在機器A
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


        public static IServiceCollection AddDatabase(this IServiceCollection services, Dictionary<MysqlDbConnectionEnum, string> dbSettings)
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
