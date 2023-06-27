using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCoreSwaggerJWTExample
{
    public class JsonWebTokenService : IJsonWebTokenService
    {
        private readonly string _secretKey;
        private readonly string _issur;
        private readonly string _audience;

        public JsonWebTokenService(IConfiguration configuration)
        {
            _secretKey = configuration.GetValue<string>("JwtSettings:SignKey");
            _issur = configuration.GetValue<string>("JwtSettings:Issuer");
            _audience = configuration.GetValue<string>("JwtSettings:Audience");
        }

        //1. 產生Token
        public string GenerateToken(string user)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, user)                
            };

            var jwtKey = Encoding.UTF8.GetBytes(_secretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256);

            //2-1. Token產生的issuer、audience、signingCredentials 要與JWTBearer 的配置相同
            var token = new JwtSecurityToken(
                issuer: _issur,    
                audience: _audience, 
                signingCredentials: signingCredentials,
                claims: claims,                    
                expires: DateTime.UtcNow.AddSeconds(10)
            );
            //2-2. 轉為String回傳
            var newToken = new JwtSecurityTokenHandler().WriteToken(token);
            return newToken;
        }
    }
}
