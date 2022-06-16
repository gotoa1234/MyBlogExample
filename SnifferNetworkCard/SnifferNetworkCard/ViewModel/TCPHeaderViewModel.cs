using System.ComponentModel;

namespace SnifferNetworkCard.ViewModel
{
    /// <summary>
    /// TCP標頭資訊
    /// </summary>
    public class TCPHeaderViewModel
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
        /// 【序列號碼】第3區塊：4Byte ，32~63
        /// </summary>
        [Description("序列號碼")]
        public string SequenceNumber { get; set; }

        /// <summary>
        /// 【確認號碼（當ACK設定）】第4區塊：4Byte ，64~95
        /// </summary>
        [Description("確認號碼（當ACK設定）")]
        public string AcknowledgementNumber { get; set; }

        /// <summary>
        /// 【資料偏移】第5區塊：4bit ，96~99
        /// </summary>
        [Description("資料偏移")]
        public string HeaderLength { get; set; }

        /// <summary>
        /// 【保留】第6區塊：3bit ，100~102
        /// </summary>
        [Description("保留")]
        public string Reserved { get; set; }

        /// <summary>
        /// 【ECN顯式擁塞通知】第7區塊：1bit， 103
        /// </summary>
        [Description("ECN顯式擁塞通知")]
        public string ExplicitCongestionNotificationFlag { get; set; }

        /// <summary>
        /// 【CWD減少視窗擁塞】第8區塊：1bit， 104
        /// </summary>
        [Description("CWD減少視窗擁塞")]
        public string CongestionWindowReduced { get; set; }

        /// <summary>
        /// 【ECN-Echo】第9區塊：1bit， 105
        /// </summary>
        [Description("ECN-Echo")]
        public string EcnEcho { get; set; }

        /// <summary>
        /// 【URG緊急】第10區塊：1bit， 106
        /// </summary>
        [Description("URG緊急")]
        public string Urgent { get; set; }

        /// <summary>
        /// 【ACK應答響應】第11區塊：1bit， 107
        /// </summary>
        [Description("ACK應答響應")]
        public string Acknowledgment { get; set; }
        /// <summary>
        /// 【PUS推送】第12區塊：1bit， 108
        /// </summary>
        [Description("PUS推送")]
        public string Push { get; set; }
        /// <summary>
        /// 【RST復位】第13區塊：1bit， 109
        /// </summary>
        [Description("RST復位")]
        public string Reset { get; set; }

        /// <summary>
        /// 【Syn同步】第14區塊：1bit， 110
        /// </summary>
        [Description("Syn同步")]
        public string Sync { get; set; }

        /// <summary>
        /// 【Fin結束】第15區塊：1bit， 111
        /// </summary>
        [Description("Fin結束")]
        public string Finish { get; set; }

        /// <summary>
        /// 【窗口大小】第16區塊：2byte， 112~127
        /// </summary>
        [Description("窗口大小")]
        public string Window { get; set; }

        /// <summary>
        /// 【校驗和】第17區塊：2Byte， 128~143
        /// </summary>
        [Description("校驗和")]
        public string Checksum { get; set; }
        /// <summary>
        /// 【緊急指標】第18區塊：2Byte， 144~159
        /// </summary>
        [Description("緊急指標")]
        public string UrgentPointer { get; set; }

        /// <summary>
        ///【資料】第19區塊：變動Bit 160~
        /// </summary>
        [Description("資料")]
        public string Data { get; set; }
    }
}
