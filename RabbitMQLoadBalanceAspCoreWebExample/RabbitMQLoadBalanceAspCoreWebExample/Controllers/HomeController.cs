using Microsoft.AspNetCore.Mvc;
using RabbitMQLoadBalanceAspCoreWebExample.Service;

namespace RabbitMQLoadBalanceAspCoreWebExample.Controllers
{
    public class HomeController : Controller
    {        
        private readonly IAccountTradeOrder _accountTradeOrder;

        public HomeController(IAccountTradeOrder accountTradeOrder)
        {            
            _accountTradeOrder = accountTradeOrder;
        }

        public IActionResult Index()
        {                        
            return View();
        }

        /// <summary>
        /// 建立假定單 API
        /// </summary>        
        [HttpGet]
        public IActionResult BuildAccountTradeOrder()
        {
            _accountTradeOrder.BuildAccountTradeOrder();
            var result = _accountTradeOrder.GetAccountTraderOrder();
            return PartialView("_AccountTradeOrderPartial", result);
        }    

        /// <summary>
        /// 查詢頁面
        /// </summary>        
        public IActionResult QueryPage()
        {
            var result =  _accountTradeOrder.GetAccountTraderOrder();
            return View(result);
        }

        /// <summary>
        /// 查詢頁面
        /// </summary>   
        [HttpGet]
        public IActionResult QueryPagePartial()
        {
            var result = _accountTradeOrder.GetAccountTraderOrder();
            return PartialView("_AccountTradeOrderPartial", result);
        }
    }
}
