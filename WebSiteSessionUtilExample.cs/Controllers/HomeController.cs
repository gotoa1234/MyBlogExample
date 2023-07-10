using Microsoft.AspNetCore.Mvc;
using WebSiteSessionUtilExample.cs.Common.BaseController;
using WebSiteSessionUtilExample.cs.Common.Util;

namespace WebSiteSessionUtilExample.cs.Controllers
{
    //1. 將Controller 繼承於 BaseController 使其記錄HttpContext
    public class HomeController : BaseController
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

        public IActionResult AddSession()
        {
            //2. 模擬假設登入時，會把Session記錄下來
            HttpContext.Session.SetString(nameof(SessionUtil.SessionId), HttpContext.Session.Id);
            HttpContext.Session.SetString(nameof(SessionUtil.Account), "Session工作者");
            HttpContext.Session.SetString(nameof(SessionUtil.DateTimeRecord), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            HttpContext.Session.SetInt32(nameof(SessionUtil.Age), 18);
            return View();
        }
    }
}