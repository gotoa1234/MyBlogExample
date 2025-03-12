using RedisProducerAndComsumerForPubAndSubExample.Service;

namespace RedisProducerAndComsumerForPubAndSubExample.Background
{
    /// <summary>
    /// 1. 消費者的 Host 在 Server 啟動時，就會保持讓生產者將資料傳入
    /// </summary>
    public class RedisConsumerService : BackgroundService
    {
        private readonly RedisPubSubService _pubSubService;

        public RedisConsumerService(RedisPubSubService pubSubService)
        {
            _pubSubService = pubSubService;
        }

        //2-1. 背景建立 - 保持訂閱事件
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _pubSubService.Subscribe("myChannel", message =>
            {
                // 2-2. 消費者處理接收到的消息
                Console.WriteLine($"收到資料囉，從 myChannel 來，得到的資訊: {message}");
            });

            return Task.CompletedTask;
        }
    }
}
