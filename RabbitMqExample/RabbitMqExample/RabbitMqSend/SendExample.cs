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
        /// ����ǰe�T�����s
        /// </summary>
        private void ButtonSend_Click(object sender, EventArgs e)
        {
            SendRabbitMQWorking();
        }

        /// <summary>
        /// �ǰe�D��(�Ͳ���)��Method
        /// </summary>
        public void SendRabbitMQWorking()
        {
            //�إ�MQ�s�u�򥻸�T
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.51.93",
                UserName = "admin",
                Password = "admin"
            };

            //�ǰe��T(�Ͳ���)
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Queue�򥻳]�m
                channel.QueueDeclare(queue: "Louis Test �Ͳ���",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                //�ǰe�����e
                string message = $@"�{�b�ɶ�{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Louis Test �Ͳ���",
                                     basicProperties: null,
                                     body: body);
                sendTextBox.AppendText($@"[�ǰe] {message} {Environment.NewLine}");
            }
        }
    }
}