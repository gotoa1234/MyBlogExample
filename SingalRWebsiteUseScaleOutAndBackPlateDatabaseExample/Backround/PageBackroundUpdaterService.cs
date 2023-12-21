using Microsoft.AspNetCore.SignalR;
using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.SignalR;

namespace SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Backround
{
    public class PageBackroundUpdaterService : BackgroundService
    {
        private readonly IHubContext<UpdateHub> _hubContext;
        // 1. 配置變數，版本號、間隔時間
        private int _versionNumber = 0;
        private readonly int _second = 30 * 1000;//30秒
        public PageBackroundUpdaterService(IHubContext<UpdateHub> hubContext
            )
        {
            _hubContext = hubContext;
            _versionNumber = 0;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // 2. 編寫返回資訊
                var data = $@"回報時間：{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") } 版本號：{_versionNumber}";

                // 3. 推播訊息給客戶端
                await _hubContext.Clients.All.SendAsync("SendUpdate", data);

                // 4. 輪詢時間
                await Task.Delay(_second, stoppingToken);
                _versionNumber++;
            }
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
