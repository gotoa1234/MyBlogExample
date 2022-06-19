using System.Web.Mvc;
using WebApplicationTryCaheGlobalExample.Base;
using WebApplicationTryCaheGlobalExample.Service;

namespace WebApplicationTryCaheGlobalExample.Controllers
{
    public class HomeController : FoundationController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestErrorFirst()
        {
            ViewBag.Message = "測試第一個錯誤包覆";
            TestErrorFirstService.Instance.Value.TestErrorFirst_MethodA();
            //TestErrorFirstService.Instance.Value.TestErrorFirst_MethodB();
            return View();
        }
        
        public ActionResult TestErrorSecond()
        {
            ViewBag.Message = "測試第二個錯誤包覆";
            TestErrorSecondService.Instance.Value.TestErrorSecond_MethodC();
            //TestErrorSecondService.Instance.Value.TestErrorSecond_MethodD();
            return View();
        }
    }
}