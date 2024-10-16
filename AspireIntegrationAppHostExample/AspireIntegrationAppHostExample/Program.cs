using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// 1. ���o�A�ȱҰʳ]�w
var serviceSettings = builder.Configuration
    .GetSection("ServiceSettings")
    .Get<Dictionary<string, bool>>();

// �Y�L�t�m�ɮ� - �i��������
if (serviceSettings == null)
    return;

#region 2. �̷ӳ]�w�ҰʪA�� - �åB�ʱ�

// ��k�B�z MinIO �A��
HandleMinIOForm(builder, serviceSettings);

// ��k�B�z Redis �A��
HandleRedisConnection(builder, serviceSettings);

#endregion 

builder.Build().Run();


// ��k�G2-1. �B�z MinIO ���
void HandleMinIOForm(IDistributedApplicationBuilder builder, Dictionary<string, bool> settings)
{
    // �v���}�Ҥ~�Ұ��[��
    if (settings.TryGetValue("MinIOFormExample", out bool startMinIO) && startMinIO)
    {       
        builder.AddProject<Projects.MinIOFormExample>("minioformexample");
    }
}

// ��k�G2-2. �B�z Redis �s��
void HandleRedisConnection(IDistributedApplicationBuilder builder, Dictionary<string, bool> settings)
{
    // �v���}�Ҥ~�Ұ��[��
    if (settings.TryGetValue("StartRedisConnectionExample", out bool startRedis) && startRedis)
    {
        // �ܷN - �Y���Ҭ� Development �ɡA�Ұʶ}�o���Ҫ� Redis �s��
        if (builder.Environment.IsDevelopment())
        {
            var redis = builder.AddConnectionString("RedisDb");
            builder.AddProject<Projects.RedisConnectionExample>("redisconnectionexample")
                   .WithReference(redis);
        }
        else// �ܷN - �Y��L���p�U�A�i�H�ϥήe���ƪ� Redis �s��
        {
            var redis = builder.AddRedis("RedisDb");
            builder.AddProject<Projects.RedisConnectionExample>("redisconnectionexample")
                   .WithReference(redis);
        }
    }
}