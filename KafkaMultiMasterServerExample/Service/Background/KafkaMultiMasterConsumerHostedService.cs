using Confluent.Kafka;
using KafkaAspCoreWebExample.Models;
using KafkaAspCoreWebExample.Services;
using Microsoft.Extensions.Options;

namespace KafkaMultiMasterServerExample.Service.Background
{
    public class KafkaMultiMasterConsumerHostedService : BackgroundService
    {
        private readonly ILogger<KafkaMultiMasterConsumerHostedService> _logger;
        private readonly IKafkaConsumerService _kafkaConsumerService;
        private readonly KafkaConfigOptions _kafkaConfig;
        private IConsumer<string, string> _consumer;
        private readonly IConfiguration _configuration;
        private bool _isConnected = false;

        public KafkaMultiMasterConsumerHostedService(ILogger<KafkaMultiMasterConsumerHostedService> logger,
            IKafkaConsumerService kafkaConsumerService,
            IConfiguration configuration,
            IOptions<KafkaConfigOptions> kafkaConfigOptions)
        {
            _logger = logger;
            _kafkaConsumerService = kafkaConsumerService;
            _kafkaConfig = kafkaConfigOptions.Value;
            _configuration = configuration;
        }

        /// <summary>
        /// 一、持續執行的背景工作
        /// </summary>        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // 1. 記錄配置資訊
                var urls = _configuration["urls"] ?? string.Empty;
                _logger.LogInformation($"當前主服務位置: {urls}");
                _logger.LogInformation($"嘗試連接到 Kafka 伺服器: {_kafkaConfig.BootstrapServers}");
                _logger.LogInformation($"傾聽生產者的主題: {_kafkaConfig.TopicName}");
                _logger.LogInformation($"消費者群組多個主主都是用相同的: {_kafkaConfig.ConsumerGroupId}");
                
                
                // 2. 初始化 Kafka 消費者
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _kafkaConfig.BootstrapServers,
                    GroupId = _kafkaConfig.ConsumerGroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,// 2-1. 自動提交 true:啟動 false:關閉，若想要代碼邏輯處理完成在自行設定為消費完成需要使用 false
                    EnableAutoOffsetStore = false,   // 2-2 加入這行：關閉自動偏移量存儲 ，若不關閉，即使手動沒提交也會自動往前 Offset ，導致無法重新消費此筆訊息
                    SocketTimeoutMs = 10000,
                    RetryBackoffMs = 1000,
                    SessionTimeoutMs = 6000,        // 2-3. Session Timeout 時間 6 秒
                    HeartbeatIntervalMs = 2000,     // 2-4. 心跳 2秒 (必須 < SessionTimeoutMs)
                    MaxPollIntervalMs = 10000,      // 2-5. 設定輪詢超時超過 10 秒就換機器處理此筆訊息 (必須 >= SessionTimeoutMs)                    
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
                    return;
                }

                var taskCompletionSource = new TaskCompletionSource<bool>();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        while (!stoppingToken.IsCancellationRequested)
                        {
                            try
                            {
                                // 3. 處理生產資料，取出
                                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));

                                if (consumeResult != null && !consumeResult.IsPartitionEOF)
                                {
                                    _logger.LogInformation($"【故意不手動提交，模擬此主服務異常】 當前主服務位置: {urls}");

                                    //_consumer.Commit(consumeResult); // 3-1. 完成手動提交給 Kafka 告知此筆訊息已消費
                                    _logger.LogInformation($"收到 Kafka 訊息: 鍵={consumeResult.Message.Key}, 值={consumeResult.Message.Value}");
                                }
                            }
                            catch (ConsumeException ex)
                            {
                                // 3-2. 失敗時重新丟回 Kafka 處理剛剛那筆資訊
                                _logger.LogError($"消費時出錯: {ex.Error.Reason}");                                
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

                // 4. 主線程等待取消信號
                await taskCompletionSource.Task;
            }
            catch (Exception ex)
            {
                _logger.LogError($"背景服務執行時發生未處理異常: {ex.Message}");
            }
        }

        /// <summary>
        /// 二、停止背景服務時 EX: 關閉、停用
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
