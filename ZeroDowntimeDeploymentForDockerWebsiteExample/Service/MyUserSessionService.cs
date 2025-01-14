using ZeroDowntimeDeploymentForDockerWebsiteExample.Models;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Service.Interface;

namespace ZeroDowntimeDeploymentForDockerWebsiteExample.Service
{
    /// <summary>
    /// 用戶 Session 設定
    /// </summary>
    public class MyUserSessionService: IMyUserSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMyAuthenticationService _myAuthenticationService;        
        private readonly ICacheService _cacheService;
        private readonly string _userSessionName = "UserSession_";
        private readonly double _sessionExpireHour = 1;

        public MyUserSessionService(
            IHttpContextAccessor httpContextAccessor,
            IMyAuthenticationService myAuthenticationService,
            ICacheService cacheService)
        {
            _httpContextAccessor = httpContextAccessor;
            _myAuthenticationService = myAuthenticationService;
            _cacheService = cacheService;
            _sessionExpireHour = _myAuthenticationService.GetCookieExpireHour();
        }

        /// <summary>
        /// 保存用戶 Session 資訊
        /// </summary>       
        public async Task SaveUserSessionAsync(UserInfo userInfo)
        {
            await _cacheService.SetAsync($"{_userSessionName}{userInfo.UserId}", userInfo);
        }

        /// <summary>
        /// 取得用戶 Session 資訊
        /// </summary>        
        public async Task<UserInfo> GetUserSessionAsync(int userId)
        {
            var result = await _cacheService.GetAsync<UserInfo>($"{_userSessionName}{userId}");
            return result;
        }

        /// <summary>
        /// 移除用戶 Session 資訊
        /// </summary>        
        public async Task RemoveUserSessionAsync(int userId)
        {
           await _cacheService.RemoveAsync($"{_userSessionName}{userId}");
        }
    }
}
