using Example.Common.FakeDataBase;
using Example.Common.FakeDataBase.Model;
using Example.Common.MinIO;

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
            foreach (var item in result)
            {
                var bucketName = $@"{item.Id}";
                // 不存在就建立 Bucket
                if (!await _minIOClientInstance.IsExistBucket(bucketName))
                {
                    await _minIOClientInstance.CreateBucket(bucketName);
                }
                else// 存在就將檔案取回
                {
                    var getFiles = await _minIOClientInstance.GetBucketList(bucketName);
                    item.MySelfFiles = getFiles;
                }
            }
            return result;
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
            if (file == null ||
                file.Length == 0 ||
                string.IsNullOrEmpty(bucketName))
            {
                throw new Exception("參數異常");
            }
            await _minIOClientInstance.UploadFile(file, bucketName);
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>        
        public async Task DeleteFile(string fileName, string bucketName)
        {
            if (string.IsNullOrEmpty(fileName) ||
                string.IsNullOrEmpty(bucketName))
            {
                throw new Exception("參數異常");
            }
            await _minIOClientInstance.DeleteFile(fileName, bucketName);
        }

        /// <summary>
        /// 建立帳號
        /// </summary>
        public async Task<long> CreateAccount()
        {
            var createdId = _dataBase.CreateTeacher();
            var bucketName = $@"{createdId}";
            // 不存在就建立 Bucket
            if (!await _minIOClientInstance.IsExistBucket(bucketName))
            {
                await _minIOClientInstance.CreateBucket(bucketName);
            }
            return createdId;
        }

        /// <summary>
        /// 刪除帳號
        /// </summary>
        public async Task DeleteAccount(long id)
        {
            _dataBase.DeleteTeacher(id);

            var bucketName = $@"{id}";
            await _minIOClientInstance.DeleteBucket(bucketName);
        }

    }
}