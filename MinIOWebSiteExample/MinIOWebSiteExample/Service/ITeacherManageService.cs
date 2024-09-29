using Example.Common.FakeDataBase.Model;

namespace MinIOWebSiteExample.Service
{
    public interface ITeacherManageService
    {
        Task<List<TeacherModel>> GetTeachers();

        Task<MemoryStream> DownloadFile(string fileName, string bucketName);

        Task UploadFile(IFormFile file, string bucketName);

        Task DeleteFile(string fileName, string bucketName);

        Task<long> CreateAccount();

        Task DeleteAccount(long id);
    }
}
