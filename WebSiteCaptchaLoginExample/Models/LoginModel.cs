namespace WebSiteCaptchaLoginExample.Models
{
    public class LoginModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 圖形驗證碼
        /// </summary>
        public string InputCaptcha { get; set; } = string.Empty;
    }
}
