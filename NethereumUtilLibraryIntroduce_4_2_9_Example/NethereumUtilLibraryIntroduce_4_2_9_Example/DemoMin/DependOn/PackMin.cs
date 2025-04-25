namespace NethereumUtilLibraryIntroduce_4_2_9_Example.DemoMin.DependOn
{
    public sealed class PackMin
    {
        /// <summary>
        /// 私有建構函式，防止類別被實例化，僅提供靜態方法使用
        /// </summary>
        private PackMin()
        {
        }

        // ===================== 大端序 (Big-Endian, BE) 轉換方法 =====================

        /// <summary>
        /// 將 32位元無符號整數 轉換為 大端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 32 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        internal static void UInt32_To_BE(uint n, byte[] bs)
        {
            bs[0] = (byte)(n >> 24);  // 取最高 8 位 (位元組 3)
            bs[1] = (byte)(n >> 16);  // 取次高 8 位 (位元組 2)
            bs[2] = (byte)(n >> 8);   // 取次低 8 位 (位元組 1)
            bs[3] = (byte)n;          // 取最低 8 位 (位元組 0)
        }
      
        // ===================== 小端序 (Little-Endian, LE) 轉換方法 =====================

        /// <summary>
        /// 將 32位元無符號整數 轉換為 小端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="n">要轉換的 32 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt32_To_LE(uint n, byte[] bs, int off)
        {
            bs[off] = (byte)n;              // 取最低 8 位
            bs[off + 1] = (byte)(n >> 8);   // 取次低 8 位
            bs[off + 2] = (byte)(n >> 16);  // 取次高 8 位
            bs[off + 3] = (byte)(n >> 24);  // 取最高 8 位
        }


        /// <summary>
        /// 從小端序位元組陣列的指定偏移位置轉換 32位元無符號整數
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <returns>轉換後的 32 位元整數</returns>
        internal static uint LE_To_UInt32(byte[] bs, int off)
        {
            return (uint)(bs[off] | (bs[off + 1] << 8) |
                         (bs[off + 2] << 16) | (bs[off + 3] << 24));
        }

        /// <summary>
        /// 將 64位元無符號整數 轉換為 小端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="n">要轉換的 64 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt64_To_LE(ulong n, byte[] bs, int off)
        {
            UInt32_To_LE((uint)n, bs, off);            // 低 32 位
            UInt32_To_LE((uint)(n >> 32), bs, off + 4); // 高 32 位
        }

        /// <summary>
        /// 將 64位元無符號整數陣列 轉換為 小端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="ns">要轉換的 64 位元整數陣列</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt64_To_LE(ulong[] ns, byte[] bs, int off)
        {
            for (int i = 0; i < ns.Length; i++)
            {
                UInt64_To_LE(ns[i], bs, off);  // 逐個轉換並寫入
                off += 8;                       // 每個 ulong 後移 8 位元組
            }
        }

        /// <summary>
        /// 將 64位元無符號整數陣列的指定部分 轉換為 小端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="ns">要轉換的 64 位元整數陣列</param>
        /// <param name="nsOff">整數陣列的起始位置</param>
        /// <param name="nsLen">要轉換的整數數量</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="bsOff">位元組陣列的起始寫入位置</param>
        internal static void UInt64_To_LE(ulong[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
        {
            for (int i = 0; i < nsLen; i++)
            {
                UInt64_To_LE(ns[nsOff + i], bs, bsOff);  // 轉換指定位置的整數並寫入
                bsOff += 8;                              // 每個 ulong 後移 8 位元組
            }
        }

        /// <summary>
        /// 從小端序位元組陣列的指定偏移位置轉換 64位元無符號整數
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <returns>轉換後的 64 位元整數</returns>
        internal static ulong LE_To_UInt64(byte[] bs, int off)
        {
            uint num = LE_To_UInt32(bs, off);                    // 取低 32 位
            return ((ulong)LE_To_UInt32(bs, off + 4) << 32) | num; // 高 32 位左移後與低 32 位合併
        }
    }

}
