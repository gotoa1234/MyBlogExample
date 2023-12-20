using Microsoft.AspNetCore.SignalR;

namespace NetCoreSignalRWebSiteExample.SignalR
{
    public class UpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {
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
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
}
