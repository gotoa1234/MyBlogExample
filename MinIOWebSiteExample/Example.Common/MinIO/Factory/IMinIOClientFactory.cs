using Example.Common.MinIO.Model;
using Minio;

namespace Example.Common.MinIO.Factory
{
    public interface IMinIOClientFactory
    {
        IMinioClient CreateClient(MinIOConnectionModel param);
    }
}
