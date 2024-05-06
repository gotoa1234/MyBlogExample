using Microsoft.AspNetCore.SignalR;
using SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.RabbitMQ;

namespace SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.SignalR
{
    public class UpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IConfiguration _configure;
        private readonly RabbitMqService _rabbitMqService;
        private static string _Site = string.Empty;
        private int _siteNumber = 0;

        public UpdateHub(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
            _configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                  .AddJsonFile("appsettings.json")
                                                  .Build();
            _Site = _configure.GetValue("Site", string.Empty);

            //1. 求出自己站點編號的 2 ^ (SiteNumber-1) 值 EX: 編號1=1 / 編號2=2 / 編號3=4
            _siteNumber = (int)Math.Pow(2, (_configure.GetValue("SiteNumber", 1) - 1));
        }
        /// <summary>
        /// 建立連接時，將歷史訊息回傳
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var getData = FakeHistoryMessage();
            foreach (var message in getData)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", message);
            }
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 提供前端讓用戶添加到群組
        /// </summary>        
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }       

        /// <summary>
        /// 接收前端傳送訊息
        /// </summary>                
        public async Task SendMessage(string user, string message)
        {
            var dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var combineMessage = $@"[站點{_siteNumber} {dateTime}] {user}： {message}";
            _rabbitMqService.SendMessage(combineMessage);            
        }

        /// <summary>
        /// 偽造歷史資料 ※實務資料源可為 Redis / Mysql / SqlServer / MongoDB .....
        /// </summary>        
        private List<string> FakeHistoryMessage()
        {
            return new List<string>()
            {
                "[站點1 2024-4-29 05:32:35] Louis： 1",
                "[站點2 2024-4-29 05:32:35] MilkTeaGreen： 2",
                "[站點1 2024-4-29 05:33:17] Louis： 3",
                "[站點2 2024-4-29 05:33:52] MilkTeaGreen： 4",
                "[站點1 2024-4-29 05:34:12] Louis： 5",
                "[站點2 2024-4-29 05:34:17] MilkTeaGreen： 6",
            };
        }
    }
}
