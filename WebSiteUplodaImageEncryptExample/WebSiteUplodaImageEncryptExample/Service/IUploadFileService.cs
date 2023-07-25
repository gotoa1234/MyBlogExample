namespace WebSiteUplodaImageEncryptExample.Service
{
    public interface IUploadFileService
    {
        public void UploadImage(IFormFile imageFile);

        public void UploadImageEncrypt(IFormFile imageFile);
    }
}
