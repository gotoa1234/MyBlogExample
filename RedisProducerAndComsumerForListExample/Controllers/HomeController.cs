using Microsoft.AspNetCore.Mvc;
using RedisProducerAndComsumerForListExample.Service;

namespace RedisProducerAndComsumerForListExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RedisQueueService _queueService;

        public HomeController(ILogger<HomeController> logger
            , RedisQueueService queueService)
        {
            _queueService = queueService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 2. ��{ List Redis �Ͳ��̱��e�T��
        /// </summary>
        [HttpPost]
        public IActionResult ProduceMessage([FromBody] MessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Message))
            {
                return BadRequest("Message is required.");
            }
            request.Message = request.Message + $@" => �ǰe��ƪ��ɶ� : {DateTime.Now}";

            _queueService.Enqueue("myQueue", request.Message);
            return Ok(new { message = "Message added to queue: " + request.Message });
        }

        public class MessageRequest
        {
            public string Message { get; set; }
        }
    }
}
