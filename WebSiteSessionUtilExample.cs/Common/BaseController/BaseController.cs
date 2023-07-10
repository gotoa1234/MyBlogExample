using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebSiteSessionUtilExample.cs.Common.Util;

namespace WebSiteSessionUtilExample.cs.Common.BaseController
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 1. 當觸發任何Controller執行前的動作
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //2. 呼叫SessionUtil.Configure 將當前的HttpContext存取者，記錄，使後續的Controller生命週期中都可以呼叫使用
            SessionUtil.Configure(HttpContext?.RequestServices?.GetService<IHttpContextAccessor>());
            base.OnActionExecuting(context);
        }
    }
}
