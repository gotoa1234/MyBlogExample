namespace DistributedeSAGAWithMysql.Repository.Dao
{
    public class SagaTransactionDao
    {
        public string SagaId { get; set; } = string.Empty;

        public long MemberId { get; set; }
        public long ProductId { get; set; }
        public decimal Amount { get; set; }

        /// <summary>
        /// PENDING / COMPLETED / FAILED
        /// </summary>
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
