using Example.Common.FakeDataBase.Model;
using Example.Common.MinIO.Factory;
using Example.Common.MinIO.Model;
using Example.Common.MinIO.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Example.Common.MinIO
{
    /// <summary>
    /// Error: https://min.io/docs/minio/linux/developers/dotnet/API.html
    /// Right: https://github.com/minio/minio-dotnet/blob/master/Minio.Examples/Cases/ListObjects.cs
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

        /// <summary>
        /// 建立 Bucket
        /// </summary>                
        public async Task CreateBucket(string bucketName)
        {
            try
            {
                // 取得是否存在               
                var isExist = await IsExistBucket(bucketName);

                // 不存在 - 才創建
                if (!isExist)
                {
                    await _minIOClientSelf.MakeBucketAsync(
                        new MakeBucketArgs()
                            .WithBucket(bucketName)
                    ).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[CreateBucket]  Exception: {e}");
            }
        }

        /// <summary>
        /// 取得 Bucket 內的資料
        /// </summary>        
        public async Task<FileModel> GetBucketList(string bucketName)
        {
            var result = new FileModel();
            try
            {
                // 取得是否存在               
                var isExist = await IsExistBucket(bucketName);
                // 不存在捨棄
                if (!isExist)
                    return result;

                // 使用 ListObjectsAsync 方法來列出所有的物件
                var listArgs = new ListObjectsArgs()
                                   .WithBucket(bucketName)
                                   .WithRecursive(true); // 設為 true 可以列出所有物件，包括子目錄

                var objects = _minIOClientSelf.ListObjectsEnumAsync(listArgs);

                // 遍歷 bucket 中的所有物件
                result.BucketName = bucketName;
                await foreach (Minio.DataModel.Item obj in objects)
                {
                    result.Files.Add(new FileItem()
                    {
                        FileName = obj.Key,
                        FileExtension = Path.GetExtension(obj.Key),
                        FileSize = obj.Size,
                        LastUpdateTime = obj.LastModifiedDateTime
                    });
                }
            }
            catch (MinioException e)
            {
                Console.WriteLine($"[GetBucketList]  Exception: {e}");
            }

            return result;
        }

        /// <summary>
        /// 下載檔案 - 記憶體資料
        /// </summary>        
        public async Task<MemoryStream> GetObjectAsync(string fileName, string bucketName)
        {
            var memoryStream = new MemoryStream();
            try
            {                
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithCallbackStream((stream) =>
                    {                        
                        stream.CopyTo(memoryStream);
                        memoryStream.Position = 0;
                    });
                
                await _minIOClientSelf.GetObjectAsync(getObjectArgs).ConfigureAwait(false);

                return memoryStream; // 成功返回文件流
            }
            catch (Exception e)
            {
                Console.WriteLine($"[GetObjectAsync]  Exception: {e}");
            }
            return memoryStream;
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>                
        public async Task UploadFile(IFormFile file, string bucketName) 
        {
            try
            {
                // 取得是否存在               
                var isExist = await IsExistBucket(bucketName);
                // 不存在捨棄
                if (!isExist)
                    return;

                // 取得上傳檔案的流
                using (var fileStream = file.OpenReadStream())
                {
                    var objectName = file.FileName; // 使用檔案名稱作為物件名稱

                    // 設置 PutObjectArgs
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName)
                        .WithStreamData(fileStream)
                        .WithObjectSize(fileStream.Length)
                        .WithContentType(file.ContentType); // 使用上傳檔案的 Content-Type
                    await _minIOClientSelf.PutObjectAsync(putObjectArgs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[UploadFile]  Exception: {e}");
            }
        }

        /// <summary>
        /// 完整刪除整個 Bucket 
        /// </summary>                
        public async Task DeleteBucket(string bucketName)
        {
            try
            {
                // 取得是否存在               
                var isExist = await IsExistBucket(bucketName);

                // 不存在 - 退出
                if (!isExist)
                    return;

                // 列出並刪除所有物件                                
                var listArgs = new ListObjectsArgs()
                                   .WithBucket(bucketName)
                                   .WithRecursive(true);

                var objects = _minIOClientSelf.ListObjectsEnumAsync(listArgs);
                await foreach (Minio.DataModel.Item obj in objects)
                {
                    await _minIOClientSelf.RemoveObjectAsync(new RemoveObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(obj.Key));
                }

                // 物件內的資料都清除後才能刪除 Bucket
                await _minIOClientSelf.RemoveBucketAsync(
                      new RemoveBucketArgs().WithBucket(bucketName)
                      ).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                Console.WriteLine($"[DeleteBucket]  Exception: {e}");
            }
        }

        /// <summary>
        /// 刪除單一檔案
        /// </summary>                
        public async Task DeleteFile(string fileName, string bucketName)
        {
            try
            {
                // 取得是否存在               
                var isExist = await IsExistBucket(bucketName);

                // 不存在 - 退出
                if (!isExist)
                    return;

                // 刪除指定物件                                
                await _minIOClientSelf.RemoveObjectAsync(new RemoveObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(fileName));
            }
            catch (Exception e)
            {
                Console.WriteLine($"[DeleteFile]  Exception: {e}");
            }

        }

        /// <summary>
        /// 是否存在指定 Bucket
        /// </summary>
        public async Task<bool> IsExistBucket(string bucketName)
        {
            try
            {
                // 取得是否存在
                var getArgs = new BucketExistsArgs().WithBucket(bucketName);
                var isExist = await _minIOClientSelf.BucketExistsAsync(getArgs).ConfigureAwait(false);

                return isExist;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[IsExistBucket]  Exception: {e}");
                return false;
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
