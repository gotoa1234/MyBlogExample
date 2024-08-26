namespace Example.Common.RabbitMQ.Model
{
    public class ExchangeModel : RabbitMQConnectionModel
    {
        /// <summary>
        /// 交換機模式
        /// </summary>
        public string ExchangeType { get; set; } = string.Empty;

        /// <summary>
        /// 交換機名稱
        /// </summary>
        public string ExchangeName { get; set; } = string.Empty;
    }
}
