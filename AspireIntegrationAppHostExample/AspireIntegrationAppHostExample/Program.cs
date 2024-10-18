using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// 1. 取得服務啟動設定
var serviceSettings = builder.Configuration
    .GetSection("ServiceSettings")
    .Get<Dictionary<string, bool>>();

// 若無配置檔案 - 可直接關閉
if (serviceSettings == null)
    return;

#region 2. 依照設定啟動服務 - 並且監控

// 方法處理 MinIO 服務
HandleMinIOForm(builder, serviceSettings);

// 方法處理 Redis 服務
HandleRedisConnection(builder, serviceSettings);

#endregion 

builder.AddProject<Projects.MysqlConnectionExample>("mysqlconnectionexample");

builder.Build().Run();


// 方法：2-1. 處理 MinIO 表單
void HandleMinIOForm(IDistributedApplicationBuilder builder, Dictionary<string, bool> settings)
{
    // 權限開啟才啟動觀察
    if (settings.TryGetValue("MinIOFormExample", out bool startMinIO) && startMinIO)
    {       
        builder.AddProject<Projects.MinIOFormExample>("minioformexample");
    }
}

// 方法：2-2. 處理 Redis 連接
void HandleRedisConnection(IDistributedApplicationBuilder builder, Dictionary<string, bool> settings)
{
    // 權限開啟才啟動觀察
    if (settings.TryGetValue("StartRedisConnectionExample", out bool startRedis) && startRedis)
    {
        // 示意 - 若環境為 Development 時，啟動開發環境的 Redis 連接
        if (!builder.Environment.IsDevelopment())
        {
            var redis = builder.AddConnectionString("RedisDb");
            builder.AddProject<Projects.RedisConnectionExample>("redisconnectionexample")
                   .WithReference(redis);
        }
        else// 示意 - 若其他情況下，可以使用容器化的 Redis 連接
        {
            var redis = builder.AddRedis("RedisDb");
            builder.AddProject<Projects.RedisConnectionExample>("redisconnectionexample")
                   .WithReference(redis);
        }
    }
}


// 方法：2-3. 處理 Mysql 連接
void HandleMysqlConnection(IDistributedApplicationBuilder builder, Dictionary<string, bool> settings)
{
    // 權限開啟才啟動觀察
    if (settings.TryGetValue("StartMysqlConnectionExample", out bool startRedis) && startRedis)
    {
        // 示意 - 若環境為 Development 時，啟動開發環境的 Mysql 連接
        if (!builder.Environment.IsDevelopment())
        {
            var mysql = builder.AddConnectionString("MySqlConnection");
            builder.AddProject<Projects.MysqlConnectionExample>("mysqlconnectionexample")
                   .WithReference(mysql);
        }
        else// 示意 - 若其他情況下，可以使用容器化的 Mysql 連接
        {
            var mysql = builder.AddMySql("MySqlConnection");
            builder.AddProject<Projects.MysqlConnectionExample>("mysqlconnectionexample")
                   .WithReference(mysql);
        }
    }
}