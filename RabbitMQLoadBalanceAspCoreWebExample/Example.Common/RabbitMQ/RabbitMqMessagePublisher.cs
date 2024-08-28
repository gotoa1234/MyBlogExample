using Example.Common.RabbitMQ.Common;
using Example.Common.RabbitMQ.Model;
using RabbitMQ.Client;
using System.Text;

namespace Example.Common.RabbitMQ
{
    public class RabbitMqMessagePublisher : RabbitMQBaseParameterModel, IDisposable
    {
        /// <summary>
        /// 生產者
        /// </summary>        
        public RabbitMqMessagePublisher(ExchangeModel rabbitParameters)
        {
            SettingValue();
            
            // 設定生產者 - 基本設定
            void SettingValue()
            {
                _connection = RabbitMQHelper.GetConnection(rabbitParameters);
                _channel = RabbitMQHelper.GetModel(_connection);
                _exchangeType = rabbitParameters.ExchangeType;
                _channel.ExchangeDeclare(rabbitParameters.ExchangeName, rabbitParameters.ExchangeType.ToString().ToLower(), true, false, null);
            }
        }

        /// <summary>
        /// 檢查 RabbitMQ 連線，確保當前連線是否開啟
        /// </summary>
        public bool IsOpen => _connection?.IsOpen == true;

        /// <summary>
        /// 發送訊息
        /// </summary>        
        public void PblisherSend(string message, string exchangeName, string routingKey)
        {
            var properties = _channel.CreateBasicProperties();
            properties.Timestamp = new AmqpTimestamp(DateTime.Now.Ticks);
            // 設定消息的持久性，確保 RabbitMQ 伺服器重啟後仍然存在
            properties.Persistent = true; 
            _channel.BasicPublish(exchangeName, routingKey, false, properties, Encoding.UTF8.GetBytes(message));
        }

        #region 解構式 - 釋放資源
        
        ~RabbitMqMessagePublisher()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _channel?.Dispose();
            _connection?.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
