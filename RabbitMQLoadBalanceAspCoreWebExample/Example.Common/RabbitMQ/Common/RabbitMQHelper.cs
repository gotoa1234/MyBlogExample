using Example.Common.RabbitMQ.Model;
using RabbitMQ.Client;

namespace Example.Common.RabbitMQ.Common
{
    public static class RabbitMQHelper
    {
        public static ConnectionFactory GetConstructFactory(ExchangeModel parameters)
        {
            var factory = new ConnectionFactory
            {
                HostName = parameters.HostName,
                UserName = parameters.UserName,
                Password = parameters.Password,
                // 心跳機制，30秒內視為連線有效
                RequestedHeartbeat = TimeSpan.FromSeconds(30),
                // 啟用自動重連
                AutomaticRecoveryEnabled = true,
                // 啟用拓撲恢復 (Exchange、Queue、Binding)
                TopologyRecoveryEnabled = true,
                // 拓撲重連間隔時間，拓撲 (Exchange、Queue、Binding) 恢復間隔時間
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                // 握手過程的超時時間，首次連線時的超時時間
                HandshakeContinuationTimeout = TimeSpan.FromSeconds(60)
            };
            return factory;
        }

        public static IConnection GetConnection(ExchangeModel parameters)
        {
            var factory = GetConstructFactory(parameters);
            var connection = factory.CreateConnection();
            return connection;
        }

        public static IModel GetModel(IConnection connection)
        {
            var channel = connection.CreateModel();
            return channel;
        }
    }
}
