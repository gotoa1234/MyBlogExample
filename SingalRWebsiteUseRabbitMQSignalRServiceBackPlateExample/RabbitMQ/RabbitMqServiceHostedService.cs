namespace SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.RabbitMQ
{
    /// <summary>
    /// 結束系統時釋放 RabbitMQ 資源
    /// </summary>
    public class RabbitMqServiceHostedService : IHostedService
    {
        private readonly RabbitMqService _rabbitMqService;

        public RabbitMqServiceHostedService(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMqService.Dispose();
            return Task.CompletedTask;
        }
    }
}
