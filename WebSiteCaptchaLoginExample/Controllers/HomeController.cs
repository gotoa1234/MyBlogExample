using Microsoft.AspNetCore.Mvc;
using WebSiteCaptchaLoginExample.Common.Attriubte;
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
        [CaptchaBinding(CaptchaBindingName = nameof(UserLogin), Generate = true)]
        public IActionResult UserLogin()
        {            
            //4. 使用 CaptchaUtil.GetCapChatImg 產生Base64的驗證碼圖形
            var result = new LoginViewModel()
            {
                Chapcha = $@"data:image/jpeg;base64,{Convert.ToBase64String(CaptchaUtil.GetCapChatImg(SessionUtil.CaptCha))}",
            };
            return View(result);
        }

        /// <summary>
        /// 用戶登入按鈕-進行登入
        /// </summary>        
        [HttpPost]
        [CaptchaBinding(CaptchaBindingName = nameof(UserLogin))]
        public IActionResult UserLoginVerify(LoginViewModel inputData)
        {
            //1. 進行登入時會依照綁定的"設置名稱" 取得對應頁面的-圖形驗證碼，就不會造成A頁面卻吃到B頁面驗證碼的錯誤
            if (inputData.SubmitData.InputCaptcha == SessionUtil.CaptCha)
                return Ok("驗證成功");
            else
                return Ok("登入失敗");
        }


        /// <summary>
        /// 管理者登入
        /// </summary>        
        [CaptchaBinding(CaptchaBindingName = nameof(AdminLogin), Generate = true)]        
        public IActionResult AdminLogin()
        {
            var result = new LoginViewModel()
            {
                Chapcha = $@"data:image/jpeg;base64,{Convert.ToBase64String(CaptchaUtil.GetCapChatImg(SessionUtil.CaptCha))}",
            };
            return View(result);
        }

        /// <summary>
        /// 管理者登入按鈕-進行登入
        /// </summary>
        [HttpPost]
        [CaptchaBinding(CaptchaBindingName = nameof(AdminLogin))]
        public IActionResult AdminLoginVerify(LoginViewModel inputData)
        {
            if (inputData.SubmitData.InputCaptcha == SessionUtil.CaptCha)
                return Ok("驗證成功");
            else
                return Ok("登入失敗");
        }

    }
}