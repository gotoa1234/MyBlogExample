namespace BlazorJWTLoginExample2.Util
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

    }
}
