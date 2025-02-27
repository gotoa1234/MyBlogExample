using KafkaAspCoreWebExample.Models;
using KafkaAspCoreWebExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace KafkaAspCoreWebExample.Controllers
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

        /// <summary>
        /// Partial View 頁面 - 生產者的資訊 (發送資料)
        /// </summary>        
        public IActionResult Produce()
        {
            return View(new KafkaMessageViewModel());
        }

        /// <summary>
        /// 生產者 - 發送資料到 Kafka 
        /// </summary>        
        [HttpPost]
        public async Task<IActionResult> Produce(KafkaMessageViewModel model)
        {
            // 1. 用 C# 特性，自動驗證 Model 上的欄位
            if (ModelState.IsValid)
            {
                //2. 將資料傳送到 Kafka  上 - 關鍵是 Topic : 主題 ; 鍵值 : Key  才能正確傳送到對象 
                var result = await _producerService.ProduceMessageAsync(model.Topic, model.Key, model.Message);

                //3. 返回回傳結果
                if (result.IsSuccess)
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

        /// <summary>
        /// Partial View 頁面 - 消費者的資訊 (當前收到的資料)
        /// </summary>   
        public IActionResult Consume()
        {
            // 1. 取得此 AP 上發送過的 Kafka 資料 (這篇簡單示範，還有其他持久化消費對列的方式)
            var receivedMessages = _consumerService.GetReceivedMessages();
            return View(receivedMessages);
        }

    }
}
