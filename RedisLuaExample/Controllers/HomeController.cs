using Microsoft.AspNetCore.Mvc;
using RedisLuaExample.Service;

namespace RedisLuaExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGeneratorService _generator;

        public HomeController(ILogger<HomeController> logger,
            IGeneratorService generator)
        {
            _logger = logger;
            _generator = generator;
        }

        /// <summary>
        /// ­¶­±¸ê®Æ
        /// </summary>
        public async Task<IActionResult> Index()
        { 
            var getResult = await _generator.GeneratorReport();
            var result = new { data = getResult };
            return View(result);
        }
    }
}
