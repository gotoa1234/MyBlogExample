using Example.Common.RabbitMQ.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Example.Common.RabbitMQ.Factory
{
    /// <summary>
    /// MQ 連線工廠
    /// </summary>
    public class RabbitMqFactory : IRabbitMqFactory, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly string _rabbitMqHostName;
        private readonly string _rabbitMqUserName;
        private readonly string _rabbitMqPassword;

        /// <summary>
        /// 建構式
        /// </summary>        
        public RabbitMqFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            var rabbitParam = _configuration.GetSection("RabbitMQ").Get<RabbitMQConnectionModel>();
            _rabbitMqHostName = rabbitParam?.HostName ?? string.Empty;
            _rabbitMqUserName = rabbitParam?.UserName ?? string.Empty;
            _rabbitMqPassword = rabbitParam?.Password ?? string.Empty;
        }

        private static readonly object _lockObject = new();
        private static readonly ConcurrentDictionary<string, object> _lockObjectDict = new();
        private static readonly ConcurrentDictionary<string, RabbitMqMessagePublisher> _publisherDict = new();

        /// <summary>
        /// 取得生產者
        /// </summary>                
        public RabbitMqMessagePublisher Get(string mqExchangeName, string exchangeType = "Direct")
        {
            var key = $"{_rabbitMqHostName}_{mqExchangeName}";
            var publisher = GetPublisher(key, mqExchangeName, exchangeType);
            return publisher;

            // 取得生產者配置
            RabbitMqMessagePublisher GetPublisher(string key, string mqExchangeName, string exchangeType)
            {
                if (!_publisherDict.TryGetValue(key, out var publisher) || !publisher.IsOpen)
                {
                    var lockObject = GetLockObj(key);
                    lock (lockObject)
                    {
                        if (!_publisherDict.TryGetValue(key, out publisher) || !publisher.IsOpen)
                        {
                            publisher?.Dispose();

                            publisher = new RabbitMqMessagePublisher(new ExchangeModel
                            {
                                HostName = _rabbitMqHostName,
                                UserName = _rabbitMqUserName,
                                Password = _rabbitMqPassword,
                                ExchangeName = mqExchangeName,
                                ExchangeType = exchangeType
                            });
                            _publisherDict[key] = publisher;
                        }
                    }
                }
                return publisher;
            }
        }

        /// <summary>
        /// 取得鎖物件
        /// </summary>
        private object GetLockObj(string key)
        {
            if (!_lockObjectDict.TryGetValue(key, out var obj) || obj == null)
            {
                lock (_lockObject)
                {
                    if (!_lockObjectDict.TryGetValue(key, out obj) || obj == null)
                    {
                        obj = new object();
                        _lockObjectDict[key] = obj;
                    }
                }
            }
            return obj;
        }

        #region
        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {
            foreach (var publisher in _publisherDict.Values)
            {
                publisher.Dispose();
            }
            _publisherDict.Clear();
        }
        #endregion
    }
}
