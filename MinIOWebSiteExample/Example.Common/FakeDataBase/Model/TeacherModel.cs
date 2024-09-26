namespace Example.Common.FakeDataBase.Model
{
    public class TeacherModel
    {
        public string Name { get; set;}
        
        public int Id { get; set;}

        public FileModel MySelfFiles { get; set; } = new FileModel();
    }
}
