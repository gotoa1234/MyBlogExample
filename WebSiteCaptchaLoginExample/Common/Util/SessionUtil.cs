namespace WebSiteCaptchaLoginExample.Common.Util
{
    public static class SessionUtil
    {
        private static IHttpContextAccessor _httpContextAccessor;
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
        /// 圖形驗證碼綁定名稱 - 使驗證碼與某些Action具有關聯性
        /// </summary>
        public static string CaptChaBindName
        {
            get
            {
                return _httpContextAccessor?.HttpContext?.Session.GetString("CaptChaBindName") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor?.HttpContext?.Session.SetString("CaptChaBindName", value);
            }
        }

        /// <summary>
        /// 圖形驗證碼
        /// </summary>
        public static string CaptCha
        {
            get
            {
                return _httpContextAccessor?.HttpContext?.Session.GetString($@"{CaptChaBindName}_CaptCha") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor?.HttpContext?.Session.SetString($@"{CaptChaBindName}_CaptCha", value);
            }
        }
    }
}
