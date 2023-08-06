using Microsoft.AspNetCore.Mvc;
using NetCoreSwaggerWebSiteAdditionExample.Base;

namespace NetCoreSwaggerWebSiteAdditionExample.Areas.External
{
    //繼承OpenBaseController 將會開放
    public class MemberController : OpenBaseController
    {
        /// <summary>
        /// 第三方API
        /// </summary>
        /// <param name="userId">測試</param>
        /// <returns></returns>
        [HttpGet]        
        public IActionResult GetPersonalInfo(int userId)
        {
            return Ok("GetPersonalInfo Successful");
        }

        //也可以強制不開放
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetPersonalInfoNoShow(int userId)
        {
            return Ok("GetPersonalInfo Successful");
        }
    }
}
