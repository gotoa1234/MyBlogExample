using Example.Common.FakeDataBase;
using Example.Common.FakeDataBase.Model;
using Example.Common.MinIO;
using Microsoft.AspNetCore.Http.HttpResults;
using Minio;

namespace MinIOWebSiteExample.Service
{
    /// <summary>
    /// 教師管理系統
    /// </summary>
    public class TeacherManageService : ITeacherManageService
    {
        private readonly FakeDataBase _dataBase;
        private readonly MinIOClientInstance _minIOClientInstance;

        public TeacherManageService(FakeDataBase dataBase,
            MinIOClientInstance minIOClientInstance)
        {
            _dataBase = dataBase;
            _minIOClientInstance = minIOClientInstance;            
        }

        /// <summary>
        /// 取得所有教師資料
        /// </summary>       
        public async Task<List<TeacherModel>> GetTeachers()
        {
            var result = new List<TeacherModel>();
            result = await Task.Run(() =>
            {
                return _dataBase.GetTeachers();
            });
            await _minIOClientInstance.GetBucketList("my-bucket");
            await _minIOClientInstance.CreateBucket("my-bucket");
            //await _minIOClientInstance.CreateBucket("ttt");
       
            return result;
        }

        /// <summary>
        /// 建立帳號
        /// </summary>
        /// <returns></returns>
        public async Task CreateAccount()
        {            
        }

        /// <summary>
        /// 刪除帳號
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAccount()
        {
            await _minIOClientInstance.DeleteBucket("ttt");
        }

        /// <summary>
        /// 下載檔案
        /// </summary>        
        public async Task<MemoryStream> DownloadFile(string fileName, string bucketName)
        {        
            return await _minIOClientInstance.GetObjectAsync(fileName, bucketName);            
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>        
        public async Task UploadFile(IFormFile file, string bucketName)
        {
            if (file == null || file.Length == 0 || string.IsNullOrEmpty(bucketName))
            {
                throw new Exception("No file uploaded.");
            }
            await _minIOClientInstance.UploadFile(file, bucketName);
        }
        

    }
}