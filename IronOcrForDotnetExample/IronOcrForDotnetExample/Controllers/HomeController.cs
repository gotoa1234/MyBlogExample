using IronOcrForDotnetExample.Service;
using Microsoft.AspNetCore.Mvc;

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
            ViewBag.IronOCR = _ocrService.IronOCR();
            return View();
        }
    }
}
