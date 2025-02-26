using KaffkaAspCoreWebExample.Models;
using KaffkaAspCoreWebExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace KaffkaAspCoreWebExample.Controllers
{
    public class KafkaController : Controller
    {
        private readonly IKafkaProducerService _producerService;
        private readonly IKafkaConsumerService _consumerService;

        public KafkaController(IKafkaProducerService producerService, IKafkaConsumerService consumerService)
        {
            _producerService = producerService;
            _consumerService = consumerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Produce()
        {
            return View(new KafkaMessageViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Produce(KafkaMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _producerService.ProduceMessageAsync(model.Topic, model.Key, model.Message);
                if (result.Success)
                {
                    TempData["SuccessMessage"] = $"訊息發送成功！位置: {result.TopicPartitionOffset}";
                }
                else
                {
                    TempData["ErrorMessage"] = $"訊息發送失敗: {result.ErrorMessage}";
                }
                return RedirectToAction(nameof(Produce));
            }
            return View(model);
        }

        public IActionResult Consume()
        {
            var receivedMessages = _consumerService.GetReceivedMessages();
            return View(receivedMessages);
        }

    }
}
