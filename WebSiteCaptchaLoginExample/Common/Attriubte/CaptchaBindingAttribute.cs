using Microsoft.AspNetCore.Mvc.Filters;
using WebSiteCaptchaLoginExample.Common.Util;

namespace WebSiteCaptchaLoginExample.Common.Attriubte
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CaptchaBindingAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// 圖形驗證碼綁定名稱
        /// </summary>
        public string CaptchaBindingName { get; set; } = string.Empty;

        public bool Generate { get; set; } = false;

        //1. 有綁定 [CaptchaBinding] 的Action才會觸發
        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                //2. 配置當前"設定名稱"
                var attribute = context.ActionDescriptor.EndpointMetadata.OfType<CaptchaBindingAttribute>().FirstOrDefault();
                if (attribute != null)
                {
                    SessionUtil.CaptChaBindName = attribute.CaptchaBindingName;
                    //3. 配置時為產生 Generate = true 才會生成字符串
                    if (Generate)
                    {
                        SessionUtil.CaptCha = CaptchaUtil.GetRandomCaptcha(5);
                    }
                }
            }
            catch
            {
                //直接拋棄，不影響正式流程
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
