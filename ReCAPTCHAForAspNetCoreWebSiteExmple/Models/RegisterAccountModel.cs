namespace ReCAPTCHAForAspNetCoreWebSiteExmple.Models
{
    public class RegisterAccountModel
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 驗證密碼
        /// </summary>
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
