namespace DistributedeSAGAWithMysql.Repository.Dao
{
    public class PurchaseDao
    {
        public long PurchaseId { get; set; }
        public string SagaId { get; set; } = string.Empty;

        public long MemberId { get; set; }
        public long ProductId { get; set; }
        public decimal Amount { get; set; }

        /// <summary>
        /// SUCCESS / FAILED
        /// </summary>
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
