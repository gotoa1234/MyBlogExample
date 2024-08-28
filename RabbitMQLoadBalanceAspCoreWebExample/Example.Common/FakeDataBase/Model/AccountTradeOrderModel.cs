namespace Example.Common.FakeDataBase.Model
{
    public class AccountTradeOrderModel
    {
        /// <summary>
        /// 帳戶訂單編號
        /// </summary>
        public int AccountTradeOrderId { get; set; }

        /// <summary>
        /// 帳戶名稱
        /// </summary>
        public string AccountName { get; set; } = string.Empty;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccessful { get; set; } = false;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime DateTimeValue { get; set;}

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 機器名稱 - 消費者處理時才寫入
        /// </summary>
        public string MechineName { get; set; } = string.Empty;
    }
}
