using Example.Common.MinIO.Model;
using Microsoft.Extensions.Configuration;

namespace Example.Common.MinIO.Util
{
    public static class MinIOUtil
    {
        public static MinIOConnectionModel GetConfigureSetting(IConfiguration configuration)
        {
            var result = new MinIOConnectionModel();
            var minIOParam = configuration.GetSection("MinIO").Get<MinIOConnectionModel>();
            result.Host = minIOParam?.Host ?? string.Empty;
            result.Port = minIOParam?.Port ?? default(int);
            result.AccessKey = minIOParam?.AccessKey ?? string.Empty;
            result.SecretKey = minIOParam?.SecretKey ?? string.Empty;
            return result;
        }   

    }
}
