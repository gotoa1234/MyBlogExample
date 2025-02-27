namespace KafkaAspCoreWebExample.Models
{
    /// <summary>
    /// 生產者返回資訊
    /// </summary>
    public class KafkaProduceResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 主題 + 分區 + 偏移量
        /// </summary>
        public string TopicPartitionOffset { get; set; } = string.Empty;

        /// <summary>
        /// 錯誤訊息 (如果發生，才有值)
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
