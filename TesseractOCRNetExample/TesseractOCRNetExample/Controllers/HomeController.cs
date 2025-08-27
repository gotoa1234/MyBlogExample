using Microsoft.AspNetCore.Mvc;
using TesseractOCRNetExample.Service;

namespace TesseractOCRNetExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITesseractOCRService _tesseractOCRService;
        private readonly ITesseractService _tesseractService;

        public HomeController(ILogger<HomeController> logger,
            ITesseractOCRService tesseractOCRService,
             ITesseractService tesseractService)
        {
            _logger = logger;
            _tesseractOCRService = tesseractOCRService;
            _tesseractService = tesseractService;
        }

        public IActionResult Index()
        {
            ViewBag.TesseractOCR = _tesseractOCRService.TesseractOCRVersionImage();
            //ViewBag.Tesseract = _tesseractService.TesseractVersionImage();
            return View();
        }
    }
}
