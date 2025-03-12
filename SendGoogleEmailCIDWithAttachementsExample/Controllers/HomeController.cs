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

        [HttpPost("Send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailDTO model)
        {
            try
            {
                await _sendEmailService.SendEmail();
                // �إ� SMTP �Ȥ��
                return Ok(new { success = true, message = "�l��o�e���\" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}

