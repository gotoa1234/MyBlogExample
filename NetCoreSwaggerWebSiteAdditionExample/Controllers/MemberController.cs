using Microsoft.AspNetCore.Mvc;

namespace NetCoreSwaggerWebSiteAdditionExample.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/Member/GetMemberInfo")]
        public IActionResult GetPersonalInfo(int userId)
        {
            return Ok("GetPersonalInfo Successful");
        }
    }
}
