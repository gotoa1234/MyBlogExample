using System.Web.Mvc;

namespace IsAjaxRequestExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AjaxCall()
        {
            ViewBag.IsAjax = Request.IsAjaxRequest();
            var result = new
            {
                IsAjax = Request.IsAjaxRequest()
            };
            return Json(result);
        }

    }
}