using RabbitMQ.Client;
using System.Text;

namespace RabbitMqSend
{
    public partial class SendExample : Form
    {
        public SendExample()
        {
            InitializeComponent();
        }

        private void SendExample_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 執行傳送訊息按鈕
        /// </summary>
        private void ButtonSend_Click(object sender, EventArgs e)
        {
            SendRabbitMQWorking();
        }

        /// <summary>
        /// 傳送主體(生產者)的Method
        /// </summary>
        public void SendRabbitMQWorking()
        {
            //建立MQ連線基本資訊
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.51.93",
                UserName = "admin",
                Password = "admin"
            };

            //傳送資訊(生產者)
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Queue基本設置
                channel.QueueDeclare(queue: "Louis Test 生產者",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                //傳送的內容
                string message = $@"現在時間{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Louis Test 生產者",
                                     basicProperties: null,
                                     body: body);
                sendTextBox.AppendText($@"[傳送] {message} {Environment.NewLine}");
            }
        }
    }
}