using Confluent.Kafka;
using KaffkaAspCoreWebExample.Models;
using Microsoft.Extensions.Options;

namespace KaffkaAspCoreWebExample.Services.Background
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // 記錄配置資訊
                _logger.LogInformation($"嘗試連接到 Kafka 伺服器: {_kafkaConfig.BootstrapServers}");
                _logger.LogInformation($"主題: {_kafkaConfig.TopicName}");
                _logger.LogInformation($"消費者群組: {_kafkaConfig.ConsumerGroupId}");

                // 初始化 Kafka 消費者
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _kafkaConfig.BootstrapServers,
                    GroupId = _kafkaConfig.ConsumerGroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = true,
                    SocketTimeoutMs = 10000,
                    RetryBackoffMs = 1000
                };

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
                    // 關鍵修改：無法連接時，直接結束方法，不進入迴圈
                    return;
                }

                // 使用 TaskCompletionSource 來控制執行流程
                var taskCompletionSource = new TaskCompletionSource<bool>();

                // 在另一個線程中執行 Kafka 消費操作
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 這裡是迴圈，但在另一個線程中執行
                        while (!stoppingToken.IsCancellationRequested)
                        {
                            try
                            {
                                // 設置一個較短的超時時間
                                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));

                                if (consumeResult != null && !consumeResult.IsPartitionEOF)
                                {
                                    _logger.LogInformation($"收到 Kafka 訊息: 鍵={consumeResult.Message.Key}, 值={consumeResult.Message.Value}");

                                    // 處理訊息...
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
                                // 重要：發生錯誤後暫停一段時間，避免迴圈過快消耗資源
                                await Task.Delay(5000, stoppingToken);
                            }
                            catch (OperationCanceledException)
                            {
                                break;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"未預期錯誤: {ex.Message}");
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

                // 主線程等待取消信號
                await taskCompletionSource.Task;
            }
            catch (Exception ex)
            {
                _logger.LogError($"背景服務執行時發生未處理異常: {ex.Message}");
            }
        }

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
