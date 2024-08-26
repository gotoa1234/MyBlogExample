using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Common.RabbitMQ
{
    public class RabbitMQSubscriber
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        //private RabbitMqMessageReceiver<string> _orderBatchSequenceReceiver = null!;
        /// <summary>
        /// 建構式
        /// </summary>        
        public RabbitMQSubscriber(string rabbitMqHostName, 
            string rabbitMqUserName, 
            string rabbitMqPassword,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            var rabbitParam = _configuration.GetSection("RabbitMQ").Value;
            _serviceProvider = serviceProvider;
            _hostName = rabbitMqHostName;
            _userName = rabbitMqUserName;
            _password = rabbitMqPassword;
        }

        /// <summary>
        /// 建立接收器
        /// </summary>
        public void BuildReceive()
        {
            //// 建立批次訂單接收器
            //_orderBatchSequenceReceiver = new(new BaseModel
            //{
            //    HostName = _hostName,
            //    UserName = _userName,
            //    Password = _password,
            //    ExchangeName = RabbitMQConsts.MYEXCHANGENAME,
            //    ExchangeType = ExchangeType.Direct
            //}, 5, OnBatchOrderReceived);

            //// Direct 模式要帶Key
            //_orderBatchSequenceReceiver.AddQueue(RabbitMQConsts.MYEXCHANGENAME, RabbitMQConsts.MYEXCHANGENAME);
        }

        ///// <summary>
        ///// 處理快捷下單區 - 批次處理
        ///// </summary>    
        //private void OnBatchOrderReceived(TradeOrderBatchSequenceDto dto, MqReceiver<TradeOrderBatchSequenceDto> receiver, ulong deliverTag, long timestamp)
        //{
        //    Task.Run(async () =>
        //    {
        //        try
        //        {
        //            using (var scope = _serviceProvider.CreateScope())
        //            {
        //                var orderBatchService = scope.ServiceProvider.GetService<ITradeOrderBatchSequenceService>();

        //                // 調用 ExecuteCmd 並等待其完成
        //                await orderBatchService.MultiTradeOrderBatchProcess(dto.TradeOrderBatchSequenceId);

        //                // 完成後響應MQ - 回覆完成
        //                receiver.BasicAck(deliverTag);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            // TODO: Logo
        //        }
        //    });
        //}

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
}
