using Microsoft.AspNetCore.Mvc;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Models;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Service.Interface;

namespace ZeroDowntimeDeploymentForDockerWebsiteExample.Controllers
{
    /// <summary>
    /// 入口代碼
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IMyAuthenticationService _myAuthService;
        private readonly IMyUserSessionService _myUserSessionService;
        public AccountController(
            IMyAuthenticationService myAuthService,
            IMyUserSessionService myUserSessionService)
        {
            _myAuthService = myAuthService;
            _myUserSessionService = myUserSessionService;
        }

        /// <summary>
        /// 要求登入 - 檢視頁面
        /// </summary>        
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登入
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. 驗證登入邏輯，這裡簡化為示範，都有填寫就可登入
                if (false == string.IsNullOrEmpty(model.Username) &&
                    false == string.IsNullOrEmpty(model.Password)
                    )
                {
                    // 2-1. 存儲用戶資訊到 Session
                    var userInfo = new UserInfo
                    {
                        UserId = 1,//為了Demo 說明，統一用 UserId = 1 
                        Username = model.Username,
                        Role = "Admin"
                    };

                    // 2-2. 保存 Session 和 Redis
                    await _myUserSessionService.SaveUserSessionAsync(userInfo);

                    // 2-3. 添加認證票據存到 Cookie
                    await _myAuthService.SignInAsync(userInfo);

                    // 3. 導向到登入後的首頁
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "用戶名或密碼錯誤");
            }
            return View(model);
        }
    }
}
