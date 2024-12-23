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
        /// �������
        /// </summary>
        public IActionResult Index()
        {
            var getResult = (_teacherManageService.GetTeachers()).Result;
            return View(getResult);
        }

        /// <summary>
        /// �W�ǳ�@�ɮ�
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string bucketName)
        {
            await _teacherManageService.UploadFile(file, bucketName);
            return Ok("Upload Success!");
        }

        /// <summary>
        /// �U����@�ɮ�
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
        /// �R����@�ɮ�
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteFile(string fileName, string bucketName)
        {
            await _teacherManageService.DeleteFile(fileName, bucketName);
            return Ok($"{fileName} Has been remove.");
        }

        /// <summary>
        /// �R���b��
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteAccount(long Id)
        {
            await _teacherManageService.DeleteAccount(Id);
            return Ok(new { message = $"Teacher has been remove ID�G{Id}" });
        }

        /// <summary>
        /// �s�رb��
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CreateAccount()
        {
            var createdId = await _teacherManageService.CreateAccount();
            return Ok(new { message = $"Teacher account has been create ID:{createdId}" });
        }
    }
}
