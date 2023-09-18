using CustomerDeviceProxyModeSwitchingExample.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomerDeviceProxyModeSwitchingExample.Controllers
{
    /// <summary>
    /// 建立一個基底的控制器
    /// </summary>
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. 設定HttpReques、Session
            RequestUtil.Configure(HttpContext?.RequestServices?.GetService<IHttpContextAccessor>());
            SessionUtil.Configure(HttpContext?.RequestServices?.GetService<IHttpContextAccessor>());
            base.OnActionExecuting(context);
        }
    }
}
