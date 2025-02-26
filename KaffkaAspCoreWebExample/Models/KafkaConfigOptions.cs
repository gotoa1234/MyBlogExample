namespace KaffkaAspCoreWebExample.Models
{
    public class KafkaConfigOptions
    {
        public string BootstrapServers { get; set; }
        public string TopicName { get; set; }
        public string ConsumerGroupId { get; set; }
    }
}
