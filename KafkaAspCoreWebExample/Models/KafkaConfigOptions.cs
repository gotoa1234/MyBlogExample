namespace KafkaAspCoreWebExample.Models
{
    public class KafkaConfigOptions
    {
        /// <summary>
        /// 伺服器位置 ※Kafka 集群的入口點 ; Broker 格式 "192.168.51.100:9091 ,192.168.51.100:9092"
        /// </summary>
        public string BootstrapServers { get; set; } = string.Empty;

        /// <summary>
        /// 主題名稱
        /// </summary>
        public string TopicName { get; set; } = string.Empty;

        /// <summary>
        /// 消費者群組對象
        /// </summary>
        public string ConsumerGroupId { get; set; } = string.Empty;
    }
}
