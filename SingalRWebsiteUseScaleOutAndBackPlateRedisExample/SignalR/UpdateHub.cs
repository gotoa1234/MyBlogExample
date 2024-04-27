using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SingalRWebsiteUseScaleOutAndBackPlateRedisExample.Models;
using SingalRWebsiteUseScaleOutAndBackPlateRedisExample.Redis;

namespace SingalRWebsiteUseScaleOutAndBackPlateRedisExample.SignalR
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

            //1. 求出自己站點編號的 2 ^ (SiteNumber-1) 值 EX: 編號1=1 / 編號2=2 / 編號3=4
            _siteNumber = (int)Math.Pow(2, (_configure.GetValue("SiteNumber", 1) - 1));
        }
        /// <summary>
        /// 建立連接時，將歷史訊息回傳
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            int startIndex = 0;
            int endIndex = -1;

            // 2. 從 Redis 中獲取聊天室的歷史訊息列表
            var chatHistory = await _redisService.GetDb(0).SortedSetRangeByRankAsync(_RedisKey, startIndex, endIndex);

            // 3. 發送聊天室的歷史訊息給新連接的用戶
            foreach (var message in chatHistory)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", message.ToString());
            }
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
            var dataEntity = new SignalRMessagesEntity() {
                Message = message,
                SiteValues = _siteNumber,
                UserName = user,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            var jsonData= JsonConvert.SerializeObject(dataEntity);

            //3. 寫入資料庫 觸發SignalR 的 Database Backplane
            await _redisService.GetDb(0).SortedSetAddAsync(_RedisKey, jsonData, dataEntity.CreateTime);

            //4. 透過publish 到Redis上
            await Clients.All.SendAsync("ReceiveMessage", jsonData.ToString());
        } 
    }
}
