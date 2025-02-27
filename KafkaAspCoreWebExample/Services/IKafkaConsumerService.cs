using KafkaAspCoreWebExample.Models;

namespace KafkaAspCoreWebExample.Services
{
    public interface IKafkaConsumerService
    {
        List<KafkaMessageViewModel> GetReceivedMessages();
        void AddReceivedMessage(KafkaMessageViewModel message);
    }
}
