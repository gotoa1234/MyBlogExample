using Microsoft.AspNetCore.Mvc;
using NetCoreSwaggerWebSiteAdditionExample.Base;

namespace NetCoreSwaggerWebSiteAdditionExample.Areas.Mobile
{
    //繼承BaseController 將不會開放
    public class MemberController : BaseController
    {
        [HttpGet]
        public IActionResult GetPersonalInfo(int userId)
        {
            return Ok("Mobile GetPersonalInfo Successful");
        }
    }
}
