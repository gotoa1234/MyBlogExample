using Example.Common.MinIO.Factory;
using Example.Common.MinIO.Model;
using Example.Common.MinIO.Util;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Example.Common.MinIO
{
    /// <summary>
    /// https://min.io/docs/minio/linux/developers/dotnet/API.html
    /// </summary>
    public class MinIOClientInstance : MinIOConnectionModel, IDisposable
    {
        private readonly IMinIOClientFactory _minioClientFactory;
        private readonly IMinioClient _minIOClientSelf = null;

        public MinIOClientInstance(MinIOConnectionModel param,
            IConfiguration configuration,
            IMinIOClientFactory minioClientFactory)
        {
            _minioClientFactory = minioClientFactory;
            _minIOClientSelf = _minioClientFactory.CreateClient(MinIOUtil.GetConfigureSetting(configuration));
        }

        public async Task GetBucketList(string bucketName)
        {
            try
            {
                // 使用 ListObjectsAsync 方法來列出所有的物件
                var listArgs = new ListObjectsArgs()
                                   .WithBucket(bucketName)
                                   .WithRecursive(true); // 設為 true 可以列出所有物件，包括子目錄

                var objects = _minIOClientSelf.ListObjectsEnumAsync(listArgs);

                // 遍歷 bucket 中的所有物件
                await foreach (Minio.DataModel.Item obj in objects)
                {
                    var temp = obj.Key;// FileName
                }

            }
            catch (MinioException e)
            {
                Console.WriteLine("Error occurred: " + e);
            }       
        }

        #region 解構式 - 釋放資源
        ~MinIOClientInstance()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
