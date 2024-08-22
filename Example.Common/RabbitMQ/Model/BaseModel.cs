namespace Example.Common.RabbitMQ.Model
{
    public class BaseModel
    {
        /// <summary>
        /// 主機位置
        /// </summary>
        public string HostName { get; set; } = string.Empty;

        /// <summary>
        /// 帳戶
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;        
    }
}
