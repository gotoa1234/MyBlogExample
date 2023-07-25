using System.ComponentModel.DataAnnotations;

namespace WebSiteUplodaImageEncryptExample.Models
{
    public class UploadModel
    {
        /// <summary>
        /// 圖片
        /// </summary>
        [Required]
        public IFormFile Image { get; set; }
    }
}
