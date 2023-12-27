using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Repository;
using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.SignalR;

namespace SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Backround
{
    public class PageBackroundUpdaterService : BackgroundService
    {
        private readonly IHubContext<UpdateHub> _hubContext;        
        private readonly IConfiguration _configure;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;
        // 1. 配置變數，版本號、間隔時間
        private int _siteNumber = 0;
        private readonly int _second = 2;//2秒        
        public PageBackroundUpdaterService(IHubContext<UpdateHub> hubContext,
            IServiceProvider serviceProvider, IMemoryCache memoryCache)                
        {
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
            _configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                      .AddJsonFile("appsettings.json")
                                      .Build();

            //2. 求出自己站點編號的 2 ^ (SiteNumber-1) 值 EX: 編號1=1 / 編號2=2 / 編號3=4
            _siteNumber = (int)Math.Pow(2, (_configure.GetValue("SiteNumber", 1) - 1));  
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    // 3-1. Singalton 的方式避免不斷 create scope 浪費資源
                    if (!_memoryCache.TryGetValue("SignalRMessagesRepository", out ISignalRMessagesRepository _signalRMessages))
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            _signalRMessages = scope.ServiceProvider.GetRequiredService<ISignalRMessagesRepository>();
                            // 3-2. 將服務存入快取，可以自行調整快取的過期時間
                            _memoryCache.Set("SignalRMessagesRepository", _signalRMessages, TimeSpan.FromSeconds(60));
                        }
                    }

                    // 4. 讀取資料庫是否有未處裡的資料
                    var data = await _signalRMessages.GetMessage(_siteNumber);
                    if (data.Any())
                    {
                        foreach (var message in data)
                        {
                            // 5-1. 組成回傳給用戶 ※如果有需要
                            message.Message = $"siteNumber: [{_siteNumber}]" + message.Message;

                            // 5-2. 推播訊息給客戶端
                            await _hubContext.Clients.All.SendAsync("SendUpdate", message);
                        }
                        // 5-3. 回傳成功寫回資料庫更新
                        var ids = string.Join(", ", data.Select(item => item.SignalRMessagesId));
                        await _signalRMessages.UpdateSended(ids, _siteNumber);
                    }
                    // 6. 增加區隔時間，避免CPU無法處理                
                    await Task.Delay(_second, stoppingToken);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
