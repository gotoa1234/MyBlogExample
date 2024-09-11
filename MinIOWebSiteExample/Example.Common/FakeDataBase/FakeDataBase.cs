using Example.Common.FakeDataBase.Model;

namespace Example.Common.FakeDataBase
{
    public class FakeDataBase
    {
        private List<TeacherModel> _teachers = new List<TeacherModel>();

        public FakeDataBase()
        {
            _teachers.Add(new TeacherModel() { Id = 1, Name = "張明宇" });
            _teachers.Add(new TeacherModel() { Id = 2, Name = "李曉峰" });
            _teachers.Add(new TeacherModel() { Id = 3, Name = "陳佳玲" });
        }

        public List<TeacherModel> GetTeachers()
        {
            return _teachers;
        }
    }
}
