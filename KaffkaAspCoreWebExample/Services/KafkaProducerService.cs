using Confluent.Kafka;
using KaffkaAspCoreWebExample.Models;
using Microsoft.Extensions.Options;

namespace KaffkaAspCoreWebExample.Services
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaConfigOptions _kafkaConfig;

        public KafkaProducerService(IOptions<KafkaConfigOptions> kafkaConfigOptions)
        {
            _kafkaConfig = kafkaConfigOptions.Value;

            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers,
                Acks = Acks.All
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task<KafkaProduceResult> ProduceMessageAsync(string topic, string key, string message)
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
                    Success = true,
                    TopicPartitionOffset = deliveryResult.TopicPartitionOffset.ToString()
                };
            }
            catch (ProduceException<string, string> ex)
            {
                return new KafkaProduceResult
                {
                    Success = false,
                    ErrorMessage = ex.Error.Reason
                };
            }
        }
    }
}
