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
            var result = new
            {
                RemoteIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Scheme = HttpContext.Request.Scheme,
                Host = HttpContext.Request.Host.Value
            };

            return Ok(result);
        }
    }
}
