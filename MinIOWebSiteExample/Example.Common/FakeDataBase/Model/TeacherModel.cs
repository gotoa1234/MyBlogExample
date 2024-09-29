namespace Example.Common.FakeDataBase.Model
{
    public class TeacherModel
    {
        public string Name { get; set; } = string.Empty;

        public long Id { get; set; }

        public FileModel MySelfFiles { get; set; } = new FileModel();
    }
}