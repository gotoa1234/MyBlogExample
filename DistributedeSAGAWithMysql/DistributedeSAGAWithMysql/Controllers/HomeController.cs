using DistributedeSAGAWithMysql.Models;
using DistributedeSAGAWithMysql.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DistributedeSAGAWithMysql.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShoppingCartService _shoppingCartService;

        public HomeController(ILogger<HomeController> logger,
            IShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> FullSAGA()
        {
            var resultData = GetMemberShoppingData();
            await _shoppingCartService.Shoppinng(resultData);
            return Ok("成功! 正常情況下，沒有任何異常的寫入 - SAGA (寫 3 種分散式庫都正常)");
           
        }

        [HttpGet]
        public async Task<ActionResult> Step1SAGA()
        {
            var resultData = GetMemberShoppingData();
            await _shoppingCartService.Shoppinng(resultData);
            return Ok("成功 Step1 - SAGA (寫 Log 庫)");
        }

        [HttpGet]
        public async Task<ActionResult> Step2SAGA()
        {
            return Ok("成功 Step2 - SAGA (寫 Balance 庫 - 扣款)");
        }

        [HttpGet]
        public async Task<ActionResult> Step3SAGA()
        {
            return Ok("成功 Step3 - SAGA (寫 Member 庫 - 更新用戶紀錄)");
        }

        [HttpGet]
        public async Task<ActionResult> Step4SAGA()
        {
            return Ok("成功 Step4 - SAGA (寫 Log 庫 - 完成 SAGA 模式最終一致性)");
        }

        /// <summary>
        /// 取得用戶購買資料 - 範例使用，固定同個用戶
        /// </summary>
        private RequestModel GetMemberShoppingData()
        {
            // 假資料 
            return new RequestModel()
            {
                MemberId = 1,
                ProductId = 1001,
                Count = 1
            };
        }
    }
}
