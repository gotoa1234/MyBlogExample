using Microsoft.AspNetCore.Mvc;

namespace NotUseForwardedHeadersExample.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

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
