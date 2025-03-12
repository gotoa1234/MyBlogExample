using KafkaAspCoreWebExample.Models;

namespace KafkaAspCoreWebExample.Services
{
    public interface IKafkaProducerService
    {
        Task<KafkaProduceResult> ProduceMessageAsync(string topic, string key, string message);
    }
}
