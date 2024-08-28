using Example.Common.RabbitMQ.Consts;
using Example.Common.RabbitMQ.Model;
using Example.Common.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQLoadBalanceAspCoreWebExample.Service;
using Example.Common.FakeDataBase.Model;

namespace RabbitMQLoadBalanceAspCoreWebExample.RabbitMQ
{
    public class RabbitMQSubscriber
    {
        private readonly RabbitMQConnectionModel _selfParameters = new RabbitMQConnectionModel();
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(5);
        private RabbitMqMessageReceiver<AccountTradeOrderModel> _orderBatchSequenceReceiver = null!;

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

            var serverName = _configuration.GetSection("ServreName").Get<string>();
            _selfParameters.ServerName = serverName ?? string.Empty;
        }

        /// <summary>
        /// 建立接收器
        /// </summary>
        public void BuildReceive()
        {
            // 建立批次訂單接收器
            var initParameters = new ExchangeModel
            {
                HostName = _selfParameters.HostName,
                UserName = _selfParameters.UserName,
                Password = _selfParameters.Password,
                ExchangeType = ExchangeType.Direct,
                ExchangeName = RabbitMQConsts.MY_EXCHANGE_NAME
            };
            _orderBatchSequenceReceiver = new(initParameters, 5, OnAccountTradeOrderReceived);

            // Direct 模式要帶 Rounting Key
            _orderBatchSequenceReceiver.AddQueue(RabbitMQConsts.MY_EXCHANGE_NAME, RabbitMQConsts.MY_ROUTING_KEY);
        }

        /// <summary>
        /// 帳戶訂單接收事件
        /// </summary>    
        private void OnAccountTradeOrderReceived(AccountTradeOrderModel dto, RabbitMqMessageReceiver<AccountTradeOrderModel> receiver, ulong deliverTag, long timestamp)
        {
            Task.Run(async () =>
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var orderBatchService = scope.ServiceProvider.GetRequiredService<IAccountTradeOrder>();

                        // 模擬調用消費者事件 - 完成此單
                        dto.MechineName = _selfParameters.ServerName;
                        await orderBatchService.FinishAccountTradeOrder(dto);

                        // 完成後響應MQ - 回覆MQ完成
                        await receiver.BasicAck(deliverTag);
                    }
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex);
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
