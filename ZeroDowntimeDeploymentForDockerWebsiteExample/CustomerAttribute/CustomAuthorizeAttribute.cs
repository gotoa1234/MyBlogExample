using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Models;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Service.Interface;

namespace ZeroDowntimeDeploymentForDockerWebsiteExample.CustomerAttribute
{
    /// <summary>
    /// 掛載的 Attribute 名稱
    /// </summary>
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
        {
        }
    }

    /// <summary>
    /// 實際 CustomAuthorizeAttribute 實作內容
    /// </summary>
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly ICacheService _cacheService;

        public CustomAuthorizeFilter(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// 當驗證時觸發
        /// </summary>        
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                // 使用 ICacheService 取得用戶資訊
                var userInfo = await _cacheService.GetAsync<UserInfo>(nameof(UserInfo));

                // 如果無資料導向到登入頁
                if (userInfo == null)
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                    return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
