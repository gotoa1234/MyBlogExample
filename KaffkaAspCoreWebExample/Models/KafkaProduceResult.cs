namespace KaffkaAspCoreWebExample.Models
{
    public class KafkaProduceResult
    {
        public bool Success { get; set; }
        public string TopicPartitionOffset { get; set; }
        public string ErrorMessage { get; set; }
    }
}
