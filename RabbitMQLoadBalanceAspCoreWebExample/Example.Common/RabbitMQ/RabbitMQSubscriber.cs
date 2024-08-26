using Example.Common.RabbitMQ.Consts;
using Example.Common.RabbitMQ.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Example.Common.RabbitMQ
{
    public class RabbitMQSubscriber
    {
        private readonly RabbitMQConnectionModel _selfParameters = new RabbitMQConnectionModel();
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(5);
        private RabbitMqMessageReceiver<string> _orderBatchSequenceReceiver = null!;

        /// <summary>
        /// 建構式
        /// </summary>        
        public RabbitMQSubscriber(
            IConfiguration configuration,
            IServiceProvider serviceProvider
            )
        {
            _configuration = configuration;
            var rabbitParam = _configuration.GetSection("RabbitMQ").Get<RabbitMQConnectionModel>();
            _serviceProvider = serviceProvider;
            _selfParameters.HostName = rabbitParam?.HostName ?? string.Empty;
            _selfParameters.UserName = rabbitParam?.UserName ?? string.Empty;
            _selfParameters.Password = rabbitParam?.Password ?? string.Empty;            
        }

        /// <summary>
        /// 建立接收器
        /// </summary>
        public void BuildReceive()
        {   
            // 建立批次訂單接收器
            _orderBatchSequenceReceiver = new(new ExchangeModel
            {
                HostName = _selfParameters.HostName,
                UserName = _selfParameters.UserName,
                Password = _selfParameters.Password,
                ExchangeName = RabbitMQConsts.MYEXCHANGENAME,
                ExchangeType = ExchangeType.Direct
            }, 5, OnBatchOrderReceived);

            // Direct 模式要帶Key
            _orderBatchSequenceReceiver.AddQueue(RabbitMQConsts.MYEXCHANGENAME, RabbitMQConsts.MYEXCHANGENAME);
        }

        /// <summary>
        /// 處理快捷下單區 - 批次處理
        /// </summary>    
        private void OnBatchOrderReceived(string dto, RabbitMqMessageReceiver<string> receiver, ulong deliverTag, long timestamp)
        {
            Task.Run(async () =>
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        //var orderBatchService = scope.ServiceProvider.GetService<ITradeOrderBatchSequenceService>();

                        //// 調用 ExecuteCmd 並等待其完成
                        //await orderBatchService.MultiTradeOrderBatchProcess(dto.TradeOrderBatchSequenceId);

                        // 完成後響應MQ - 回覆完成
                        //receiver.BasicAck(deliverTag);
                    }
                }
                catch (Exception e)
                {
                    // TODO: Logo
                }
            });
        }

    }

    /// <summary>
    /// 注入
    /// </summary>
    public static class MqSubscriberServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqSubscriber(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(typeof(RabbitMQSubscriber));
        }
    }

    /// <summary>
    /// 初始化配置 - 缺少就不能保持 MQ 背景運行
    /// </summary>
    public static class MqSubscriberHostExtensions
    {
        public static IHost InitMqSubscriber(this IHost host)
        {
            var mqs = host.Services.GetService<RabbitMQSubscriber>();
            mqs!.BuildReceive();
            return host;
        }
    }

}
