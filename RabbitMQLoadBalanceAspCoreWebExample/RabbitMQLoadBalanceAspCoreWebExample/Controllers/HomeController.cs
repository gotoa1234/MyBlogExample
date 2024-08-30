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
        /// 1. �d�߭���
        /// </summary>        
        public IActionResult QueryPage()
        {
            var result = _accountTradeOrder.GetAccountTraderOrder();
            return View(result);
        }

        /// <summary>
        /// 2. �إ߰��q�� API
        /// </summary>        
        [HttpGet]
        public IActionResult BuildAccountTradeOrder()
        {
            _accountTradeOrder.BuildAccountTradeOrder();
            var result = _accountTradeOrder.GetAccountTraderOrder();
            return PartialView("_AccountTradeOrderPartial", result);
        }

        /// <summary>
        /// 3. �d�߭���(��sTable)
        /// </summary>   
        [HttpGet]
        public IActionResult QueryPagePartial()
        {
            var result = _accountTradeOrder.GetAccountTraderOrder();
            return PartialView("_AccountTradeOrderPartial", result);
        }
    }
}
