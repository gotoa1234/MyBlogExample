using SnifferNetworkCard.Common;
using SnifferNetworkCard.Service;
using SnifferNetworkCard.ViewModel;
using System.Net;
using System.Text;

namespace SnifferNetworkCard.HeaderProtocol.L3TransportLayer
{
    /// <summary>
    /// Layer3 取得UDP的標頭資訊 - 共有4個區塊固定Bit區塊(標頭)與最後1個變動Bit區塊(資料)
    /// </summary>
    public class UDPHeader
    {
        private int _sourcePort;
        private int _destinationPort;
        private int _length;
        private int _checksum;
        private byte[] _data = new byte[4096];
        private long _dataLength = 8;
        public UDPHeaderViewModel Header
        {
            get
            {
                return new UDPHeaderViewModel()
                {
                    SourcePort = _sourcePort.ToString(),
                    DestinationPort = _destinationPort.ToString(),
                    Length = _length.ToString(),
                    Checksum = _checksum.ToString(),
                    Data = Data(null),
                };
            }
        }

        /// <summary>
        /// 取得UDP標頭資料
        /// </summary>
        public UDPHeader(byte[] byBuffer, int nReceived)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
                BinaryReader binaryReader = new BinaryReader(memoryStream);

                //第1區塊
                _sourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第2區塊
                _destinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第3區塊
                _length = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第4區塊
                _checksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第5區塊，UDP標頭前4個區塊固定加總為 8 Byte
                _dataLength = nReceived - 8;
                Array.Copy(byBuffer
                       , 8
                       , _data
                       , 0
                       , _dataLength);

                if (_length > 0)
                {
                    var writeString = new StringBuilder();
                    var useClass =new UDPHeaderViewModel();
                    writeString.AppendLine($@"{Util.GetDescription<UDPHeaderViewModel>(nameof(useClass.SourcePort))} ： {Header.SourcePort}");
                    writeString.AppendLine($@"{Util.GetDescription<UDPHeaderViewModel>(nameof(useClass.DestinationPort))} ： {Header.DestinationPort}");
                    writeString.AppendLine($@"{Util.GetDescription<UDPHeaderViewModel>(nameof(useClass.Length))}(Byte)  ： {Header.Length}");
                    writeString.AppendLine($@"{Util.GetDescription<UDPHeaderViewModel>(nameof(useClass.Checksum))} ： {Header.Checksum}");
                    writeString.AppendLine($@"實際封包總長(Byte) ： {nReceived} ");
                    writeString.AppendLine($@"實際Data總長(Byte) ： {_dataLength} ");
                    writeString.AppendLine($@"{Util.GetDescription<UDPHeaderViewModel>(nameof(useClass.Data))}(Base64) ： {Data(null)}");
                    writeString.AppendLine($@"{Util.GetDescription<UDPHeaderViewModel>(nameof(useClass.Data))}(UTF8) ： {Data(Encoding.UTF8)}");                    
                    WriteRecordTxtService.Instance.Value.WriteText(
                        "",
                        true,
                        "UDP", 
                        writeString.ToString()
                        );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                      ex.Message,
                      "錯誤訊息",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error
                      );
            }
        }

        /// <summary>
        ///【資料】第5區塊：變動Bit 64~
        /// </summary>
        private string Data(Encoding? useEnconding)
        {
            //取得UDP Data實際資料
            byte[] newBytes = new byte[_dataLength];
            Array.Copy(_data, newBytes, _dataLength);
            if (useEnconding == null)
            {
                return Convert.ToBase64String(newBytes);
            }
            else
            { 
                return useEnconding.GetString(newBytes);
            }
        }
    }
}
