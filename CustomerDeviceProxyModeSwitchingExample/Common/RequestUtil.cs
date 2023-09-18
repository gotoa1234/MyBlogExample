using System.Web;

namespace CustomerDeviceProxyModeSwitchingExample.Common
{
    /// <summary>
    /// HttpRequest 的共用靜態類
    /// </summary>
    public static class RequestUtil
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 取得BrowserInfo
        /// </summary>
        /// <returns></returns>
        public static string GetClientBrowserUserAgent()
        {
            string result = string.Empty;
            if (_httpContextAccessor?.HttpContext != null)
            {
                var userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
                result = HttpUtility.HtmlEncode(userAgent);
            }
            return result;
        }

        /// <summary>
        /// 判斷是否為行動設備
        /// </summary>
        /// <returns></returns>
        public static bool IsMobile()
        {
            var userAgent = GetClientBrowserUserAgent();
            bool isMobile = userAgent.ToLower().Contains("android") || 
                            userAgent.ToLower().Contains("iphone") || 
                            userAgent.ToLower().Contains("ipad") || 
                            userAgent.ToLower().Contains("windows phone");
            return isMobile;
        }
    }
}
