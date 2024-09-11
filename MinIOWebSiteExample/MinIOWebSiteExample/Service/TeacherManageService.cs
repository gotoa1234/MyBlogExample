using Example.Common.FakeDataBase;

namespace MinIOWebSiteExample.Service
{
    /// <summary>
    /// 教師管理系統
    /// </summary>
    public class TeacherManageService : ITeacherManageService
    {
        private readonly FakeDataBase _dataBase;

        public TeacherManageService(FakeDataBase dataBase)
        {
            _dataBase = dataBase;
        }       
    }
}