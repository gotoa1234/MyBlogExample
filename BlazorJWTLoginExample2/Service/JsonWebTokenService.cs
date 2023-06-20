using BlazorJWTLoginExample2.Model;
using BlazorJWTLoginExample2.Repository;
using BlazorJWTLoginExample2.Util;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorJWTLoginExample2.Service
{
    public class JsonWebTokenService
    {
        private readonly ISqliteRepository _repository;

        public JsonWebTokenService(ISqliteRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 產生JWT 
        /// </summary>
        public string GenerateToken(LoginModel user,
            int id, string type, string nickName)
        {
            var currentToken = GetToken(user.Username);
            if (!string.IsNullOrEmpty(currentToken))
            {
                return currentToken;
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("type", type),
                new Claim("id", $@"{id}"),
                new Claim("nickname", nickName),
            };
            
            var jwtKey = Encoding.UTF8.GetBytes(ConstUtil.SignKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                issuer: ConstUtil.Issuer,     // 發行者：若解析驗證Token正確性時這個不同會視為驗證失敗
                audience: ConstUtil.Audience, // 使用者：若解析驗證Token正確性時這個不同會視為驗證失敗
                signingCredentials: signingCredentials,//簽名憑證：若解析驗證Token正確性時這個不同會視為驗證失敗
                claims: claims,      // 資料：可攜帶用戶資訊，像密碼類的不建議放進，如果被收集過多的token仍有可能被破解
                expires: DateTime.UtcNow.AddSeconds(120)//過期時間：如果超過此token會直接報廢
            );

            //1. 註冊Token到資料庫中
            var newToken = new JwtSecurityTokenHandler().WriteToken(token);
            RegistToken(user.Username, newToken);

            return newToken;             
        }

        public string GetToken(string account)
        {
            return _repository.GetToken(account);
        }

        public void RegistToken(string userName, string token)
        {
            _repository.InsertOrUpdateToken(userName, token);
        }

        public bool IsMatchToken(string currentToken)
        {            
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(currentToken);

            //1. 檢核時間簽名憑證
            var jwtKey = Encoding.UTF8.GetBytes(ConstUtil.SignKey);                        
            tokenHandler.ValidateToken(currentToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = ConstUtil.Audience,
                ValidIssuer = ConstUtil.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
            }, out var validatedToken);

            //2. 檢核是否有用戶
            var UserName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(UserName))
                return false;

            //3. 檢核資料庫
            if (currentToken != _repository.GetToken(UserName))
            {
                return false;
            }
            return true;
        }
    }
}
