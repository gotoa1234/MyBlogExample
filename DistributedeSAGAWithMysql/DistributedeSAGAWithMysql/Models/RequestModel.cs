namespace DistributedeSAGAWithMysql.Models
{
    public class RequestModel
    {
        /// <summary>
        /// 會員 Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 商品 Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 購買數量
        /// </summary>
        public int Count { get; set; }
    }
}
