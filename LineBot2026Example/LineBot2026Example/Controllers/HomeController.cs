using Line.Messaging.Webhooks;
using LineBot2026Example.Service;
using Microsoft.AspNetCore.Mvc;

namespace LineBot2026Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILineBotService _lineBotService;
        private readonly ILogger<HomeController> _logger;
        private readonly string _channelSecret;

        public HomeController(ILogger<HomeController> logger, ILineBotService lineBotService, IConfiguration config)
        {
            _logger = logger;
            _lineBotService = lineBotService;
            _channelSecret = config["LineBot:ChannelSecret"]!;
        }

        /// <summary>
        /// 用於 WebHook 接收事件，請將此 URL 設定於 LINE Developers Console 的 Webhook URL 欄位中。
        /// <para>https://manager.line.biz/</para>
        /// </summary>
        [HttpPost]
        [Route("api/LineBotApi/Post")]
        public async Task<IActionResult> LineBotWebHook()
        {
            // 1. 取得並驗證 Webhook 事件（GetWebhookEventsAsync 內部自動驗簽）
            var events = await HttpContext.Request.GetWebhookEventsAsync(_channelSecret);
            await _lineBotService.HandleEventsAsync(events);
            return Ok();
        }

        /// <summary>
        /// 用於 LineBot 推播訊息至指定群組
        /// <para>https://chat.line.biz/</para>
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> LinePublish([FromBody] LineRequest request)
        {
            try
            {
                // 從 request 物件拿資料
                await _lineBotService.HandleEventsPublishAsync(request.Message, request.GroupId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "推播失敗");
                return BadRequest("推播失敗");
            }
            return Ok("推播成功");
        }

        public IActionResult Index()
        {
            return View();
        }

        public class LineRequest
        {
            public string Message { get; set; } = string.Empty;
            public string GroupId { get; set; } = string.Empty;
        }
    }
}
