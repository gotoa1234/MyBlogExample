using SnifferNetworkCard.Common;
using SnifferNetworkCard.HeaderProtocol.L2InternetLayer;
using SnifferNetworkCard.ViewModel;
using System.Net;
using System.Net.Sockets;

namespace SnifferNetworkCard.Service
{
    /// <summary>
    /// Socket實作捕抓封包資訊
    /// </summary>
    public class SocketService
    {
        //測試
        public NetworkStream myNetworkStream { get; set; }
        /// <summary>
        /// 分析封包變數
        /// </summary>
        private byte[] _ByteData = new byte[Consts.SocketPackageByteLength];

        /// <summary>
        /// 主體Socket物件
        /// </summary>
        public Socket MainSocket { get; set; }

        /// <summary>
        /// 當前Socket連接網卡
        /// </summary>
        private string NowConnectionNewtwork = string.Empty;

        /// <summary>
        /// 窗體委派互動事件 - 產生ListViewItem
        /// </summary>
        private readonly Func<IPConnectionData, ListViewItem> ProductNodeMethod;

        /// <summary>
        /// 窗體委派互動事件 - 增加資訊到"連線清單"與"歷史清單"
        /// </summary>
        private readonly Action<ListViewItem> AddListViewNodeMethod;

        /// <summary>
        /// 是否捕捉封包中
        /// </summary>
        public bool IsCapturing { get; set; }


        /// <summary>
        /// 建構式
        /// </summary>
        public SocketService(string nowConnecttion,
            Func<IPConnectionData, ListViewItem> _productNodeAct,
            Action<ListViewItem> _addListViewNodeAct
            )
        {
            NowConnectionNewtwork = nowConnecttion;
            ProductNodeMethod = _productNodeAct;
            AddListViewNodeMethod = _addListViewNodeAct;
        }

        /// <summary>
        /// 重新設定Socket繫結網卡
        /// </summary>
        /// <param name="newNetwrokCard">使用者選擇的網卡</param>
        public void ResetNowConnectionNewtwork(string newNetwrokCard)
        {
            this.NowConnectionNewtwork = newNetwrokCard;
            this.GetCaptureWorking();
        }

        /// <summary>
        /// 取得來源封包工作
        /// </summary>
        private void GetCaptureWorking()
        {
            //開始捕捉封包
            if (!IsCapturing)
            {
                IsCapturing = true;

                //建立Socket-初始設定
                //AddressFamily.InterNetwork：IPv4  說明 https://docs.microsoft.com/zh-tw/dotnet/api/system.net.sockets.addressfamily?view=net-6.0
                //SocketType 說明： https://docs.microsoft.com/zh-tw/dotnet/api/system.net.sockets.sockettype?view=net-6.0
                MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);

                //將Socket與網路卡位址做繫結 - 設定
                MainSocket.Bind(new IPEndPoint(IPAddress.Parse(NowConnectionNewtwork), 0));

                //封包的標頭-設定
                //SocketOptionName.HeaderIncluded：表示應用程式將提供輸出資料包的 IP 標頭。
                MainSocket.SetSocketOption(SocketOptionLevel.IP,
                                           SocketOptionName.HeaderIncluded,
                                           true);

                byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
                byte[] byOut = new byte[4] { 1, 0, 0, 0 };

                //捕捉的內容-設定
                //IOControlCode.ReceiveAll：啟用網路上所有 IPv4 封包的接收。 通訊端必須擁有通訊協定家族 InterNetwork
                MainSocket.IOControl(IOControlCode.ReceiveAll,
                                     byTrue,
                                     byOut);

                //開始進行接收，OnReceive是收到封包後的工作
                MainSocket.BeginReceive(_ByteData, 0, _ByteData.Length, SocketFlags.None,
                    new AsyncCallback(OnReceive), null);

            }
            else//切換/重新繫節網卡時重置Socket
            {
                IsCapturing = false;
                MainSocket.Close();
            }
        }

        /// <summary>
        /// 當接收到資料時的工作
        /// </summary>
        /// <param name="asyncResult">儲存非同步作業的相關資訊</param>
        public void OnReceive(IAsyncResult asyncResult)
        {
            try
            {
                //資料byte的結尾長度
                int nReceived = MainSocket.EndReceive(asyncResult);
                //解析封包串流資料
                ParseFlowData(_ByteData, nReceived);

                if (IsCapturing)//持續捕捉
                {
                    _ByteData = new byte[Consts.SocketPackageByteLength];
                    MainSocket.BeginReceive(_ByteData, 0, _ByteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// IP對應TCP或者UDP (Header)之流量
        /// </summary>
        /// <param name="byteData">封包內容</param>
        /// <param name="nReceived">結尾長度</param>
        private void ParseFlowData(
            byte[] byteData,
            int nReceived)
        {
            
            var ipHeader = new IPv4Header(byteData, nReceived);

            //Filter:sourceAddress 為當前使用者選擇的網路卡IP 因為我們是要監測 連到這個網路卡的流量 而非該網路卡傳送出去的流量
            if (ipHeader.Header.SourceAddress.ToString() == this.NowConnectionNewtwork)
            {
                //IP單一節點資料
                var IPNode = new IPConnectionData();
                //連到我們網卡的IP
                IPNode.SourceIP = ipHeader.Header.DestinationAddress.ToString();
                //連到我們網卡的協定機制
                IPNode.prototol = ipHeader.Header.Protocol.ToString();
                //長度
                IPNode.flow = ipHeader.Header.TotalLength;
                //現在時間
                IPNode.Receive_DateTime = DateTime.Now;

                //產生ListViewItem資料並且呈現在窗體上
                var nodeData = ProductNodeMethod.Invoke(IPNode);
                AddListViewNodeMethod.Invoke(nodeData);
            }
        }

    }
}
