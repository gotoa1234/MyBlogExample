using KaffkaAspCoreWebExample.Models;

namespace KaffkaAspCoreWebExample.Services
{
    public interface IKafkaConsumerService
    {
        List<KafkaMessageViewModel> GetReceivedMessages();
        void AddReceivedMessage(KafkaMessageViewModel message);
    }
}
