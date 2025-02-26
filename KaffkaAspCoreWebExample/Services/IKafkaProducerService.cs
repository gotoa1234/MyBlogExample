using KaffkaAspCoreWebExample.Models;

namespace KaffkaAspCoreWebExample.Services
{
    public interface IKafkaProducerService
    {
        Task<KafkaProduceResult> ProduceMessageAsync(string topic, string key, string message);
    }
}
