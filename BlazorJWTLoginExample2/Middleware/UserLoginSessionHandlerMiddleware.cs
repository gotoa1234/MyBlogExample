using BlazorJWTLoginExample2.Util;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BlazorJWTLoginExample2.Middleware
{
    public class UserLoginSessionHandlerMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _secretKey;
        private readonly string _issur;
        private readonly string _audience;

        public UserLoginSessionHandlerMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _secretKey = ConstUtil.SignKey;
            _issur = ConstUtil.Issuer;
            _audience = ConstUtil.Audience;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Session.GetString("JWT");
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                // 1. 驗證並解析 JWT Token
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _issur,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out var validatedToken);

                // 3. Set Session
                if (validatedToken != null)
                {
                    // 設定Session
                    var claims = tokenHandler.ReadJwtToken(token).Claims;
                    var account = claims.FirstOrDefault(item => nameof(SessionUtil.Account) == item.Type)?.Value ?? string.Empty;
                    var sessionId = claims.FirstOrDefault(item => JwtRegisteredClaimNames.Jti == item.Type)?.Value ?? string.Empty;
                    context.Session.SetString(nameof(SessionUtil.Account), account);
                    context.Session.SetString(nameof(SessionUtil.SessionId), sessionId);
                    SessionUtil.Configure(context.RequestServices.GetService<IHttpContextAccessor>());
                }
            }
            catch
            {
            }
            //Authorized
            await _next(context);
        }
    }
}
