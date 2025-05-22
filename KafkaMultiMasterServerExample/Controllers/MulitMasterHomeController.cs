using KafkaMultiMasterServerExample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KafkaMultiMasterServerExample.Controllers
{
    public class MulitMasterHomeController : Controller
    {
        private readonly ILogger<MulitMasterHomeController> _logger;

        public MulitMasterHomeController(ILogger<MulitMasterHomeController> logger)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
