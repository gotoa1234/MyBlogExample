using Microsoft.AspNetCore.Mvc;

namespace UseForwardedHeadersExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 當前訪問者(用戶) 後端紀錄的 IP
        /// </summary>        
        [HttpGet]
        public IActionResult CallAPI()
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress;

            // 方便 Demo 說明轉換成純 IPv4
            var ipv4 = remoteIp?.MapToIPv4();

            var result = new
            {
                RemoteIp = ipv4,
                Scheme = HttpContext.Request.Scheme,
                Host = HttpContext.Request.Host.Value
            };

            return Ok(result);
        }
    }
}
