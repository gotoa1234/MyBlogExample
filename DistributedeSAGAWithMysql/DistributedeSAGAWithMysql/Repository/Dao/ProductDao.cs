namespace DistributedeSAGAWithMysql.Repository.Dao
{
    public class ProductDao
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockCount { get; set; }
    }
}
