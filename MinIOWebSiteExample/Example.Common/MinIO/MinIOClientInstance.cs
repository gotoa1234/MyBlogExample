using Example.Common.MinIO.Factory;
using Example.Common.MinIO.Model;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Example.Common.MinIO
{
    /// <summary>
    /// https://min.io/docs/minio/linux/developers/dotnet/API.html
    /// </summary>
    public class MinIOClientInstance : MinIOConnectionModel, IDisposable
    {
        private readonly MinIOClientFactory _minioClientFactory;
        private readonly IMinioClient _minIOClientSelf = null;

        public MinIOClientInstance(MinIOConnectionModel param)
        {
            _minIOClientSelf = _minioClientFactory.CreateClient(param);            
        }

        public async Task GetBucketList(string bucketName)
        {
            try
            {
                // Get Bucket Tags for the bucket
                var args = new GetBucketTagsArgs()
                               .WithBucket(bucketName);
                var tags = await _minIOClientSelf.GetBucketTagsAsync(args);
                Console.WriteLine($"Got tags for bucket {bucketName}.");
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
