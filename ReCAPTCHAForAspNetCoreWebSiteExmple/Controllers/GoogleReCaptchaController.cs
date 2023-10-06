using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.AspNetCore.Mvc;
using ReCAPTCHAForAspNetCoreWebSiteExmple.Models;

namespace ReCAPTCHAForAspNetCoreWebSiteExmple.Controllers
{
    public class GoogleReCaptchaController : Controller
    {
        private readonly RecaptchaEnterpriseServiceClient _recaptchaService;
        private readonly IConfiguration _configuration;
        private readonly string _projectId;
        private readonly string _privateKey;

        public GoogleReCaptchaController(IConfiguration configuration)
        {
            _projectId = configuration["RecaptchaEnterprise:ProjectId"];
            _privateKey = configuration["RecaptchaEnterprise:PrivateKey"];
            _recaptchaService = RecaptchaEnterpriseServiceClient.Create();
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAccount(RegisterAccountModel data)
        {
            try
            {
                var recaptcha = await _recaptchaService.AnnotateAssessmentAsync(new AnnotateAssessmentRequest
                {
                    Name = _projectId, // Use your site key here
                });

                if (recaptcha.ToString() != "")// AnnotateAssessmentResponse.Types.Annotation.PasswordCorrect)
                {
                    // Handle invalid reCAPTCHA (e.g., return an error response)
                    return StatusCode(403, "reCAPTCHA validation failed");
                }

                // Continue with your logic

                return Ok("Success");
            }
            catch (Exception ex)
            {
                // Log the exception
                var my = ex;

                // Handle the error and return an appropriate response
                return StatusCode(500, "An error occurred during reCAPTCHA validation + EX: " + ex);
            }
        }

    }
}
