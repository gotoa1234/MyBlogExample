using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// 1-1. 上傳圖片頁面
        /// </summary>
        /// <returns></returns>
        public IActionResult UploadImageFile()
        {
            return View();
        }

        /// <summary>
        /// 1-2. 上傳圖片按鈕
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpLoadFile([FromForm] UploadModel upload)
        {
            if (ModelState.IsValid)
            {
                _upload.UploadImage(upload.Image);
            }
            return RedirectToAction("UploadImageFile", "Home");
        }

        
        /// <summary>
        /// 2-1. 上傳加密圖片頁面
        /// </summary>
        /// <returns></returns>
        public IActionResult UploadEncryptImageFile()
        {
            return View();
        }

        /// <summary>
        /// 2-2. 上傳加密圖片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpLoadFileEncrypt([FromForm] UploadModel upload)
        {
            if (ModelState.IsValid)
            {
                _upload.UploadImageEncrypt(upload.Image);
            }
            return RedirectToAction("UploadEncryptImageFile", "Home");
        }
    }
}