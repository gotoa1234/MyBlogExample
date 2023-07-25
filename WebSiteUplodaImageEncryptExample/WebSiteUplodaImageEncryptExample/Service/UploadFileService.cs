using WebSiteUplodaImageEncryptExample.Util;

namespace WebSiteUplodaImageEncryptExample.Service
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IWebHostEnvironment _environment;

        public UploadFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// 上傳圖片-明文
        /// </summary>
        /// <param name="imageFile"></param>
        public void UploadImage(IFormFile imageFile)
        {

            var path = Path.Combine(_environment.WebRootPath, "MyImage");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, "MyImage.png");
            using (var fileStream = File.Create(filePath))
            {
                imageFile.CopyTo(fileStream);
                fileStream.Flush();
            }
        }

        /// <summary>
        /// 上傳圖片-加密
        /// </summary>
        /// <param name="imageFile"></param>
        public void UploadImageEncrypt(IFormFile imageFile)
        {
            var path = $"{_environment.WebRootPath}\\MyImage";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, "MyEncryptImage.png");
            using (MemoryStream ms = new MemoryStream())
            {
                imageFile.CopyTo(ms);
                var imageData = ms.ToArray();

                // 將圖片資料進行加密
                var encryptedImageData = CryptoUtil.AesEncrypt(imageData);
                File.WriteAllBytes(filePath, encryptedImageData);
            }
        }

    }
}
