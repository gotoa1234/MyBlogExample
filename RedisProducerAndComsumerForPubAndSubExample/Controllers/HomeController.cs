using Microsoft.AspNetCore.Mvc;
using RedisProducerAndComsumerForPubAndSubExample.Service;

namespace RedisProducerAndComsumerForPubAndSubExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // 1. 訂閱生產者
        private readonly RedisPubSubService _pubSubService;

        public HomeController(ILogger<HomeController> logger,
            RedisPubSubService pubSubService)
        {
            _logger = logger;
            _pubSubService = pubSubService;
        }

        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 2. 實現生產者推送訊息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ProduceMessage(string message)
        {
            _pubSubService.Publish("myChannel", message);
            return Ok("Message sent: " + message);
        }
    }
}
