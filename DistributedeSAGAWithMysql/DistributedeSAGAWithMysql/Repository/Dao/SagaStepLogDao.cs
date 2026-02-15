namespace DistributedeSAGAWithMysql.Repository.Dao
{
    public class SagaStepLogDao
    {
        public long Id { get; set; }

        public string SagaId { get; set; } = string.Empty;

        /// <summary>
        /// CreateLog / DeductBalance / CreatePurchase / CompleteSaga
        /// </summary>
        public string StepName { get; set; } = string.Empty;

        /// <summary>
        /// SUCCESS / FAILED
        /// </summary>
        public string StepStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
