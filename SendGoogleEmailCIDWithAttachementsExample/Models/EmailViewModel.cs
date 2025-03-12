using System.ComponentModel.DataAnnotations;

namespace SendGoogleEmailCIDWithAttachementsExample.Models
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "請輸入 SMTP 伺服器")]
        public string SmtpServer { get; set; }

        [Required(ErrorMessage = "請輸入 SMTP 連接埠")]
        [Range(1, 65535, ErrorMessage = "連接埠必須介於 1-65535 之間")]
        public int SmtpPort { get; set; }

        [Required(ErrorMessage = "請輸入寄件者信箱")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        public string SenderEmail { get; set; }

        [Required(ErrorMessage = "請輸入寄件者密碼")]
        public string SenderPassword { get; set; }

        [Required(ErrorMessage = "請輸入收件者信箱")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
        public string RecipientEmail { get; set; }

        [Required(ErrorMessage = "請輸入郵件主旨")]
        public string Subject { get; set; }
    }
}
