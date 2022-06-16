using SnifferNetworkCard.Service;
using SnifferNetworkCard.ViewModel;
using System.Net;

namespace SnifferNetworkCard
{
    public partial class SnifferNetworkCardForm : Form
    {
        /// <summary>
        /// �s�u��IP��T�M��
        /// </summary>
        private List<string> _NowConnectList = new List<string>();
        
        /// <summary>
        /// �D��Socket�A��
        /// </summary>
        private SocketService? _MainSocketService = null;

        /// <summary>
        /// ������l��
        /// </summary>
        public SnifferNetworkCardForm()
        {
            InitializeComponent();
            //�ҥ����w�ġA����FPS�W�[�A���{�{
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            Initinal();
            ResetForm();

            void Initinal()
            {
                //����d
                GetNetWorkCard();
                //����Socket����
                _MainSocketService = new SocketService(
                    String.Empty,
                    CreateConnectItemNode,
                    OnSocketReciveAddInformationListItem
                    );

            }
        }

        #region  �禡�� - 1.�����d 2.��{���dIP�PSocketô�� 

        /// <summary>
        /// ���o��e�q���Ҧ������d��}
        /// </summary>
        private void GetNetWorkCard()
        {
            //���o��������W��
            var HosyEntry = Dns.GetHostEntry((Dns.GetHostName()));
            //�N�o�{�����d�s���ComboBox��
            if (HosyEntry.AddressList.Length > 0)
            {
                foreach (IPAddress ip in HosyEntry.AddressList)
                {
                    NetworkCardComboBox.Items.Add(ip.ToString());
                }
            }
        }

        /// <summary>
        /// ���ͳs�uIP�����
        /// </summary>
        private ListViewItem CreateConnectItemNode(IPConnectionData ipData)
        {
            ListViewItem nodedata = new ListViewItem(ipData.SourceIP);
            nodedata.SubItems.Add(ipData.flow);
            nodedata.SubItems.Add(ipData.prototol);
            nodedata.SubItems.Add(ipData.Receive_DateTime.ToString("yyyy/MM/dd 19:mm:ss"));
            return nodedata;
        }

        /// <summary>
        /// ��Socket�����ʥ]�ɪ��u�@
        /// </summary>
        public void OnSocketReciveAddInformationListItem(ListViewItem node)
        {
            //�������@�~ - ����PSocket���椬
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate {
                    OnSocketReciveAddInformationListItem(node);
                });
                return;
            }

            //1. �N�ʥ]�O������v�y�q��T����
            this.InformationListView.Items.Add(node);

            //2. �ʥ]�t�m������
            var ipData = new IPConnectionData()
            {
                SourceIP = node.Text,
                flow = node.SubItems[1].Text,
                prototol = node.SubItems[2].Text,
                Receive_DateTime = DateTime.Parse(node.SubItems[3].Text)
            };

            //3. �`�I�s���IP�����M�椤
            if (_NowConnectList == null)
            {
                _NowConnectList = new List<string>();
                _NowConnectList.Add(node.Text);

                this.ConnectsListView.Items.Add(CreateConnectItemNode(ipData));
            }
            else if (!_NowConnectList.Contains(node.Text))
            {
                _NowConnectList.Add(node.Text);
                this.ConnectsListView.Items.Add(CreateConnectItemNode(ipData));

            }
            else
            {
                foreach (object oom in this.ConnectsListView.Items)
                {
                    if (((ListViewItem)oom).SubItems[0].Text == node.Text)//�Ӷ��جۦP�����p�U�Ч�s
                    {
                        ((ListViewItem)oom).SubItems[1].Text = node.SubItems[1].Text;
                        ((ListViewItem)oom).SubItems[2].Text = node.SubItems[2].Text;
                        ((ListViewItem)oom).SubItems[3].Text = node.SubItems[3].Text;
                        break;
                    }
                }
            }
        }

 
        #endregion

        /// <summary>
        /// �U�Ԧ���ܺ����d��}
        /// </summary>
        private void NetworkCardComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetForm();
                //ô���s���d���s�u��socket IP 
                _MainSocketService.ResetNowConnectionNewtwork(NetworkCardComboBox.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// ��l�ƪ��
        /// </summary>
        private void ResetForm()
        {
            //���m�s�u�W����
            _NowConnectList = null;

            //Reset ���v����
            InformationListView.Items.Clear();
            InformationListView.Columns.Clear();
            InformationListView.Columns.Add("�ӷ�IP", 150, HorizontalAlignment.Center);
            InformationListView.Columns.Add("��e�y�qbyte/s", 150, HorizontalAlignment.Center);
            InformationListView.Columns.Add("TCP/UDP��w", 150, HorizontalAlignment.Center);
            InformationListView.Columns.Add("��ƶi�Ӯɶ�", 200, HorizontalAlignment.Center);

            //Reset �s�u�M��
            ConnectsListView.Items.Clear();
            ConnectsListView.Columns.Clear();
            ConnectsListView.Columns.Add("�ӷ�IP", 150, HorizontalAlignment.Center);
            ConnectsListView.Columns.Add("byte/s", 150, HorizontalAlignment.Center);
            ConnectsListView.Columns.Add("TCP/UDP��w", 150, HorizontalAlignment.Center);
            ConnectsListView.Columns.Add("�̫�i�Ӯɶ�", 200, HorizontalAlignment.Center);
        }
    }
}