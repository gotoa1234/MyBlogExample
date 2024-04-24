using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace SingalRWebsiteUseScaleOutAndBackPlateRedisExample.SignalR
{
    public class UpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IConfiguration _configure;
        private readonly IDatabase _redisDatabase;
        private static string _Site = string.Empty;
        private int _siteNumber = 0;
        public UpdateHub(IDatabase redisDatabase)
        {
            _redisDatabase = redisDatabase;
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
            // 從 Redis 中獲取聊天室的歷史訊息列表
            var chatHistory = await _redisDatabase.ListRangeAsync("ChatHistory");

            // 發送聊天室的歷史訊息給新連接的用戶
            foreach (var message in chatHistory)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", message);
            }

            //await Clients.Caller.SendAsync("ReceiveMessage", "");
            //await base.OnConnectedAsync();
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
            //3. 寫入資料庫 觸發SignalR 的 Database Backplane
            await _redisDatabase.ListLeftPushAsync("ChatHistory", $"{user}: {message}");

            //4. 回報前端，後端 Server 有收到訊息了
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
}
