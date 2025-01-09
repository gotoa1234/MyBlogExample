using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ZeroDowntimeDeploymentForDockerWebsiteExample.GlobalSetting;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Models;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Redis;

namespace ZeroDowntimeDeploymentForDockerWebsiteExample.SignalR
{
    public class UpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly RedisService _redisService;
        private readonly IConfiguration _configure;        
        private static string _RedisKey = "MyRadisSignalR";

        public UpdateHub(RedisService redisService)
        {
            _redisService = redisService;
            _configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                  .AddJsonFile("appsettings.json")
                                                  .Build();            
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
            //4. 將前端傳來的訊息轉為 Json
            var dataEntity = new SignalRMessagesEntity()
            {
                Message = message,
                SiteName = FieldSettings.EnviromentName,
                UserName = user,
                CreateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            var jsonData = JsonConvert.SerializeObject(dataEntity);

            //5. 寫入 Redis 保存資料
            await _redisService.GetDb(0).SortedSetAddAsync(_RedisKey, jsonData, dataEntity.CreateTime);

            //6. 這裡只要直接推播即可， Redis 的 Stack已經BackPlane 
            await Clients.All.SendAsync("ReceiveMessage", jsonData.ToString());
        }
    }
}
