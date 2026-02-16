namespace DistributedeSAGAWithMysql.Repository.Dto
{
    public class CompensatoryDto
    {
        public List<string> notDeductionSagaIds = new List<string>();
        public List<string> notPurchaseSagaIds = new List<string>();
        public List<string> notMarkComplateSagaIds = new List<string>();
    }
}
