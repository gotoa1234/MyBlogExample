using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace WebApplicationTryCaheGlobalExample.Base
{
    public class CustomExceptionAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        /// <summary>
        /// OnAuthentication
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var controllerName = filterContext.ActionDescriptor.ActionName;
            throw new MessageException("操作次數過多，請停頓久點再點擊");

        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}