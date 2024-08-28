using Microsoft.AspNetCore.Mvc;
using RabbitMQLoadBalanceAspCoreWebExample.Service;

namespace RabbitMQLoadBalanceAspCoreWebExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountTradeOrder _accountTradeOrder;

        public HomeController(ILogger<HomeController> logger,
            IAccountTradeOrder accountTradeOrder)
        {
            _logger = logger;
            _accountTradeOrder = accountTradeOrder;
        }

        public IActionResult Index()
        {                        
            return View();
        }

        /// <summary>
        /// �إ߰��w�� API
        /// </summary>        
        [HttpGet]
        public IActionResult BuildAccountTradeOrder()
        {
            _accountTradeOrder.BuildAccountTradeOrder();
            var result = _accountTradeOrder.GetAccountTraderOrder();
            return PartialView("_AccountTradeOrderPartial", result);
        }    

        /// <summary>
        /// �d�߭���
        /// </summary>        
        public IActionResult QueryPage()
        {
            var result =  _accountTradeOrder.GetAccountTraderOrder();
            return View(result);
        }

        /// <summary>
        /// �d�߭���
        /// </summary>   
        [HttpGet]
        public IActionResult QueryPagePartial()
        {
            var result = _accountTradeOrder.GetAccountTraderOrder();
            return PartialView("_AccountTradeOrderPartial", result);
        }
    }
}
