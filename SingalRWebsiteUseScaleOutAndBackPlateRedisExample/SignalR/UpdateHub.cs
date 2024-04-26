using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SingalRWebsiteUseScaleOutAndBackPlateRedisExample.Models;
using StackExchange.Redis;

namespace SingalRWebsiteUseScaleOutAndBackPlateRedisExample.SignalR
{
    public class UpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IConfiguration _configure;
        private readonly IDatabase _redisDatabase;
        private static string _Site = string.Empty;
        private static string _RedisKey = "MyRadisSignalR";
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

            SubScribeRedisEvent();
        }

        /// <summary>
        /// SignalR Hub 訂閱 Redis ，讓每個訂閱 Redis 的 SignalR 可以同時推播
        /// </summary>
        private void SubScribeRedisEvent()
        {            
            var redisSubscriber = _redisDatabase.Multiplexer.GetSubscriber();
            redisSubscriber.Subscribe("topic.test", async (channel, value) =>
            {
                // 當收到訂閱的事件通知時，發送更新訊息給所有的客戶端
                //var updatedValue = await _redisDatabase.StringGetAsync(_RedisKey);
                await Clients.All.SendAsync("ReceiveMessage", updatedValue);
            });
        }

        /// <summary>
        /// 建立連接時，將歷史訊息回傳
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            int startIndex = 0;
            int endIndex = -1;

            // 從 Redis 中獲取聊天室的歷史訊息列表
            var chatHistory = await _redisDatabase.SortedSetRangeByRankAsync(_RedisKey, startIndex, endIndex);

            // 發送聊天室的歷史訊息給新連接的用戶
            foreach (var message in chatHistory)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", message.ToString());
            }
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
            await _redisDatabase.SortedSetAddAsync(_RedisKey, jsonData, dataEntity.CreateTime);
            
            _redisDatabase.Publish("topic.test", "123");            
            ////4. 回報前端，後端 Server 有收到訊息了
            //await Clients.All.SendAsync("ReceiveMessage", jsonData);
        }

    }
}
