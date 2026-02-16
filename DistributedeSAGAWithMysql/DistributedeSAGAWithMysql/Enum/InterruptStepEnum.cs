namespace DistributedeSAGAWithMysql.Enum
{
    public enum InterruptStepEnum
    {
        /// <summary>
        /// 完整走完
        /// </summary>
        None = 0,

        /// <summary>
        /// 第一步執行前中斷 (未執行)
        /// </summary>
        InterruptStep1 = 1,

        /// <summary>
        /// 第二步執行前中斷 (寫入 Log 結束)
        /// </summary>
        InterruptStep2 = 2,

        /// <summary>
        /// 第三步執行前中斷 (寫入 Log + 扣款 Balance 表 結束)
        /// </summary>
        InterruptStep3 = 3,

        /// <summary>
        /// 第四步執行前中斷 (寫入 Log + 扣款 Balance 表 + 本地 Member 表 結束)
        /// </summary>
        InterruptStep4 = 4,
    }
}
