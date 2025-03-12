using KafkaAspCoreWebExample.Models;
using System.Collections.Concurrent;

namespace KafkaAspCoreWebExample.Services
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly ConcurrentQueue<KafkaMessageViewModel> _receivedMessages = new ConcurrentQueue<KafkaMessageViewModel>();
        private const int MaxStoredMessages = 100;

        public List<KafkaMessageViewModel> GetReceivedMessages()
        {
            return _receivedMessages.ToList();
        }

        /// <summary>
        /// 將訊息加入隊列中
        /// </summary>
        /// <param name="message"></param>
        public void AddReceivedMessage(KafkaMessageViewModel message)
        {

            _receivedMessages.Enqueue(message);

            // 保持訊息數量不超過上限
            while (_receivedMessages.Count > MaxStoredMessages)
            {
                _receivedMessages.TryDequeue(out _);
            }
        }
    }
}
