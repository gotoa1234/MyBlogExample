using Microsoft.AspNetCore.Mvc;
using SendGoogleEmailCIDWithAttachementsExample.Models;
using SendGoogleEmailCIDWithAttachementsExample.Service;

namespace SendGoogleEmailCIDWithAttachementsExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISendEmailService _sendEmailService;
        public HomeController(ILogger<HomeController> logger,
            ISendEmailService sendEmailService)
        {
            _logger = logger;
            _sendEmailService = sendEmailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailDTO model)
        {
            try
            {
                var result = await _sendEmailService.SendEmail(model);
                return Ok(
                    new { success = true, message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

