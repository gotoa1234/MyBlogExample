using Confluent.Kafka;
using KafkaAspCoreWebExample.Models;
using Microsoft.Extensions.Options;

namespace KafkaAspCoreWebExample.Services.Background
{
    public class KafkaConsumerHostedService : BackgroundService
    {
        private readonly ILogger<KafkaConsumerHostedService> _logger;
        private readonly IKafkaConsumerService _kafkaConsumerService;
        private readonly KafkaConfigOptions _kafkaConfig;
        private IConsumer<string, string> _consumer;
        private bool _isConnected = false;

        public KafkaConsumerHostedService(
            ILogger<KafkaConsumerHostedService> logger,
            IKafkaConsumerService kafkaConsumerService,
            IOptions<KafkaConfigOptions> kafkaConfigOptions)
        {
            _logger = logger;
            _kafkaConsumerService = kafkaConsumerService;
            _kafkaConfig = kafkaConfigOptions.Value;
        }

        /// <summary>
        /// 一、持續執行的背景工作
        /// </summary>        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // 1. 記錄配置資訊
                _logger.LogInformation($"嘗試連接到 Kafka 伺服器: {_kafkaConfig.BootstrapServers}");
                _logger.LogInformation($"主題: {_kafkaConfig.TopicName}");
                _logger.LogInformation($"消費者群組: {_kafkaConfig.ConsumerGroupId}");

                // 2. 初始化 Kafka 消費者
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _kafkaConfig.BootstrapServers,
                    GroupId = _kafkaConfig.ConsumerGroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = true,
                    SocketTimeoutMs = 10000,
                    RetryBackoffMs = 1000
                };

                // 2-2. 無法連接時，直接結束方法，不進入迴圈 (這篇範例一定要 Kafka Server 正常)
                try
                {
                    _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
                    _consumer.Subscribe(_kafkaConfig.TopicName);
                    _logger.LogInformation($"成功連接並訂閱主題: {_kafkaConfig.TopicName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"連接到 Kafka 失敗: {ex.Message}");
                    _logger.LogWarning("應用程式將繼續運行，但 Kafka 消費功能將不可用");
                    return;
                }

                // 3-1. 開啟 TaskCompletionSource 來控制執行流程
                var taskCompletionSource = new TaskCompletionSource<bool>();

                // 3-2. 啟用線程執行 Kafka 消費操作 (避免阻塞主程式)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 3-3. 不斷檢查是否有觸發生產者資料
                        while (!stoppingToken.IsCancellationRequested)
                        {
                            try
                            {
                                // 4-1. 處理生產資料，取出
                                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));

                                if (consumeResult != null && !consumeResult.IsPartitionEOF)
                                {
                                    _logger.LogInformation($"收到 Kafka 訊息: 鍵={consumeResult.Message.Key}, 值={consumeResult.Message.Value}");

                                    // 4-2. 處理訊息...
                                    _kafkaConsumerService.AddReceivedMessage(new KafkaMessageViewModel()
                                    {
                                        Key = consumeResult.Message.Key,
                                        Message = consumeResult.Message.Value,
                                        Topic = consumeResult.Topic,
                                        Offset = consumeResult.TopicPartitionOffset.Offset
                                    });
                                }
                            }
                            catch (ConsumeException ex)
                            {
                                _logger.LogError($"消費時出錯: {ex.Error.Reason}");

                                // 4-3. While 內發生錯誤後暫停一段時間，避免迴圈過快消耗資源
                                await Task.Delay(5000, stoppingToken);
                            }
                            catch (OperationCanceledException)
                            {
                                // 4-4. 確定中止強制跳出
                                break;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"未預期錯誤: {ex.Message}");
                                // 4-5. While 內發生錯誤後暫停一段時間，避免迴圈過快消耗資源
                                await Task.Delay(5000, stoppingToken);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"消費線程發生未處理錯誤: {ex.Message}");
                    }
                    finally
                    {
                        taskCompletionSource.TrySetResult(true);
                    }
                }, stoppingToken);

                // 5. 主線程等待取消信號
                await taskCompletionSource.Task;
            }
            catch (Exception ex)
            {
                _logger.LogError($"背景服務執行時發生未處理異常: {ex.Message}");
            }
        }

        /// <summary>
        /// 三、停止背景服務時 EX: 關閉、停用
        /// </summary>        
        public override Task StopAsync(CancellationToken stoppingToken)
        {
            if (_isConnected)
            {
                _logger.LogInformation("正在關閉 Kafka 消費者");
                _consumer?.Close();
                _consumer?.Dispose();
            }
            return base.StopAsync(stoppingToken);
        }
    }
}
