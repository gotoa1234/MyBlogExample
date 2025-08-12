namespace reCAPTCHATbyGoogleExample.Models.ResponseModel
{
    // 匹配 google 的 recaptcha api 的 Response Json
    public class ReCaptchaResponse
    {
        public bool success { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
        public string[] error_codes { get; set; }
    }
}
