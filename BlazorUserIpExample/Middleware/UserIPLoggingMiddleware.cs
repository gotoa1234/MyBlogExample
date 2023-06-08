using BlazorUserIpExample.Repository;

namespace BlazorUserIpExample.Middleware
{
    public class UserIPLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISqliteRepository _respoitory;

        public UserIPLoggingMiddleware(RequestDelegate next,
            IHttpContextAccessor httpContextAccessor,
            ISqliteRepository respoitory)
        {
            _next = next;
            _respoitory = respoitory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            var now = DateTime.Now;
            string userIpAddress = GetUserIP();
            
            if (!string.IsNullOrEmpty(userIpAddress))
            {
                var row = new Model.UserIpRecordModel()
                {
                    UserIp = userIpAddress,
                    Date = now.ToString("yyyyMMdd")
                };
                _respoitory.CreatedRow(row);
            }
            await _next(context);
        }

        private string GetUserIP()
        {
            //1. 取得"X-Forwarded-For" => 反向代理伺服器的多個IP，以;區隔，最左邊是用戶的IP
            string userIPAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            //2-1. 假設沒有經過反向代理
            if (string.IsNullOrEmpty(userIPAddress))
            {
                //2-2. 取得"X-Real-IP" => 常見的一些代理伺服器會將最終的用戶真實IP放在此標頭
                userIPAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
                if (string.IsNullOrEmpty(userIPAddress))
                {
                    //3. 預設瀏覽器請求的IP
                    userIPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                }
            }
            return userIPAddress;
        }
    }
}