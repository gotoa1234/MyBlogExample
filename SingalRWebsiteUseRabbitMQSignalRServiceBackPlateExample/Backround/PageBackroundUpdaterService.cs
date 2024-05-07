using Microsoft.AspNetCore.SignalR;
using SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.RabbitMQ;
using SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.SignalR;

namespace SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.Backround
{

    public class PageBackroundUpdaterService : BackgroundService
    {
        private readonly IHubContext<UpdateHub> _hubContext;
        private readonly IConfiguration _configure;        
        private readonly RabbitMqService _rabbitMqService;
        // 1. 配置變數，版本號、間隔時間
        private int _siteNumber = 0;

        public PageBackroundUpdaterService(IHubContext<UpdateHub> hubContext,
            RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;            
            _hubContext = hubContext;
            _configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                      .AddJsonFile("appsettings.json")
                                      .Build();

            //2. 求出自己站點編號的 2 ^ (SiteNumber-1) 值 EX: 編號1=1 / 編號2=2 / 編號3=4
            _siteNumber = (int)Math.Pow(2, (_configure.GetValue("SiteNumber", 1) - 1));

            //3-1. 設定 RabbitMQ 消費者(Consumer)的工作
            _rabbitMqService.StartReceiving(publishMessage => {
                //3-2. 從 RabbitMQ 收到訊息後，推播給自己 Server 下的所有用戶
                Task.Run(() => _hubContext.Clients.Group("groupName").SendAsync("ReceiveMessage", publishMessage));
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        }
    }
}
