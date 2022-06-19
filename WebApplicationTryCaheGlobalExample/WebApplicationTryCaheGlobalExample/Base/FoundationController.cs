using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Authentication;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WebApplicationTryCaheGlobalExample.Common;

namespace WebApplicationTryCaheGlobalExample.Base
{
    public class FoundationController : Controller
    {
        /// <summary>
        /// 驗證權限
        /// </summary>
        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
        }

        #region OnException 例外捕捉

        /// <summary>
        /// Step1： 當發生例外時觸發
        /// </summary>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (Request.IsAjaxRequest())
            {
                //前端Ajax請求，例外返回
                HandleAjaxException(filterContext);
            }
            else
            {
                //後端API請求，例外返回
                HandleException(filterContext);
            }
        }

        /// <summary>
        /// Step2-1：回傳Json時
        /// </summary>
        private void HandleAjaxException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            var filterContextMessage = CoustomerExceptionMessage(filterContext);
            filterContext.Result = new ContentResult
            {
                ContentType = "text/html",
                Content = $"{{\"Status\":200,\"Data\":\"{Server.HtmlEncode(filterContextMessage)}\"}}"
            };
        }

        /// <summary>
        /// Step2-2：回傳Api時
        /// </summary>
        private void HandleException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            var filterContextException = filterContext.Exception;
            var filterContextMessage = CoustomerExceptionMessage(filterContext);
            switch (filterContextException)
            {
                case MessageException _:
                case AuthenticationException _:

                    var query = (Request?.Url?.Query ?? "").Trim();

                    //var showMessage = string.IsNullOrEmpty(filterContextException.Message)
                    var showMessage = string.IsNullOrEmpty(filterContextMessage)
                        ? ""
                        : $"{(string.IsNullOrEmpty(query) ? "?" : "&")}showMessage={filterContextMessage}";
                    //: $"{(string.IsNullOrEmpty(query) ? "?" : "&")}showMessage={filterContextException.Message}";

                    filterContext.Result = new RedirectResult($"/Home/Index/{showMessage}");
                    break;
                default:
                    filterContext.Result = View($"~/Views/Home/Error.cshtml", filterContextException);
                    break;
            }
        }

        /// <summary>
        /// 【代碼擴增】客製化返回錯誤訊息
        /// </summary>
        /// <returns></returns>
        private string CoustomerExceptionMessage(ExceptionContext filterContext)
        {
            var resultCustomerMessage = RepalceErrorMessage.Instance.Value.GlobalReplaceErrorMessage(filterContext);
            if (string.IsNullOrEmpty(resultCustomerMessage))
            {
                return filterContext.Exception.Message;
            }
            return resultCustomerMessage;
        }

        #endregion

        /// <summary>
        /// Step3. API 執行時處發
        /// </summary>        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            CallContext.SetData("Ticks", DateTime.Now.Ticks);

            //寫入API接口紀錄
        }

        /// <summary>
        /// Step4. API 執行結束時觸發
        /// </summary>        
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            //寫入超時紀錄與Log4net.Error() 等等，工程師可以追錯StackErrorTrace
        }

    }

}