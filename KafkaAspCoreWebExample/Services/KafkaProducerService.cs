using Confluent.Kafka;
using KafkaAspCoreWebExample.Models;
using Microsoft.Extensions.Options;

namespace KafkaAspCoreWebExample.Services
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaConfigOptions _kafkaConfig;

        /// <summary>
        /// 1. 建構式，準備連線對象
        /// </summary>        
        public KafkaProducerService(IOptions<KafkaConfigOptions> kafkaConfigOptions)
        {
            _kafkaConfig = kafkaConfigOptions.Value;

            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers,
                Acks = Acks.All, // Acks 參數定義了生產者發送訊息後，需要多少個 Broker 確認訊息已被接收，才能視為成功發送。
                MessageTimeoutMs = 5000 // 5 秒 timeout
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        /// <summary>
        /// 2. 生產者 : 發送到 Kafka  Broker 上
        /// </summary>        
        public async Task<KafkaProduceResult> ProduceMessageAsync(
            string topic, string key, string message)
        {
            try
            {
                var deliveryResult = await _producer.ProduceAsync(
                    topic,
                    new Message<string, string>
                    {
                        Key = string.IsNullOrEmpty(key) ? Guid.NewGuid().ToString() : key,
                        Value = message
                    });

                return new KafkaProduceResult
                {
                    IsSuccess = true,
                    TopicPartitionOffset = deliveryResult.TopicPartitionOffset.ToString()
                };
            }
            catch (ProduceException<string, string> ex)
            {
                return new KafkaProduceResult
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Error.Reason
                };
            }
        }
    }
}
