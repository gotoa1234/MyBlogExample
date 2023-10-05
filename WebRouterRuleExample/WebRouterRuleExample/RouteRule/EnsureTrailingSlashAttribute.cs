using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WebRouterRuleExample.RouteRule
{
    public class EnsureTrailingSlashAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            // 1. 檢查邏輯 - URL 是否以斜槓結尾
            var isLegal = IsLegalUrlPath(request.Path.Value ?? string.Empty);
            if (false == isLegal)
            {
                // 2. 組成正確的Url 回傳Location給對方知道
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
                var pathFullUrl = string.Join('/', addPathList) + queryString;
                var rightFullUrl = (context.HttpContext.Request.IsHttps ? "https://" : "http://")
                                   + context.HttpContext.Request.Host.ToString() 
                                   + pathFullUrl;

                // 3-1. 創建一個 RedirectResult 實現重定向301
                var result = new RedirectResult(WebUtility.HtmlEncode(rightFullUrl), true);
                // 3-2. 設定 HTTP 狀態碼為 301
                context.HttpContext.Response.StatusCode = 301;

                // 4. 設定 Location 標頭以指定重定向目標(就是我們建議的正確位置)
                context.HttpContext.Response.Headers.Add("Location", WebUtility.HtmlEncode(rightFullUrl));

                // 5. 將結果設定到結果中
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
