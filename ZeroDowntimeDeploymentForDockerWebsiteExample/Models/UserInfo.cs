namespace ZeroDowntimeDeploymentForDockerWebsiteExample.Models
{
    /// <summary>
    /// 用戶資訊 Model
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用戶Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 權限名稱
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
