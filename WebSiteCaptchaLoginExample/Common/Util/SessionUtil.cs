namespace WebSiteCaptchaLoginExample.Common.Util
{
    public static class SessionUtil
    {
        private static IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 1. 增加一個方法，提供紀錄當前HttpContext 存取者
        /// </summary>        
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// SessionId
        /// </summary>
        public static string SessionId
        {
            get
            {
                return _httpContextAccessor?.HttpContext?.Session.GetString("SessionId") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor?.HttpContext?.Session.SetString("SessionId", value);
            }
        }

        /// <summary>
        /// 圖形驗證碼
        /// </summary>
        public static string CaptCha
        {
            get
            {
                return _httpContextAccessor?.HttpContext?.Session.GetString("CaptCha") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor?.HttpContext?.Session.SetString("CaptCha", value);
            }
        }
    }
}
