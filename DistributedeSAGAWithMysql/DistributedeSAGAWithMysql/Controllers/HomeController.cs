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

        /// <summary>
        /// 完整走完
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> FullSAGA()
        {
            var resultData = GetMemberShoppingData();
            await _shoppingCartService.Shoppinng(resultData);
            return Ok("成功! 正常情況下，沒有任何異常的寫入 - SAGA (寫 3 種分散式庫都正常)");
           
        }

        /// <summary>
        /// 第一步執行前中斷 (未執行)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> Step1SAGA()
        {
            var resultData = GetMemberShoppingData();
            await _shoppingCartService.Shoppinng(resultData, Enum.InterruptStepEnum.InterruptStep1);
            return Ok();
        }

        /// <summary>
        /// 第二步執行前中斷 (寫入 Log 結束)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> Step2SAGA()
        {
            var resultData = GetMemberShoppingData();
            await _shoppingCartService.Shoppinng(resultData, Enum.InterruptStepEnum.InterruptStep2);
            return Ok();
        }

        /// <summary>
        /// 第三步執行前中斷 (寫入 Log + 扣款 Balance 表 結束)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> Step3SAGA()
        {
            var resultData = GetMemberShoppingData();
            await _shoppingCartService.Shoppinng(resultData, Enum.InterruptStepEnum.InterruptStep3);
            return Ok();
        }

        /// <summary>
        /// 第四步執行前中斷 (寫入 Log + 扣款 Balance 表 + 本地 Member 表 結束)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> Step4SAGA()
        {
            var resultData = GetMemberShoppingData();
            await _shoppingCartService.Shoppinng(resultData, Enum.InterruptStepEnum.InterruptStep4);
            return Ok();
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
