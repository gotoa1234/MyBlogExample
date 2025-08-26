using Microsoft.AspNetCore.Mvc;
using TesseractOCRNetExample.Service;

namespace TesseractOCRNetExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITesseractOCRService _tesseractOCRService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {            
            return View();
        }
    }
}
