using Microsoft.AspNetCore.SignalR;

namespace NetCoreSignalRWebSiteExample.SignalR
{
    public class UpdateHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task SendUpdate(string message)
        {
            await Clients.All.SendAsync("SendUpdate", message);
        }
    }
}
