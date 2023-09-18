using CustomerDeviceProxyModeSwitchingExample.Common;
using Microsoft.AspNetCore.Mvc;

namespace CustomerDeviceProxyModeSwitchingExample.Controllers
{
    // 2. 將原本的Home繼承我們的BaseController
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
        
        
        // 3. 建立用戶客製化設備頁面，提供用戶選擇自己的設定
        public IActionResult Customer()
        {
            ChangeMode();
            return View();
        }

        
        // 4-1. 提供一個API讓用戶隨時可以設定自己的使用設備，跳過User-Agent
        [HttpGet]
        public IActionResult SettingMode(int isMobile)
        {
            // 4-2. 用戶的選擇紀錄在Session中
            SessionUtil.IsUseMobile = isMobile == 1;
            return Ok();
        }

        private void ChangeMode()
        {
            // 3-1. 判斷用戶的選擇
            if (SessionUtil.IsUseMobile)
            {
                // 3-2. 行動版 - 範例的關係直接用 IPhone 的 User-Agent
                Request.Headers["User-Agent"] = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_2_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.3 Mobile/15E148 Safari/604.1";
            }
            else 
            {
                // 3-2. 電腦版 - 的User-Agent
                Request.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";
            }
        }
    }
}