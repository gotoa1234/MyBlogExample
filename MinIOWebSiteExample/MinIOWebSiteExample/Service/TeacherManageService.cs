using Example.Common.FakeDataBase;
using Example.Common.FakeDataBase.Model;
using Example.Common.MinIO;
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

        public async Task<List<TeacherModel>> GetTeachers()
        {
            var result = new List<TeacherModel>();
            result = await Task.Run(() =>
            {
                return _dataBase.GetTeachers();
            });
            await _minIOClientInstance.GetBucketList("my-bucket");
            return result;
        }
    }
}