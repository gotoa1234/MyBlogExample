namespace Example.Common.RabbitMQ.Factory
{
    public interface IRabbitMqFactory
    {
        RabbitMqMessagePublisher Get(string mqExchangeName, string exchangeType = "Direct");
    }
}
