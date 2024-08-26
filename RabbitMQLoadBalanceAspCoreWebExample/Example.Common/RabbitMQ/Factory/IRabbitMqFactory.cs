namespace Example.Common.RabbitMQ.Factory
{
    public interface IRabbitMqFactory
    {
        MqSender Get(string mqExchangeName, string exchangeType = "Direct");
    }
}
