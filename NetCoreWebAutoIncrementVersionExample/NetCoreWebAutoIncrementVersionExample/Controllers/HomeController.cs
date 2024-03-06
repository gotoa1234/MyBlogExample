using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public IActionResult Index()
        {
            Assembly assembly = Assembly.GetEntryAssembly();            
            ViewBag.version = "Not Load";
            if (assembly != null)
            {
                // 取得當前執行的程式集的 AssemblyInformationalVersionAttribute 屬性
                var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                if (attribute != null)
                {                    
                    ViewBag.version = attribute.InformationalVersion;                    
                }
            };
            return View();
        }
    }
}