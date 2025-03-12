using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Models;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Service.Interface;

namespace ZeroDowntimeDeploymentForDockerWebsiteExample.Service
{
    public class MyAuthenticationService : IMyAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _CookieExpireHour = 1;

        public MyAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 登入註冊
        /// </summary>        
        public async Task SignInAsync(UserInfo userInfo)
        {
            // 1. ASP.NET Core 內建的 Cookie Authentication 的方法 - 票證方式完成 Cookie 註冊
            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Name, userInfo.Username),
                 new Claim(ClaimTypes.Role, userInfo.Role)
             };

            // 2. 配置 Cookie 時間
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(_CookieExpireHour)
            };

            if (_httpContextAccessor == null)
                return;

            if (_httpContextAccessor.HttpContext == null)
                return;

            // 3. 完成瀏覽器使用的 Cookie
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        /// <summary>
        /// 取得 Cookie 的過期時間設定值
        /// </summary>        
        public int GetCookieExpireHour() {
            return _CookieExpireHour;
        }
    }
}
