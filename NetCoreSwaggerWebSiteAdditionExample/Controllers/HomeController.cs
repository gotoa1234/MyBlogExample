using Microsoft.AspNetCore.Mvc;


namespace NetCoreSwaggerWebSiteAdditionExample.Controllers
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("api/Home/GetPersonalInfo")]
        public IActionResult GetPersonalInfo(int userId)
        {            
            return Ok();
        }

        [HttpPost]
        [Route("api/Home/GetDeposit")]        
        public IActionResult GetDeposit(int userId, string password)
        {
            return Ok();
        }
    }
}