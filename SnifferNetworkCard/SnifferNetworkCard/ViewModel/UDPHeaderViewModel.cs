using System.ComponentModel;

namespace SnifferNetworkCard.ViewModel
{
    /// <summary>
    /// UDP標頭資訊
    /// </summary>
    public class UDPHeaderViewModel
    {
        /// <summary>
        /// 【來源連接埠】第1區塊：2Byte，0~15
        /// </summary>
        [Description("來源連接埠")]
        public string SourcePort { get; set; }

        /// <summary>
        /// 【目的連接埠】第2區塊：2Byte ，16~31
        /// </summary>
        [Description("目的連接埠")]
        public string DestinationPort { get; set; }

        /// <summary>
        /// 【報文長度】第3區塊：2Byte，32~47
        /// </summary>
        [Description("報文長度")]
        public string Length { get; set; }

        /// <summary>
        /// 【校驗和】第4區塊：2Byte，48~63
        /// </summary>
        [Description("校驗和")]
        public string Checksum { get; set; }

        /// <summary>
        ///【資料】第5區塊：變動Bit 64~
        /// </summary>
        [Description("資料")]
        public string Data { get; set; }
    }
}
