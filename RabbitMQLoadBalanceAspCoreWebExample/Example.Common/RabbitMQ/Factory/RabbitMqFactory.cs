using Example.Common.RabbitMQ.Model;
using Microsoft.Extensions.Configuration;

namespace Example.Common.RabbitMQ.Factory
{
    public class RabbitMqFactory : IRabbitMqFactory
    {
        private readonly IConfiguration _configuration;
        private readonly string _rabbitMqHostName;
        private readonly string _rabbitMqUserName;
        private readonly string _rabbitMqPassword;

        public RabbitMqFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            //var rabbitParam = _configuration.GetSection("RabbitMQ").Get<BaseModel>();
            //_rabbitMqHostName = rabbitParam.HostName;
            //_rabbitMqUserName = rabbitParam.UserName;
            //_rabbitMqPassword = rabbitParam.Password;
        }

        private static readonly object _lockObj = new();
        private static readonly Dictionary<string, object> _lockObjDict = new();
        private static readonly Dictionary<string, MqSender> _senderDict = new();
        public MqSender Get(string mqExchangeName, string exchangeType = "Direct")
        {
            var key = $"{_rabbitMqHostName}_{mqExchangeName}";
            var sender = GetSender(key, mqExchangeName, exchangeType);
            return sender;
        }

        private MqSender GetSender(string key, string mqExchangeName, string exchangeType)
        {
            if (!_senderDict.TryGetValue(key, out var sender) || !sender.IsOpen)
            {
                var lockObj = GetLockObj(key);
                lock (lockObj)
                {
                    if (!_senderDict.TryGetValue(key, out sender) || !sender.IsOpen)
                    {
                        sender?.Dispose();

                        //sender = new MqSender(new BaseModel
                        //{
                        //    HostName = _rabbitMqHostName,
                        //    UserName = _rabbitMqUserName,
                        //    Password = _rabbitMqPassword,
                        //    ExchangeName = mqExchangeName,
                        //    ExchangeType = exchangeType
                        //});
                        _senderDict[key] = sender;
                    }
                }
            }

            return sender;
        }
        private object GetLockObj(string key)
        {
            if (!_lockObjDict.TryGetValue(key, out var obj) || obj == null!)
            {
                lock (_lockObj)
                {
                    if (!_lockObjDict.TryGetValue(key, out obj) || obj == null!)
                    {
                        obj = new object();
                        _lockObjDict[key] = obj;
                    }
                }
            }
            return obj;
        }
    }
}
