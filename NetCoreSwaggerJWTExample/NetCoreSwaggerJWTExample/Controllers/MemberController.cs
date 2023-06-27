using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreSwaggerJWTExample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //2-1. 這邊Authorize改用 JwtBearer 套件
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MemberController : ControllerBase
    {
        private readonly IJsonWebTokenService _jwtService;

        public MemberController(IJsonWebTokenService jsonWebToken) 
        {
            _jwtService = jsonWebToken;
        }

        /// <summary>        
        /// 1-1. 登入
        /// </summary>        
        [HttpPost]        
        [AllowAnonymous]
        public ActionResult Login(string user, string password)
        {
            string token = string.Empty;
            //1-2. 假設登入成功，回傳Token
            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
            {
                token = _jwtService.GenerateToken(user);
            }
            
            return Ok(token);
        }

        /// <summary>
        /// 驗證通過可以取得使用者訊息
        /// </summary>        
        [HttpGet]
        [Authorize]
        public ActionResult UserInfo()
        {          
            //2-2. JWT驗證通過，才會看到以下訊息
            return Ok("取得用戶資訊成功");
        }


    }
}
