using RedisProducerAndComsumerForPubAndSubExample.Service;

namespace RedisProducerAndComsumerForPubAndSubExample.Background
{
    /// <summary>
    /// 消費者的 Host 在 Server 啟動時，就會保持讓生產者將資料傳入
    /// </summary>
    public class RedisConsumerService : BackgroundService
    {
        private readonly RedisPubSubService _pubSubService;

        public RedisConsumerService(RedisPubSubService pubSubService)
        {
            _pubSubService = pubSubService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _pubSubService.Subscribe("myChannel", message =>
            {
                // 消費者處理接收到的消息
                Console.WriteLine($"Received message: {message}");
            });

            return Task.CompletedTask;
        }
    }
}
