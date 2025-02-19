using DistributedeXtendedArchitectureWithMysql.Model;
using DistributedeXtendedArchitectureWithMysql.Service;
using Microsoft.AspNetCore.Mvc;

namespace DistributedeXtendedArchitectureWithMysql.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBankTransferService _transferService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IBankTransferService bankTransferService)
        {
            _transferService = bankTransferService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// [使用] XA 的 API
        /// </summary>        
        [HttpGet]
        public async Task<ActionResult<TransferResult>> TransferWithXA()
        {
            var request = new TransferRequest()
            {
                Amount = 10000,
                FromAccount = "X",
                ToAccount = "Y",
            };

            var result = await _transferService.TransferWithXAAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// [不使用] XA 的 API 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<TransferResult>> TransferWithoutXA()
        {
            var request = new TransferRequest()
            {
                Amount = 10000,
                FromAccount = "X",
                ToAccount = "Y",
            };

            var result = await _transferService.TransferWithoutXAAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
      
    }
}
