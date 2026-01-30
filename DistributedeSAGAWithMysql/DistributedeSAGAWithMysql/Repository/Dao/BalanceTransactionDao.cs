namespace DistributedeSAGAWithMysql.Repository.Dao
{
    public class BalanceTransactionDao
    {
        public long TxId { get; set; }
        public long MemberId { get; set; }

        /// <summary>
        /// 正數 = 扣款
        /// 負數 = 退款
        /// </summary>
        public decimal Amount { get; set; }

        public string SagaId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
