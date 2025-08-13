namespace reCAPTCHATbyGoogleExample.Models.ResponseModel
{
    public class ReCaptchaResponseV3
    {
        public bool success { get; set; }
        public float score { get; set; }
        public string action { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}