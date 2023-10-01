using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebRouterRuleExample.Models;
using WebRouterRuleExample.RouteRule;

namespace WebRouterRuleExample.Controllers
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
        
        //1. 模擬對方網站的URL
        [Route("liu-he-cai/all-results/{year?}")]
        [EnsureTrailingSlash] // 2. 使用自訂義的動作過濾器
        public IActionResult GetData(int? year)
        {
            // 在這裡編寫您的 API 邏輯，例如根據 year 取得資料 ...
            return Ok("Data for ID: " + year);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}