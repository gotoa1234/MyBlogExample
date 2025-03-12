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
        /// 1. [���ϥ�] XA �� API 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<TransferResult>> TransferWithoutXA()
        {
            // Step 1. �ǳƸ�ơA A�w �g�i B�w
            var request = new TransferRequest()
            {
                Amount = 10000,
                FromAccount = "A_Louis",
                ToAccount = "B_Louis",
            };

            // Step 2. �i���w�g�J (���ϥ�XA)
            var result = await _transferService.TransferWithoutXAAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// 2. [�ϥ�] XA �� API
        /// </summary>        
        [HttpGet]
        public async Task<ActionResult<TransferResult>> TransferWithXA()
        {
            // Step1. �ǳƸ�ơA A�w �g�i B�w
            var request = new TransferRequest()
            {
                Amount = 10000,
                FromAccount = "A_Louis",
                ToAccount = "B_Louis",
            };

            // Step 2. �i�����w�g�J (�ϥ�XA)
            var result = await _transferService.TransferWithXAAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
