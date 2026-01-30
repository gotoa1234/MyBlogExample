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
        public async Task<ActionResult> Step1SAGA()
        {
            var resultData = GetMemberShoppingData();
            await _shoppingCartService.Shoppinng(resultData);
            return Ok("成功 Step1SAGA");
        }

        [HttpGet]
        public async Task<ActionResult> Step2SAGA()
        {
            return Ok("成功 Step2SAGA");
        }

        [HttpGet]
        public async Task<ActionResult> Step3SAGA()
        {
            return Ok("成功 Step3SAGA");
        }

        [HttpGet]
        public async Task<ActionResult> Step4SAGA()
        {
            return Ok("成功 Step4SAGA");
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
