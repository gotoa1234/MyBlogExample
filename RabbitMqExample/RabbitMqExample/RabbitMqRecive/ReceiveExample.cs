using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

#pragma warning disable CS8625 // �L�k�N null �`���ഫ�����i�� Null ���Ѧҫ��O�C
namespace RabbitMqRecive
{
    public partial class ReceiveExample : Form
    {
        /// <summary>
        /// RabbitMQ���������
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
        /// �g�J��ƨ�TextBox��
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
        /// �Ұ�RabbitMQ�����ݫ��s
        /// </summary>
        private void ButtonStarted_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "�s����...";
            RecevieRabbitMQ();
        }

        /// <summary>
        /// �����ݥD��Method
        /// </summary>
        private void RecevieRabbitMQ()
        {
            //�إ�MQ�s�u�򥻸�T
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            //�}�ҳs�u
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //Queue�򥻳]�m
            channel.QueueDeclare(queue: "�ڬOQueue��Key",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            var consumer = new EventingBasicConsumer(channel);

            //�]�wRabbitMQ ���O��(Consumer)���u�@
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                ThreadStart threadParameters = new ThreadStart(delegate { WriteTextBox($@"[����] {message}"); });
                RabbitMQThread = new Thread(threadParameters);
                RabbitMQThread.Start();
            };
            channel.BasicConsume(queue: "�ڬOQueue��Key",
                                 autoAck: true,
                                 consumer: consumer);

        }

    }
}