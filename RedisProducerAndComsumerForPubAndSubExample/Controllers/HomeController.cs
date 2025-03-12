using Microsoft.AspNetCore.Mvc;
using RedisProducerAndComsumerForPubAndSubExample.Service;

namespace RedisProducerAndComsumerForPubAndSubExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // 1. �q�\�Ͳ���
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
        /// 2. ��{ Pub/Sub �Ͳ��̱��e�T��
        /// </summary>
        [HttpPost]
        public IActionResult ProduceMessage([FromBody] MessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Message))
            {
                return BadRequest("Message is required.");
            }

            _pubSubService.Publish("myChannel", request.Message);
            return Ok(new { message = "Message sent: " + request.Message });
        }

        public class MessageRequest
        {
            public string Message { get; set; }
        }
    }
}
