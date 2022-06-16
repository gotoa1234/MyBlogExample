using SnifferNetworkCard.HeaderProtocol.L3TransportLayer;
using SnifferNetworkCard.ViewModel;
using System.Net;
using System.Net.Sockets;

namespace SnifferNetworkCard.HeaderProtocol.L2InternetLayer
{
    /// <summary>
    /// Layer2 取得IPv4的標頭資訊 - 共有14個區塊固定Bit區塊(標頭)與最後1個變動Bit區塊(資料)
    /// </summary>
    public class IPv4Header
    {
        private int _version;
        private int _internetHeaderLength;
        private int _differentiatedServices;
        private int _explicitCongestionNotification;
        private int _totalLength;
        private int _identification;
        private int _flags;
        private int _fragmentOffset;
        private int _timeToLive;
        private int _protocol;
        private int _headerChecksum;
        private uint _sourceAddress;
        private uint _destinationAddress;
        private int _options;
        private byte[] _data = new byte[4096];
        private long _dataLength;
        public IPv4HeaderViewModel Header
        {
            get
            {
                return new IPv4HeaderViewModel()
                {
                    Version = _version.ToString(),
                    InternetHeaderLength = _internetHeaderLength.ToString(),
                    DifferentiatedServices = _differentiatedServices.ToString(),
                    ExplicitCongestionNotification = _explicitCongestionNotification.ToString(),
                    TotalLength = _totalLength.ToString(),
                    Identification = _identification.ToString(),
                    Flags = _flags.ToString(),
                    FragmentOffset = _fragmentOffset.ToString(),
                    TimeToLive = _timeToLive.ToString(),
                    Protocol = (ProtocolType)_protocol,
                    HeaderChecksum = _headerChecksum.ToString(),
                    SourceAddress = new IPAddress(_sourceAddress),
                    DestinationAddress = new IPAddress(_destinationAddress),
                    Options = _options.ToString(),
                    Data = Data(),
                };
            }
        }

        /// <summary>
        /// IPv4標頭
        /// </summary>
        public IPv4Header(byte[] byBuffer, int nReceived)
        {
            try
            {
                //Create MemoryStream out of the received bytes
                MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
                //Next we create a BinaryReader out of the MemoryStream
                BinaryReader binaryReader = new BinaryReader(memoryStream);

                //第1區塊
                var byteTemp = binaryReader.ReadByte();
                _version = (byteTemp & 0b11110000) >> 4;
                //第2區塊
                _internetHeaderLength = (byteTemp & 0b00001111);
                //第3區塊
                byteTemp = binaryReader.ReadByte();
                _differentiatedServices = (byteTemp & 0b11111100) >> 2;
                //第4區塊
                _explicitCongestionNotification = (byteTemp & 0b00000011);
                //第5區塊
                _totalLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第6區塊
                _identification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第7區塊
                var shrotTemp = binaryReader.ReadInt16();
                _flags = (shrotTemp & 0b1110000000000000) >> 13;
                //第8區塊
                _fragmentOffset = (shrotTemp & 0b0001111111111111);
                //第9區塊
                byteTemp = binaryReader.ReadByte();
                _timeToLive = byteTemp;
                //第10區塊
                byteTemp = binaryReader.ReadByte();
                _protocol = byteTemp;
                //第11區塊
                _headerChecksum = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                //第12區塊
                _sourceAddress = (uint)binaryReader.ReadInt32();
                //第13區塊
                _destinationAddress = (uint)binaryReader.ReadInt32();
                //第14區塊
                if (_internetHeaderLength > 5)
                {
                    //3個bit的Options
                    _options += binaryReader.ReadBoolean() ? 4 : 0;
                    _options += binaryReader.ReadBoolean() ? 2 : 0;
                    _options += binaryReader.ReadBoolean() ? 1 : 0;
                    //1個Padding
                    binaryReader.ReadBoolean();
                }
                //第15區塊，取得方式: 總長度 - 現在binaryReader已讀取的Byte，剩下的都是Data
                _dataLength = _totalLength - binaryReader.BaseStream.Position;
                Array.Copy(byBuffer
                       , binaryReader.BaseStream.Position
                       , _data, 0,
                       _dataLength);

                var selfProtocol = (ProtocolType)_protocol;
                if (selfProtocol == ProtocolType.Udp)
                {
                    new UDPHeader(_data, (int)_dataLength);
                }
                else if (selfProtocol == ProtocolType.Tcp)
                {
                    new TCPHeader(_data, (int)_dataLength);
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
        ///【資料】第15區塊：變動Bit 160~
        /// </summary>
        public string Data()
        {
            byte[] newBytes = new byte[_dataLength];
            Array.Copy(_data, newBytes, _dataLength);
            return System.Text.Encoding.UTF8.GetString(newBytes);
        }
    }
}
