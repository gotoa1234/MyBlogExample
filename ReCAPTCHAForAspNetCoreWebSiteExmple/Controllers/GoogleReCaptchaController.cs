using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.AspNetCore.Mvc;

namespace ReCAPTCHAForAspNetCoreWebSiteExmple.Controllers
{
    public class GoogleReCaptchaController : Controller
    {
        private readonly RecaptchaEnterpriseServiceClient _recaptchaService;
        private readonly IConfiguration _configuration;
        public GoogleReCaptchaController(IConfiguration configuration)
        {
            var projectId = configuration["RecaptchaEnterprise:ProjectId"];
            var privateKey = configuration["RecaptchaEnterprise:PrivateKey"];
            _recaptchaService = RecaptchaEnterpriseServiceClient.Create();
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> YourAction([FromBody] dynamic data)
        {
            // Your action logic goes here

            // Perform reCAPTCHA verification
            var recaptchaSiteKey = _configuration["RecaptchaEnterprise:SiteKey"];

            var recaptcha = await _recaptchaService.AnnotateAssessmentAsync(new AnnotateAssessmentRequest
            {
                Name = recaptchaSiteKey, // Use your site key here
                Annotation = AnnotateAssessmentRequest.Types.Annotation.PasswordCorrect// "human" indicates a valid user
            });

            if (recaptcha.ToString() != AnnotateAssessmentRequest.Types.Annotation.PasswordCorrect.ToString())
            {
                // Handle invalid reCAPTCHA (e.g., return an error response)
                return StatusCode(403, "reCAPTCHA validation failed");
            }

            // Continue with your logic

            return Ok("Success");
        }
    }
}
