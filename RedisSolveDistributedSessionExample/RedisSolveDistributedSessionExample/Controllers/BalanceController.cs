using Microsoft.AspNetCore.Mvc;
using RedisSolveDistributedSessionExample.Service.IService;

namespace RedisSolveDistributedSessionExample.Controllers
{
    public class BalanceController : Controller
    {
        private readonly ILogger<BalanceController> _logger;
        private readonly IUserBalanceService _userBalance;
        public BalanceController(ILogger<BalanceController> logger,
            IUserBalanceService userBalance)
        {
            _logger = logger;
            _userBalance = userBalance;
        }


        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 1. 更新用戶金額
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> UpdateBalance(decimal amount)
        {
            await _userBalance.UpdateBalance(1, amount);
            return Ok();
        }

        /// <summary>
        /// 2. 取得用戶金額
        /// </summary>        
        [HttpGet]
        public async Task<IActionResult> GetBalance(decimal amount)
        {
            var currentAmount = await _userBalance.GetBalance(1, amount);
            return Ok(currentAmount);
        }
    }
}
