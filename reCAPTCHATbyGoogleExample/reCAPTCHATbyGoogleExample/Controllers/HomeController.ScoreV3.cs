using Microsoft.AspNetCore.Mvc;


namespace reCAPTCHATbyGoogleExample.Controllers;

public partial class HomeController : Controller
{
    [HttpGet]
    public IActionResult ScoreV3()
    {
        return View();
    }
}
