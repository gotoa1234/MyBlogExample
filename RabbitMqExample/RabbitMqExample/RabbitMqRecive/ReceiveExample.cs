using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

#pragma warning disable CS8625 // 無法將 null 常值轉換成不可為 Null 的參考型別。
namespace RabbitMqRecive
{
    public partial class ReceiveExample : Form
    {
        /// <summary>
        /// RabbitMQ接收執行緒
        /// </summary>
        private Thread RabbitMQThread = default;

        public ReceiveExample()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 寫入資料到TextBox中
        /// </summary>
        public void WriteTextBox(string text)
        {
            if (receiveTextBox.InvokeRequired)
            {
                Action writeAction = delegate { WriteTextBox($"{text}{Environment.NewLine}"); };
                receiveTextBox.Invoke(writeAction);
            }
            else
            {
                receiveTextBox.Text += text;
            }
        }

        /// <summary>
        /// 啟動RabbitMQ接收端按鈕
        /// </summary>
        private void ButtonStarted_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "連接中...";
            RecevieRabbitMQ();
        }

        /// <summary>
        /// 接收端主體Method
        /// </summary>
        private void RecevieRabbitMQ()
        {
            //建立MQ連線基本資訊
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.51.28",
                UserName = "admin",
                Password = "admin"
            };
            //開啟連線
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //Queue基本設置
            channel.QueueDeclare(queue: "Louis Test 生產者",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            var consumer = new EventingBasicConsumer(channel);

            //設定RabbitMQ 消費者(Consumer)的工作
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                ThreadStart threadParameters = new ThreadStart(delegate { WriteTextBox($@"[接收] {message}"); });
                RabbitMQThread = new Thread(threadParameters);
                RabbitMQThread.Start();
            };
            channel.BasicConsume(queue: "Louis Test 生產者",
                                 autoAck: true,
                                 consumer: consumer);

        }

    }
}