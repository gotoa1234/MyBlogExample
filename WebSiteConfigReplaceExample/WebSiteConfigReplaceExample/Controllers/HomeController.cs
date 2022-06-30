using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebSiteConfigReplaceExample.Models;
using WebSiteConfigReplaceExample.Service;

namespace WebSiteConfigReplaceExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConfigureSettingService _myConfigSetting;

        public HomeController(ILogger<HomeController> logger,
            ConfigureSettingService service)
        {
            _logger = logger;
            _myConfigSetting = service;
        }

        public IActionResult Index()
        {
            //STEP3-1：以下兩種方法皆可取得設定值
            ViewBag.MasterDatabase = NotDIGetAppsettingMethod();
            ViewBag.MasterDatabase = _myConfigSetting.MasterDatabase;
            return View();
        }

        /// <summary>
        /// STEP3-2：非DI的方式取得設定檔案的方法
        /// </summary>
        /// <returns></returns>
        private string NotDIGetAppsettingMethod()
        {
            IConfiguration conf = (new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                );
            return conf["ConnectionStrings:MasterDatabase"].ToString();
        }

        public IActionResult Privacy()
        {
           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}