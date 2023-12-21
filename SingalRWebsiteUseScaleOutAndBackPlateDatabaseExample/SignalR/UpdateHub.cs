using Microsoft.AspNetCore.SignalR;
namespace SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.SignalR
{
    public class UpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {        
        private readonly IConfiguration _configure;
        private static string _Site = string.Empty;

        public UpdateHub(IConfiguration configure)
        {
            _configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                  .AddJsonFile("appsettings.json")
                                                  .Build();
            _Site = _configure.GetValue("Site", string.Empty);
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
            await Clients.All.SendAsync("ReceiveMessage", user, $@"[當前站點：{_Site} ]" + message);
        }

    }
}
