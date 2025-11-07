using ReappearSignalRATaskWasCanceldExample.Model;
using ReappearSignalRATaskWasCanceldExample.Service;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ReappearSignalRATaskWasCanceldExample.SignalR
{
    public class UpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly RedisService _redisService;
        private readonly IConfiguration _configure;
        private static string _Site = string.Empty;
        private static string _RedisKey = "MyRadisSignalR";
        private int _siteNumber = 0;

        public UpdateHub(RedisService redisService)
        {
            _redisService = redisService;
            _configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                  .AddJsonFile("appsettings.json")
                                                  .Build();
            _Site = _configure.GetValue("Site", string.Empty);            
            _siteNumber = (int)Math.Pow(2, (_configure.GetValue("SiteNumber", 1) - 1));
        }
        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        //事件名稱SendUpdate 行為:回傳message
        public async Task SendUpdate(string message)
        {
            await Clients.All.SendAsync("SendUpdate", message);
        }

        /// <summary>
        /// 接收前端傳送訊息
        /// </summary>                
        public async Task SendMessage(string user, string message)
        {
            // 1. 重現步驟 - 使用 HubContext 內建的 CT
            var ct = Context.ConnectionAborted;
            await Task.Delay(10000, ct); // 2. 重現步驟 - 帶入 ct，客戶端中斷就會丟 TaskCanceledException

            var dataEntity = new SignalRMessagesEntity()
            {
                Message = message,
                SiteValues = _siteNumber,
                UserName = user,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            var jsonData = JsonConvert.SerializeObject(dataEntity);
            await _redisService.GetDb(0).SortedSetAddAsync(_RedisKey, jsonData, dataEntity.CreateTime);            
            await Clients.All.SendAsync("ReceiveMessage", jsonData.ToString(), ct);
        }
    }
}
