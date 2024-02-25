using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreSwaggerJWTExample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //3. 這邊Authorize改用 JwtBearer 套件
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
        /// <param name="user">使用者帳號</param>
        /// <param name="password">使用者密碼</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /
        ///     {
        ///        "user": "tester1",
        ///        "password": "123456",        
        ///     }
        ///
        /// </remarks>
        /// <returns>字串(String)格式的JWT </returns>
        [HttpGet]
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
        /// 2-1. 使用者訊息API
        /// </summary>        
        /// <remarks>        
        /// "由JWT Toekn自動驗證" <br /> "JWT驗證錯誤返回401"  
        /// </remarks>  
        /// <returns>字串(String)訊息</returns>
        /// <response code="200">返回成功的訊息："取得用戶資訊成功"</response>
        /// <response code="401">驗證錯誤</response>
        [HttpGet]
        [Authorize]
        public ActionResult UserInfo()
        {
            //2-2. 驗證通過可以取得使用者訊息
            return Ok("取得用戶資訊成功");
        }


    }
}
