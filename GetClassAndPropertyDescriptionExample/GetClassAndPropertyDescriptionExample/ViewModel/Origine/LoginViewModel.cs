namespace GetClassAndPropertyDescriptionExample.ViewModel.Origine
{
    /// <summary>
    /// 原始登入的ViewModel
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登入密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否密碼驗證
        /// </summary>
        public bool HasPassword { get; set; }

        /// <summary>
        /// 用戶Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 個人資訊
        /// </summary>
        public int PersonalInformation { get; set; }
    }
}
