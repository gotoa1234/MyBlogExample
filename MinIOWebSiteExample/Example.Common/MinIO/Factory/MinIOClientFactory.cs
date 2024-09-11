using Example.Common.MinIO.Model;
using Microsoft.Extensions.Configuration;
using Minio;
using System.Collections.Concurrent;

namespace Example.Common.MinIO.Factory
{
    public class MinIOClientFactory : IMinIOClientFactory, IDisposable
    {
        private readonly IConfiguration _configuration;
        private static readonly ConcurrentDictionary<string, IMinioClient> _minIOClientDict = new();
        private static readonly object _lockObject = new();
        private static readonly ConcurrentDictionary<string, object> _lockObjectDict = new();
        private readonly MinIOConnectionModel _ConnectionItem;

        public MinIOClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            var minIOParam = _configuration.GetSection("MinIO").Get<MinIOConnectionModel>();
            _ConnectionItem = new MinIOConnectionModel();
            _ConnectionItem.Host = minIOParam?.Host ?? string.Empty;
            _ConnectionItem.Port = minIOParam?.Port ?? default(int);
            _ConnectionItem.AccessKey = minIOParam?.AccessKey ?? string.Empty;
            _ConnectionItem.SecretKey = minIOParam?.SecretKey ?? string.Empty;            
        }

        public IMinioClient CreateClient(MinIOConnectionModel param)
        {
            var key = $"{param.Host}_{param.Port}_{param.SecretKey}_{param.AccessKey}";
            var minIOClientItem = GetMinIOClient(key);
            return minIOClientItem;

            // 取得 MinIO 客戶端
            IMinioClient GetMinIOClient(string key)
            {
                if (!_minIOClientDict.TryGetValue(key, out var minIOClientItem))
                {
                    var lockObject = GetLockObj(key);
                    lock (lockObject)
                    {
                        if (!_minIOClientDict.TryGetValue(key, out minIOClientItem))
                        {
                            minIOClientItem = new MinioClient()
                                .WithEndpoint(param.Host, param.Port)
                                .WithCredentials(param.AccessKey, param.SecretKey)
                                .Build();
                            _minIOClientDict[key] = minIOClientItem;
                        }
                    }
                }
                return minIOClientItem;
            }
        }

        /// <summary>
        /// 取得鎖物件
        /// </summary>
        private object GetLockObj(string key)
        {
            if (!_lockObjectDict.TryGetValue(key, out var obj) || obj == null)
            {
                lock (_lockObject)
                {
                    if (!_lockObjectDict.TryGetValue(key, out obj) || obj == null)
                    {
                        obj = new object();
                        _lockObjectDict[key] = obj;
                    }
                }
            }
            return obj;
        }

        public void Dispose()
        {
            foreach (var client in _minIOClientDict.Values)
            {
                client.Dispose();
            }
            _minIOClientDict.Clear();
        }
    }
}
