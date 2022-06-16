using SnifferNetworkCard.Common;
using SnifferNetworkCard.Service;
using SnifferNetworkCard.ViewModel;
using System.Net;
using System.Text;

namespace SnifferNetworkCard.HeaderProtocol.L3TransportLayer
{
    /// <summary>
    /// Layer3 取得TCP的標頭資訊 - 共有18個區塊固定Bit區塊(標頭)與最後1個變動Bit區塊(資料)
    /// </summary>
    public class TCPHeader
    {
        private int _sourcePort;
        private int _destinationPort;
        private uint _sequenceNumber;
        private uint _acknowledgementNumber;
        private int _headerLength;
        private int _reserved;
        private int _ecnEcho;
        private int _explicitCongestionNotificationFlag;
        private int _congestionWindowReduced;
        private int _urgent;
        private int _acknowledgment;
        private int _push;
        private int _reset;
        private int _sync;
        private int _finish;
        private int _checksum;
        private int _urgentPointer;
        private int _window;
        private byte[] _data = new byte[4096];
        private long _dataLength = 8;

        public TCPHeaderViewModel Header
        {
            get
            {
                return new TCPHeaderViewModel()
                {
                    SourcePort = _sourcePort.ToString(),
                    DestinationPort = _destinationPort.ToString(),
                    SequenceNumber = _sequenceNumber.ToString(),
                    AcknowledgementNumber = _acknowledgementNumber.ToString(),
                    HeaderLength = _headerLength.ToString(),
                    Reserved = _reserved.ToString(),
                    ExplicitCongestionNotificationFlag = _explicitCongestionNotificationFlag.ToString(),
                    CongestionWindowReduced = _congestionWindowReduced.ToString(),
                    EcnEcho = _ecnEcho.ToString(),
                    Urgent = _urgent.ToString(),
                    Acknowledgment = _acknowledgment.ToString(),
                    Push = _push.ToString(),
                    Reset = _reset.ToString(),
                    Sync = _sync.ToString(),
                    Finish = _finish.ToString(),
                    Window = _window.ToString(),
                    Checksum = _checksum.ToString(),
                    UrgentPointer = _urgentPointer.ToString(),
                    Data = Data(null),
                };
            }
        }

        public TCPHeader(byte[] byBuffer, int nReceived)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
                BinaryReader binaryReader = new BinaryReader(memoryStream);
                //var byteTemp = binaryReader.ReadByte();
                //第1區塊
                _sourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第2區塊
                _destinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第3區塊
                _sequenceNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                //第4區塊
                _acknowledgementNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                //第5區塊
                var byteTemp = binaryReader.ReadByte();
                _headerLength = (byteTemp & 0b11110000) >> 4;
                //第6區塊
                _reserved = (byteTemp & 0b00001110) >> 1;
                //第7區塊
                _explicitCongestionNotificationFlag = (byteTemp & 0b000000001);
                //第8區塊
                byteTemp = binaryReader.ReadByte();
                _congestionWindowReduced = (byteTemp & 0b10000000) >> 7;
                //第9區塊
                _ecnEcho = (byteTemp & 0b01000000) >> 6;
                //第10區塊
                _urgent = (byteTemp & 0b00100000) >> 5;
                //第11區塊
                _acknowledgment = (byteTemp & 0b00010000) >> 4;
                //第12區塊
                _push = (byteTemp & 0b00001000) >> 3;
                //第13區塊
                _reset = (byteTemp & 0b00000100) >> 2;
                //第14區塊
                _sync = (byteTemp & 0b00000010) >> 1;
                //第15區塊
                _finish = (byteTemp & 0b000000001);
                //第16區塊
                _window = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第17區塊
                _checksum = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第18區塊
                _urgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                //第19區塊，取得方式: 總長度 - 現在binaryReader已讀取的Byte，剩下的都是Data
                _dataLength = (_headerLength * 4) - binaryReader.BaseStream.Position;
                Array.Copy(byBuffer
                       , binaryReader.BaseStream.Position
                       , _data, 0,
                       _dataLength);

                if (_dataLength > 0)
                {
                    var writeString = new StringBuilder();
                    var useClass = new TCPHeaderViewModel();
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.SourcePort))} ： {Header.SourcePort}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.DestinationPort))} ： {Header.DestinationPort}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.SequenceNumber))} ： {Header.SequenceNumber}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.AcknowledgementNumber))} ： {Header.AcknowledgementNumber}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.HeaderLength))}(Byte) ： {Header.HeaderLength}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Reserved))} ： {Header.Reserved}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.ExplicitCongestionNotificationFlag))} ： {Header.ExplicitCongestionNotificationFlag}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.CongestionWindowReduced))} ： {Header.CongestionWindowReduced}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.EcnEcho))} ： {Header.EcnEcho}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Urgent))} ： {Header.Urgent}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Acknowledgment))} ： {Header.Acknowledgment}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Push))} ： {Header.Push}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Reset))} ： {Header.Reset}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Sync))} ： {Header.Sync}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Finish))} ： {Header.Finish}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Window))} ： {Header.Window}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Checksum))} ： {Header.Checksum}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.UrgentPointer))} ： {Header.UrgentPointer}");
                    writeString.AppendLine($@"實際封包總長(Byte) ： {nReceived} ");
                    writeString.AppendLine($@"實際Data總長(Byte) ： {_dataLength} ");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Data))}(Base64) ： {Data(null)}");
                    writeString.AppendLine($@"{Util.GetDescription<TCPHeaderViewModel>(nameof(useClass.Data))}(UTF8) ： {Data(Encoding.UTF8)}");

                    WriteRecordTxtService.Instance.Value.WriteText(
                        "",
                        true,
                        "TCP",
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
        ///【資料】第19區塊：變動Bit 160~
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
