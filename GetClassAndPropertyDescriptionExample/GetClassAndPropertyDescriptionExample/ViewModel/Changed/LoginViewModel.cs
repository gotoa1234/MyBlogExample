namespace GetClassAndPropertyDescriptionExample.ViewModel.Changed
{
    /// <summary>
    /// 登入的ViewModel
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 原名稱：UserName  使用者名稱
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 原名稱：Password  登入密碼
        /// </summary>
        public string ValidateString { get; set; }

        /// <summary>
        /// 原名稱：HasPassword  是否密碼驗證
        /// </summary>
        public bool IsValidate { get; set; }

        /// <summary>
        /// 原名稱：UserId  用戶Id
        /// </summary>
        public int SequenceId { get; set; }

        /// <summary>
        /// 原名稱：PersonalInformation  個人資訊
        /// </summary>
        public int Detail { get; set; }
    }
}
