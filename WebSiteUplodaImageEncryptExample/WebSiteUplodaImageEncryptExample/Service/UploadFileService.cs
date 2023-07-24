namespace WebSiteUplodaImageEncryptExample.Service
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IWebHostEnvironment _environment;

        public UploadFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void UploadImage(IFormFile imageFile)
        {
            var path = $"{_environment.ContentRootPath}\\MyImage";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, imageFile.FileName);
            using (var fileStream = File.Create(filePath))
            {
                imageFile.CopyTo(fileStream);
                fileStream.Flush();
            }
        }

    }
}
