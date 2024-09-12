using Example.Common.FakeDataBase.Model;

namespace MinIOWebSiteExample.Service
{
    public interface ITeacherManageService
    {
        Task<List<TeacherModel>> GetTeachers();
    }
}
