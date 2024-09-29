using Bogus;
using Example.Common.FakeDataBase.Model;

namespace Example.Common.FakeDataBase
{
    public class FakeDataBase
    {
        private List<TeacherModel> _teachers = new List<TeacherModel>();

        /// <summary>
        /// 預設假資料
        /// </summary>
        public FakeDataBase()
        {
            _teachers.Add(new TeacherModel() { Id = 20240928001, Name = "張明宇" });
            _teachers.Add(new TeacherModel() { Id = 20240928002, Name = "李曉峰" });
            _teachers.Add(new TeacherModel() { Id = 20240928003, Name = "陳佳玲" });
        }

        /// <summary>
        /// 取得教師資料
        /// </summary>
        public List<TeacherModel> GetTeachers()
        {
            return _teachers;
        }

        /// <summary>
        /// 新增假資料庫的帳號 (名字隨機)
        /// </summary>
        public long CreateTeacher()
        {
            var fakeAutoIncretmentId = this._teachers.Max(item => item.Id) + 1;
            var faker = new Faker("zh_CN");
            var now = DateTime.Now;
            _teachers.Add(new TeacherModel()
            {
                Id = fakeAutoIncretmentId,
                Name = faker.Name.FullName()
            });
            return fakeAutoIncretmentId;
        }

        /// <summary>
        /// 移除假資料庫的帳號
        /// </summary>        
        public void DeleteTeacher(long id)
        {
            var getItem = _teachers.Where(item => item.Id == id).FirstOrDefault();
            if (getItem != null)
            {
                _teachers.Remove(getItem);
            }
        }
    }
}
