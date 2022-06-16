namespace SnifferNetworkCard.ViewModel
{
    /// <summary>
    /// 呈現在ListView 的資料內容
    /// </summary>
    public class IPConnectionData
    {
        /// <summary>
        /// 連接該網路卡來源的IP
        /// </summary>
        public string SourceIP { get; set; }

        /// <summary>
        /// 每byte/s 的連接量
        /// </summary>
        public string flow { get; set; }

        /// <summary>
        /// 該IP的使用協定
        /// </summary>
        public string prototol { get; set; }
        /// <summary>
        /// 最後接收資料時間點
        /// </summary>
        public DateTime Receive_DateTime { get; set; }

    }
}
