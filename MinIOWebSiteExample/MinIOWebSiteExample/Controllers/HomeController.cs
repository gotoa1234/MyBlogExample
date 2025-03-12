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

        /// <summary>
        /// 頁面資料
        /// </summary>
        public IActionResult Index()
        {
            var getResult = (_teacherManageService.GetTeachers()).Result;
            return View(getResult);
        }

        /// <summary>
        /// 上傳單一檔案
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string bucketName)
        {
            await _teacherManageService.UploadFile(file, bucketName);
            return Ok("Upload Success!");
        }

        /// <summary>
        /// 下載單一檔案
        /// </summary>
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

            return BadRequest(new { message = $"Cannot download file {fileName}." });
        }

        /// <summary>
        /// 刪除單一檔案
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteFile(string fileName, string bucketName)
        {
            await _teacherManageService.DeleteFile(fileName, bucketName);
            return Ok($"{fileName} Has been remove.");
        }

        /// <summary>
        /// 刪除帳號
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteAccount(long Id)
        {
            await _teacherManageService.DeleteAccount(Id);
            return Ok(new { message = $"Teacher has been remove ID：{Id}" });
        }

        /// <summary>
        /// 新建帳號
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CreateAccount()
        {
            var createdId = await _teacherManageService.CreateAccount();
            return Ok(new { message = $"Teacher account has been create ID:{createdId}" });
        }
    }
}
