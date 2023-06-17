using BlazorJWTLoginExample.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorJWTLoginExample.Service
{
    public class JsonWebTokenService
    {
        public const string _issuer = "";
        public const string _audience = "";
        public const string _secretKey = "this_is_a_secure_key_with_length_greater_than_32";

        /// <summary>
        /// 產生JWT 
        /// </summary>
        public string GenerateToken(LoginModel user,
            int id, string type, string nickName)
        {
            // 1.攜帶用戶資訊(塞在Token中)
            var claims = new[]
             {
             new Claim(ClaimTypes.Name, user.Username),
             new Claim("type", type),
             new Claim("id", $@"{id}"),
             new Claim("nickname", nickName),
         };

            // 2. 使用加密金鑰 與 SHA256加密算法創建簽名憑證
            var jwtKey = Encoding.UTF8.GetBytes(_secretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256);

            // 3. 定義 JWT 的內容
            var token = new JwtSecurityToken(
                issuer: _issuer,     // 發行者：若解析驗證Token正確性時這個不同會視為驗證失敗
                audience: _audience, // 使用者：若解析驗證Token正確性時這個不同會視為驗證失敗
                signingCredentials: signingCredentials,//簽名憑證：若解析驗證Token正確性時這個不同會視為驗證失敗
                claims: claims,      // 資料：可攜帶用戶資訊，像密碼類的不建議放進，如果被收集過多的token仍有可能被破解
                expires: DateTime.UtcNow.AddSeconds(10)//過期時間：如果超過此token會直接報廢
            );

            // 4. 最後產生token為字串格式
            return  new JwtSecurityTokenHandler().WriteToken(token);             
        }
    }
}
