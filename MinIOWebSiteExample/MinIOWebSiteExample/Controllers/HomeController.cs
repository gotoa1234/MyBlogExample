using Microsoft.AspNetCore.Mvc;
using Minio.DataModel.Args;
using MinIOWebSiteExample.Service;
using System.Security.AccessControl;

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

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string bucketName)
        {
            await _teacherManageService.UploadFile(file, bucketName);
            return Ok("上傳成功");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string fileName, string bucketName)
        {
            var fileStream = await _teacherManageService.DownloadFile(fileName, bucketName);
            if (fileStream != null)
            {
                return new FileStreamResult(fileStream, "application/octet-stream")
                {
                    FileDownloadName = fileName
                };
            }

            return BadRequest(new { message = $@"Cannot download file {fileName}." });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
