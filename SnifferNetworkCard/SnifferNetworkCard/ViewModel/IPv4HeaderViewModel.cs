using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace SnifferNetworkCard.ViewModel
{
    public class IPv4HeaderViewModel
    {
        /// <summary>
        /// 【版本】第1區塊：4bit ，0~3
        /// </summary>
        [Description("版本")]
        public string Version { get; set; }

        /// <summary>
        /// 【首部長度】第2區塊：4bit ，4~7
        /// </summary>
        [Description("首部長度")]
        public string InternetHeaderLength { get; set; }

        /// <summary>
        /// 【區分服務】第3區塊：6bit ，8~13
        /// </summary>
        [Description("區分服務")]
        public string DifferentiatedServices { get; set; }

        /// <summary>
        /// 【顯式擁塞通告】第4區塊：2bit ，14~15
        /// </summary>
        [Description("顯式擁塞通告")]
        public string ExplicitCongestionNotification { get; set; }

        /// <summary>
        /// 【全長】第5區塊：16bit ，16~31
        /// </summary>
        [Description("全長")]
        public string TotalLength { get; set; }

        /// <summary>
        /// 【識別碼】第6區塊：16bit ，32~47
        /// </summary>
        [Description("識別碼")]
        public string Identification { get; set; }

        /// <summary>
        /// 【標誌】第7區塊：3bit，48~50
        /// </summary>
        [Description("標誌")]
        public string Flags { get; set; }

        /// <summary>
        ///【分片偏移】第8區塊：13bit，51~63
        /// </summary>
        [Description("分片偏移")]
        public string FragmentOffset { get; set; }

        /// <summary>
        ///【存活時間】第9區塊：8bit，64~71
        /// </summary>
        [Description("存活時間")]
        public string TimeToLive { get; set; }

        /// <summary>
        ///【協定】第10區塊：8bit，72~79
        /// </summary>
        [Description("協定")]
        public ProtocolType Protocol { get; set; }

        /// <summary>
        ///【首部核對和】第11區塊：16bit，80~95
        /// </summary>
        [Description("首部核對和")]
        public string HeaderChecksum { get; set; }

        /// <summary>
        ///【來源IP位址】第12區塊：16bit，96~127
        /// </summary>
        [Description("來源IP位址")]
        public IPAddress SourceAddress { get; set; }

        /// <summary>
        ///【目的IP位址】第13區塊：16bit，128~159
        /// </summary>
        [Description("來源IP位址")]
        public IPAddress DestinationAddress { get; set; }
        /// <summary>
        ///【選項（如首部長度>5）】第14區塊：3bit，160
        /// </summary>
        [Description("來源IP位址")]
        public string Options { get; set; }

        /// <summary>
        ///【資料】第15區塊：變動Bit 160~
        /// </summary>
        [Description("資料")]
        public string Data { get; set; }
    }


}
