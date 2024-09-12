using Microsoft.AspNetCore.Mvc;
using MinIOWebSiteExample.Service;

namespace MinIOWebSiteExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITeacherManageService _teacherManageService;
        public HomeController(ILogger<HomeController> logger,
            ITeacherManageService teacherManageService)
        {
            _logger = logger;
            _teacherManageService = teacherManageService;
        }

        public IActionResult Index()
        {
            var getResult = (_teacherManageService.GetTeachers()).Result;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
