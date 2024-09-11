namespace Example.Common.MinIO.Model
{
    /// <summary>
    /// MinIO 連線模型
    /// </summary>
    public class MinIOConnectionModel
    {
        /// <summary>
        /// 主機位置
        /// </summary>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Port 號
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 存取金鑰
        /// </summary>
        public string AccessKey { get; set; } = string.Empty;

        /// <summary>
        /// 密鑰
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;
    }
}
