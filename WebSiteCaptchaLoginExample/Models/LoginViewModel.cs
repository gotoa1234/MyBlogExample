namespace WebSiteCaptchaLoginExample.Models
{
    public class LoginViewModel
    {
        public string Chapcha { get; set; } = string.Empty;

        public LoginModel SubmitData { get; set; } = new LoginModel();

    }
}
