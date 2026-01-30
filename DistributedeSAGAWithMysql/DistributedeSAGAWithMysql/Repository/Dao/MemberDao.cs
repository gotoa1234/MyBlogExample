namespace DistributedeSAGAWithMysql.Repository.Dao
{
    public class MemberDao
    {
        public long MemberId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
    }
}
