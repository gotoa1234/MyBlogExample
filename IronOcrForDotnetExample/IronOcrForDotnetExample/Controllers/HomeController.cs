using IronOcrForDotnetExample.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IronOcrForDotnetExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IIronOCRService _ocrService;

        public HomeController(ILogger<HomeController> logger,
            IIronOCRService ocrService)
        {
            _logger = logger;
            _ocrService = ocrService;
        }

        public IActionResult Index()
        {
            ViewBag.Title = _ocrService.IronOCR();
            return View();
        }
    }
}
