using RedisProducerAndComsumerForListExample.Service;

namespace RedisProducerAndComsumerForListExample.Background
{
    /// <summary>
    /// 1. 消費者的 Host 在 Server 啟動時，就會保生產者
    /// </summary>
    public class RedisQueueConsumerService : BackgroundService
    {
        private readonly RedisQueueService _queueService;
        private readonly int _waitMilliSecond = 10000;//等待 10 秒

        public RedisQueueConsumerService(RedisQueueService queueService)
        {
            _queueService = queueService;
        }

        //2-1. 背景建立 - 保持消費事件
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // 2-2. 取出資料
                var message = _queueService.Dequeue("myQueue");

                if (message != null)
                {
                    // 2-3. 消費者處理 Queue 堆積的消息
                    Console.WriteLine($"從 myQueue 傳來的資料，進行消費 message: {message} 收到消費的時間: {DateTime.Now}");
                }
                else
                {
                    // 2-4. 如果隊列為空，稍作等待
                    await Task.Delay(_waitMilliSecond, stoppingToken);
                }
            }
        }
    }
}
