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
            _accountTradeOrder.BuildAccountTradeOrder();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
