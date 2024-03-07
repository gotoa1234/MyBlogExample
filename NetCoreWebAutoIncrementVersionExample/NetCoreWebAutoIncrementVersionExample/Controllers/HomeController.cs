using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace NetCoreWebAutoIncrementVersionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            
            _logger = logger;
        }

        /// <summary>
        /// 手動自定義版號
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // 取得 Assembly Version
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            ViewBag.AssemblyVersion = assemblyName.Version;

            // 取得 File Version
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location);
            ViewBag.FileVersion = fileVersionInfo.FileVersion;
            
            // 取得 取得軟體版本資訊 / 取得 Nuget (套件版本) ※只能存在一種屬性
            // 此專案非 Nuget 套件，因此使用 Version 
            var version = typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            ViewBag.Version = version;           

            return View();
        }

        /// <summary>
        /// 自動遞增版號
        /// </summary>
        /// <returns></returns>
        public IActionResult AutoIncretmentVersion()
        {
            // 取得 Prefix Version
            string prefixVersion = typeof(Program).Assembly.GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>().InformationalVersion;
            ViewBag.PrefixVersion = prefixVersion;

            // 此時合併版號應存在於 Version 中
            var version = typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            ViewBag.Version = version;

            // 取得 Suffix Version ※為動態寫入值，通常不可直接取得
            string suffixVersion = version.Split('.').Length >= 4 
                                   ? version.Split('.')[3] 
                                   : "0";
            ViewBag.SuffixVersion = suffixVersion;

            return View();
        }
    }
}