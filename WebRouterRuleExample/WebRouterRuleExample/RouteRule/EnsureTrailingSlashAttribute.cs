using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace WebRouterRuleExample.RouteRule
{
    public class EnsureTrailingSlashAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            // 檢查 URL 是否以斜槓結尾
            var isLegal = IsLegalUrlPath(request.Path.Value ?? string.Empty);
            if (false == isLegal)
            {
                // 組成正確的Url 給用戶知道
                var path = context.HttpContext.Request.Path;
                var pathSplit = request.Path.Value.Split('/');                
                var addPathList = new List<string>() { ""};
                
                for (int index = 1; index < pathSplit.Length; index++)
                {
                    if (string.IsNullOrEmpty(pathSplit[index]))
                        continue;
                    addPathList.Add(pathSplit[index]);
                }
                var queryString = context.HttpContext.Request.QueryString;                
                var rightFullUrl = string.Join('/', addPathList) + queryString;

                // 創建一個 RedirectToActionResult 以執行重定向
                var result = new RedirectResult(WebUtility.HtmlEncode(rightFullUrl), true);

                // 設定 HTTP 狀態碼為 301 Found
                context.HttpContext.Response.StatusCode = 301;

                // 設定 Location 標頭以指定重定向目標(就是我們建議的正確位置)
                context.HttpContext.Response.Headers.Add("Location", WebUtility.HtmlEncode(rightFullUrl));

                // 將結果設定到 ActionExecutingContext 中
                context.Result = result;

                base.OnActionExecuting(context);
            }
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 檢核是否為合法的Url
        /// </summary>                
        private static bool IsLegalUrlPath(string url)
        {            
            var urlItem = url.Split('/');
            if (!string.IsNullOrEmpty(urlItem[0]))
            {
                return false;
            }
            for (int index = 1; index < urlItem.Length; index++)
            {
                if (string.IsNullOrEmpty(urlItem[index]))
                    return false;
            }
            return true;
        }
    }
}
