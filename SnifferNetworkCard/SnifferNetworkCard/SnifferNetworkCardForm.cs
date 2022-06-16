using SnifferNetworkCard.Service;
using SnifferNetworkCard.ViewModel;
using System.Net;

namespace SnifferNetworkCard
{
    public partial class SnifferNetworkCardForm : Form
    {
        /// <summary>
        /// 連線的IP資訊清單
        /// </summary>
        private List<string> _NowConnectList = new List<string>();
        
        /// <summary>
        /// 主體Socket服務
        /// </summary>
        private SocketService? _MainSocketService = null;

        /// <summary>
        /// 視窗初始化
        /// </summary>
        public SnifferNetworkCardForm()
        {
            InitializeComponent();
            //啟用雙緩衝，視窗FPS增加，不閃爍
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            Initinal();
            ResetForm();

            void Initinal()
            {
                //抓網卡
                GetNetWorkCard();
                //產生Socket物件
                _MainSocketService = new SocketService(
                    String.Empty,
                    CreateConnectItemNode,
                    OnSocketReciveAddInformationListItem
                    );

            }
        }

        #region  函式區 - 1.取網卡 2.實現網卡IP與Socket繫結 

        /// <summary>
        /// 取得當前電腦所有網路卡位址
        /// </summary>
        private void GetNetWorkCard()
        {
            //取得本機網域名稱
            var HosyEntry = Dns.GetHostEntry((Dns.GetHostName()));
            //將發現的網卡存放到ComboBox中
            if (HosyEntry.AddressList.Length > 0)
            {
                foreach (IPAddress ip in HosyEntry.AddressList)
                {
                    NetworkCardComboBox.Items.Add(ip.ToString());
                }
            }
        }

        /// <summary>
        /// 產生連線IP的資料
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
        /// 當Socket接收封包時的工作
        /// </summary>
        public void OnSocketReciveAddInformationListItem(ListViewItem node)
        {
            //跨執行緒作業 - 窗體與Socket間交互
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate {
                    OnSocketReciveAddInformationListItem(node);
                });
                return;
            }

            //1. 將封包記錄到歷史流量資訊紀錄
            this.InformationListView.Items.Add(node);

            //2. 封包配置成物件
            var ipData = new IPConnectionData()
            {
                SourceIP = node.Text,
                flow = node.SubItems[1].Text,
                prototol = node.SubItems[2].Text,
                Receive_DateTime = DateTime.Parse(node.SubItems[3].Text)
            };

            //3. 節點存放到IP紀錄清單中
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
                    if (((ListViewItem)oom).SubItems[0].Text == node.Text)//該項目相同的情況下請更新
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
        /// 下拉式選擇網路卡位址
        /// </summary>
        private void NetworkCardComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetForm();
                //繫結新網卡中連線的socket IP 
                _MainSocketService.ResetNowConnectionNewtwork(NetworkCardComboBox.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 初始化表單
        /// </summary>
        private void ResetForm()
        {
            //重置連線名單資料
            _NowConnectList = null;

            //Reset 歷史紀錄
            InformationListView.Items.Clear();
            InformationListView.Columns.Clear();
            InformationListView.Columns.Add("來源IP", 150, HorizontalAlignment.Center);
            InformationListView.Columns.Add("當前流量byte/s", 150, HorizontalAlignment.Center);
            InformationListView.Columns.Add("TCP/UDP協定", 150, HorizontalAlignment.Center);
            InformationListView.Columns.Add("資料進來時間", 200, HorizontalAlignment.Center);

            //Reset 連線清單
            ConnectsListView.Items.Clear();
            ConnectsListView.Columns.Clear();
            ConnectsListView.Columns.Add("來源IP", 150, HorizontalAlignment.Center);
            ConnectsListView.Columns.Add("byte/s", 150, HorizontalAlignment.Center);
            ConnectsListView.Columns.Add("TCP/UDP協定", 150, HorizontalAlignment.Center);
            ConnectsListView.Columns.Add("最後進來時間", 200, HorizontalAlignment.Center);
        }
    }
}