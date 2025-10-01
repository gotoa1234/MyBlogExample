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
        /// ��e�X�ݪ�(�Τ�) ��ݬ����� IP
        /// </summary>        
        [HttpGet]
        public IActionResult CallAPI()
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress;

            // ��K Demo �����ഫ���� IPv4
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
