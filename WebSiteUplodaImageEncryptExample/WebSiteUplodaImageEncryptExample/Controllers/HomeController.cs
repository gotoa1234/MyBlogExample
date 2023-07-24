using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebSiteUplodaImageEncryptExample.Models;
using WebSiteUplodaImageEncryptExample.Service;

namespace WebSiteUplodaImageEncryptExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUploadFileService _upload;

        public HomeController(ILogger<HomeController> logger,
            IUploadFileService upload)
        {
            _logger = logger;
            _upload = upload;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadImageFile()
        {
            return View();
        }

        /// <summary>
        /// 上傳圖片按鈕
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpLoadFile([FromForm] UploadModel upload)
        {
            _upload.UploadImage(upload.Image);
            return Ok();
        }

        /// <summary>
        /// 上傳圖片並加密
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpLoadFileEncrypt([FromForm] UploadModel upload)
        {
            _upload.UploadImage(upload.Image);
            return Ok();
        }
    }
}