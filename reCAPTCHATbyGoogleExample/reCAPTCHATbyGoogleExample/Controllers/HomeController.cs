using Microsoft.AspNetCore.Mvc;

namespace reCAPTCHATbyGoogleExample.Controllers;

public partial class HomeController : Controller
{
    private readonly string MyWebSiteKey = $@"";
    private readonly string _secretKey = $@"";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }        
}
