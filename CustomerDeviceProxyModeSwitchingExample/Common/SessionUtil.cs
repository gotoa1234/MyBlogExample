namespace CustomerDeviceProxyModeSwitchingExample.Common
{
    /// <summary>
    /// Session 的共用靜態類
    /// </summary>
    public class SessionUtil
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 用戶自己設定當前的瀏覽模式
        /// </summary>
        public static bool IsUseMobile
        {
            get
            {
                var resultDevice = _httpContextAccessor?.HttpContext?.Session.GetInt32("UseDevice") ?? 0;
                return resultDevice == 1;
            }
            set
            {
                _httpContextAccessor?.HttpContext?.Session.SetInt32("UseDevice", value ? 1 : 0);
            }
        }
    }
}
