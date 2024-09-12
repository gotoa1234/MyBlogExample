using Example.Common.MinIO;
using Example.Common.MinIO.Model;
using Microsoft.AspNetCore.Connections;

namespace MinIOWebSiteExample.MinIO
{
    public class MinIOSubscriber
    {
        //private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(5);
        //private readonly IConfiguration _configuration;
        //private readonly IServiceProvider _serviceProvider;
        //private readonly MinIOClientInstance _client;
        //public MinIOSubscriber(IConfiguration configuration,
        //    IServiceProvider serviceProvider)
        //{
        //    _configuration = configuration;
        //    var minIOParam = _configuration.GetSection("MinIO").Get<MinIOConnectionModel>();
        //    _ConnectionItem = new MinIOConnectionModel();
        //    _ConnectionItem.Host = minIOParam?.Host ?? string.Empty;
        //    _ConnectionItem.Port = minIOParam?.Port ?? default(int);
        //    _ConnectionItem.AccessKey = minIOParam?.AccessKey ?? string.Empty;
        //    _ConnectionItem.SecretKey = minIOParam?.SecretKey ?? string.Empty;

        //    DynamicLimitSet();

        //    // 設置 SemaphoreSlim 限制
        //    void DynamicLimitSet()
        //    {
        //        // 依照本機的 CPU 核心數量動態設定最大並行值 ※設太大會導致 CPU 過載，因此應該動態設定適當的值
        //        int cpuCoreCount = Environment.ProcessorCount;
        //        _semaphoreSlim = new SemaphoreSlim(Math.Max(1, cpuCoreCount - 1));
        //    }
        //}

        ///// <summary>
        ///// 1-2. 建立MinIO Client 端
        ///// </summary>
        //public void BuildClient()
        //{
            
        //}
    }
}
