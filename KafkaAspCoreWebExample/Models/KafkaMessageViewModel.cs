using System.ComponentModel.DataAnnotations;

namespace KafkaAspCoreWebExample.Models
{
    public class KafkaMessageViewModel
    {
        [Required(ErrorMessage = "主題為必填項")]
        [Display(Name = "主題")]
        public string Topic { get; set; } = string.Empty;

        [Display(Name = "鍵值 (可選)")]
        public string Key { get; set; } = string.Empty;

        [Required(ErrorMessage = "訊息內容為必填項")]
        [Display(Name = "訊息內容")]
        public string Message { get; set; } = string.Empty;

        public DateTime? ReceivedAt { get; set; }

        public long? Offset { get; set; }
    }
}
