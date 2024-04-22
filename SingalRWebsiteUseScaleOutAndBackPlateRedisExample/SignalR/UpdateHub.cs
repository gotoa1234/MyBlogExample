using Microsoft.AspNetCore.SignalR;
namespace SingalRWebsiteUseScaleOutAndBackPlateRedisExample.SignalR
{
    public class UpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IConfiguration _configure;
        private static string _Site = string.Empty;
        private int _siteNumber = 0;
        //private readonly ISignalRMessagesRepository _signalRMessagesRepository;
        public UpdateHub()//ISignalRMessagesRepository signalRMessagesRepository)
        {
            //_signalRMessagesRepository = signalRMessagesRepository;
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
            //var getResult = await _signalRMessagesRepository.GetHistoryMessage(_siteNumber);
            //var temp = getResult.ToList();
            //for (int index = 0; index < getResult.Count(); index++)
            //{
            //    temp[index].Message = $@"siteNumber: [{_siteNumber}]" + temp[index].Message;
            //}

            await Clients.Caller.SendAsync("ReceiveMessage", "");
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
            //2. 接收前端傳來的聊天訊息
            var connectionId = Context.ConnectionId;
            var jwtToken = Context.GetHttpContext()?.Request.Query["access_token"];

            //3. 寫入資料庫 觸發SignalR 的 Database Backplane
            //await _signalRMessagesRepository.InsertMessage(connectionId, $@"{user}：{message}");

            //4. 回報前端，後端 Server 有收到訊息了
            await Clients.All.SendAsync("ReceiveMessage", user, $@"寫入Mysql資料庫成功：" + message);
        }

    }
}
