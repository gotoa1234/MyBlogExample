using GetDockerContainerEnvironmentParameterExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace GetDockerContainerEnvironmentParameterExample.Controllers
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
            var getEnviromentInfo = new ContainerEnvironmentModel();
            return View(getEnviromentInfo);
        }
    }
}
