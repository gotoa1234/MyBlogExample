namespace WebSiteSessionUtilExample.cs.Common.Util
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

        #region 2. 以下為要記錄的Session 資料

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
        /// 帳號
        /// </summary>
        public static string Account
        {
            get
            {
                return _httpContextAccessor?.HttpContext?.Session.GetString("Account") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor?.HttpContext?.Session.SetString("Account", value);
            }
        }

        /// <summary>
        /// 時間紀錄
        /// </summary>
        public static string DateTimeRecord
        {
            get
            {
                return _httpContextAccessor?.HttpContext?.Session.GetString("DateTimeRecord") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor?.HttpContext?.Session.SetString("DateTimeRecord", value);
            }
        }

        /// <summary>
        /// 年齡
        /// </summary>
        public static int Age
        {
            get
            {
                return _httpContextAccessor?.HttpContext?.Session.GetInt32("Age") ?? 0;
            }
            set
            {
                _httpContextAccessor?.HttpContext?.Session.SetInt32("Age", Age);
            }
        }

        #endregion
    }
}
