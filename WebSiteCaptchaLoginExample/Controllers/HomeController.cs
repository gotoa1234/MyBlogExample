using Microsoft.AspNetCore.Mvc;
using WebSiteCaptchaLoginExample.Common.BaseController;
using WebSiteCaptchaLoginExample.Common.Util;
using WebSiteCaptchaLoginExample.Models;

namespace WebSiteCaptchaLoginExample.Controllers
{
    public class HomeController : BaseController
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

        /// <summary>
        /// 用戶登入
        /// </summary>
        /// <returns></returns>
        public IActionResult UserLogin()
        {
            HttpContext.Session.SetString(nameof(SessionUtil.CaptCha), CaptchaUtil.GetRandomCaptcha(5));

            var result = new LoginViewModel()
            {
                Chapcha = $@"data:image/jpeg;base64,{Convert.ToBase64String(CaptchaUtil.GetCapChatImg(SessionUtil.CaptCha))}",
            };
            return View(result);
        }

        [HttpPost]
        public IActionResult UserLoginVerify(LoginViewModel inputData)
        {
            return Ok("成功");
        }


        /// <summary>
        /// 管理者登入
        /// </summary>
        /// <returns></returns>
        public IActionResult AdminLogin()
        {
            HttpContext.Session.SetString(nameof(SessionUtil.CaptCha), CaptchaUtil.GetRandomCaptcha(5));

            var result = new LoginViewModel()
            {
                Chapcha = $@"data:image/jpeg;base64,{Convert.ToBase64String(CaptchaUtil.GetCapChatImg(SessionUtil.CaptCha))}",
            };
            return View(result);
        }

        [HttpPost]
        public IActionResult AdminLoginVerify(LoginViewModel inputData)
        {
            return Ok("成功");
        }

    }
}