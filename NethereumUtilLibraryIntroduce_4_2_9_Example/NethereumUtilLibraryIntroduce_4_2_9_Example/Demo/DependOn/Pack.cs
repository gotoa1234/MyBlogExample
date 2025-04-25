using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NethereumUtilLibraryIntroduce_4_2_9_Example.Demo.DependOn
{
    // 3-1. Pack
    /// <summary>
    /// Pack 類別用於處理整數與位元組陣列之間的轉換，支援大端序(BE)與小端序(LE)格式
    /// 大端序：高位元組存放在低位記憶體地址中 (網路傳輸常用)
    /// 小端序：低位元組存放在低位記憶體地址中 (大多數電腦架構使用)
    /// </summary>
    public sealed class Pack
    {
        /// <summary>
        /// 私有建構函式，防止類別被實例化，僅提供靜態方法使用
        /// </summary>
        private Pack()
        {
        }

        // ===================== 大端序 (Big-Endian, BE) 轉換方法 =====================

        /// <summary>
        /// 將 16位元無符號整數 轉換為 大端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 16 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        internal static void UInt16_To_BE(ushort n, byte[] bs)
        {
            bs[0] = (byte)(n >> 8);   // 高 8 位放在位址低位
            bs[1] = (byte)n;          // 低 8 位放在位址高位
        }

        /// <summary>
        /// 將 16位元無符號整數 轉換為 大端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="n">要轉換的 16 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt16_To_BE(ushort n, byte[] bs, int off)
        {
            bs[off] = (byte)(n >> 8);     // 高 8 位
            bs[off + 1] = (byte)n;        // 低 8 位
        }

        /// <summary>
        /// 將大端序位元組陣列轉換為 16位元無符號整數
        /// </summary>
        /// <param name="bs">大端序格式的位元組陣列</param>
        /// <returns>轉換後的 16 位元整數</returns>
        internal static ushort BE_To_UInt16(byte[] bs)
        {
            return (ushort)((bs[0] << 8) | bs[1]);  // 第一個位元組左移 8 位後與第二個位元組進行位或運算
        }

        /// <summary>
        /// 從大端序位元組陣列的指定偏移位置轉換 16位元無符號整數
        /// </summary>
        /// <param name="bs">大端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <returns>轉換後的 16 位元整數</returns>
        internal static ushort BE_To_UInt16(byte[] bs, int off)
        {
            return (ushort)((bs[off] << 8) | bs[off + 1]);
        }

        /// <summary>
        /// 將 32位元無符號整數 轉換為 大端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 32 位元整數</param>
        /// <returns>含有轉換結果的新位元組陣列</returns>
        internal static byte[] UInt32_To_BE(uint n)
        {
            byte[] array = new byte[4];      // 建立一個 4 位元組的陣列
            UInt32_To_BE(n, array, 0);       // 調用重載方法執行轉換
            return array;
        }

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

        /// <summary>
        /// 將 32位元無符號整數 轉換為 大端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="n">要轉換的 32 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt32_To_BE(uint n, byte[] bs, int off)
        {
            bs[off] = (byte)(n >> 24);      // 取最高 8 位
            bs[off + 1] = (byte)(n >> 16);  // 取次高 8 位
            bs[off + 2] = (byte)(n >> 8);   // 取次低 8 位
            bs[off + 3] = (byte)n;          // 取最低 8 位
        }

        /// <summary>
        /// 將 32位元無符號整數陣列 轉換為 大端序位元組陣列
        /// </summary>
        /// <param name="ns">要轉換的 32 位元整數陣列</param>
        /// <returns>含有轉換結果的新位元組陣列</returns>
        internal static byte[] UInt32_To_BE(uint[] ns)
        {
            byte[] array = new byte[4 * ns.Length];  // 每個 uint 佔 4 位元組
            UInt32_To_BE(ns, array, 0);              // 調用重載方法執行轉換
            return array;
        }

        /// <summary>
        /// 將 32位元無符號整數陣列 轉換為 大端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="ns">要轉換的 32 位元整數陣列</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt32_To_BE(uint[] ns, byte[] bs, int off)
        {
            for (int i = 0; i < ns.Length; i++)
            {
                UInt32_To_BE(ns[i], bs, off);  // 逐個轉換並寫入
                off += 4;                       // 每個 uint 後移 4 位元組
            }
        }

        /// <summary>
        /// 將大端序位元組陣列轉換為 32位元無符號整數
        /// </summary>
        /// <param name="bs">大端序格式的位元組陣列</param>
        /// <returns>轉換後的 32 位元整數</returns>
        internal static uint BE_To_UInt32(byte[] bs)
        {
            // 將四個位元組分別左移相應位數後進行位或運算
            return (uint)((bs[0] << 24) | (bs[1] << 16) | (bs[2] << 8) | bs[3]);
        }

        /// <summary>
        /// 從大端序位元組陣列的指定偏移位置轉換 32位元無符號整數
        /// </summary>
        /// <param name="bs">大端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <returns>轉換後的 32 位元整數</returns>
        internal static uint BE_To_UInt32(byte[] bs, int off)
        {
            return (uint)((bs[off] << 24) | (bs[off + 1] << 16) |
                          (bs[off + 2] << 8) | bs[off + 3]);
        }

        /// <summary>
        /// 將大端序位元組陣列轉換為 32位元無符號整數陣列
        /// </summary>
        /// <param name="bs">大端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <param name="ns">存放轉換結果的 32 位元整數陣列</param>
        internal static void BE_To_UInt32(byte[] bs, int off, uint[] ns)
        {
            for (int i = 0; i < ns.Length; i++)
            {
                ns[i] = BE_To_UInt32(bs, off);  // 逐個轉換並儲存
                off += 4;                        // 每個 uint 佔 4 位元組
            }
        }

        /// <summary>
        /// 將 64位元無符號整數 轉換為 大端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 64 位元整數</param>
        /// <returns>含有轉換結果的新位元組陣列</returns>
        internal static byte[] UInt64_To_BE(ulong n)
        {
            byte[] array = new byte[8];      // 建立一個 8 位元組的陣列
            UInt64_To_BE(n, array, 0);       // 調用重載方法執行轉換
            return array;
        }

        /// <summary>
        /// 將 64位元無符號整數 轉換為 大端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 64 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        internal static void UInt64_To_BE(ulong n, byte[] bs)
        {
            // 將 64 位分為高 32 位和低 32 位分別處理
            UInt32_To_BE((uint)(n >> 32), bs);     // 高 32 位放在前 4 個位元組
            UInt32_To_BE((uint)n, bs, 4);          // 低 32 位放在後 4 個位元組
        }

        /// <summary>
        /// 將 64位元無符號整數 轉換為 大端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="n">要轉換的 64 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt64_To_BE(ulong n, byte[] bs, int off)
        {
            UInt32_To_BE((uint)(n >> 32), bs, off);     // 高 32 位
            UInt32_To_BE((uint)n, bs, off + 4);         // 低 32 位
        }

        /// <summary>
        /// 將 64位元無符號整數陣列 轉換為 大端序位元組陣列
        /// </summary>
        /// <param name="ns">要轉換的 64 位元整數陣列</param>
        /// <returns>含有轉換結果的新位元組陣列</returns>
        internal static byte[] UInt64_To_BE(ulong[] ns)
        {
            byte[] array = new byte[8 * ns.Length];  // 每個 ulong 佔 8 位元組
            UInt64_To_BE(ns, array, 0);              // 調用重載方法執行轉換
            return array;
        }

        /// <summary>
        /// 將 64位元無符號整數陣列 轉換為 大端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="ns">要轉換的 64 位元整數陣列</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt64_To_BE(ulong[] ns, byte[] bs, int off)
        {
            for (int i = 0; i < ns.Length; i++)
            {
                UInt64_To_BE(ns[i], bs, off);  // 逐個轉換並寫入
                off += 8;                       // 每個 ulong 後移 8 位元組
            }
        }

        /// <summary>
        /// 將大端序位元組陣列轉換為 64位元無符號整數
        /// </summary>
        /// <param name="bs">大端序格式的位元組陣列</param>
        /// <returns>轉換後的 64 位元整數</returns>
        internal static ulong BE_To_UInt64(byte[] bs)
        {
            // 先分別轉換高 32 位和低 32 位
            uint num = BE_To_UInt32(bs);           // 高 32 位
            uint num2 = BE_To_UInt32(bs, 4);       // 低 32 位
            return ((ulong)num << 32) | num2;      // 合併為 64 位
        }

        /// <summary>
        /// 從大端序位元組陣列的指定偏移位置轉換 64位元無符號整數
        /// </summary>
        /// <param name="bs">大端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <returns>轉換後的 64 位元整數</returns>
        internal static ulong BE_To_UInt64(byte[] bs, int off)
        {
            uint num = BE_To_UInt32(bs, off);          // 高 32 位
            uint num2 = BE_To_UInt32(bs, off + 4);     // 低 32 位
            return ((ulong)num << 32) | num2;          // 合併為 64 位
        }

        /// <summary>
        /// 將大端序位元組陣列轉換為 64位元無符號整數陣列
        /// </summary>
        /// <param name="bs">大端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <param name="ns">存放轉換結果的 64 位元整數陣列</param>
        internal static void BE_To_UInt64(byte[] bs, int off, ulong[] ns)
        {
            for (int i = 0; i < ns.Length; i++)
            {
                ns[i] = BE_To_UInt64(bs, off);  // 逐個轉換並儲存
                off += 8;                        // 每個 ulong 佔 8 位元組
            }
        }

        // ===================== 小端序 (Little-Endian, LE) 轉換方法 =====================

        /// <summary>
        /// 將 16位元無符號整數 轉換為 小端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 16 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        internal static void UInt16_To_LE(ushort n, byte[] bs)
        {
            bs[0] = (byte)n;          // 低 8 位放在位址低位
            bs[1] = (byte)(n >> 8);   // 高 8 位放在位址高位
        }

        /// <summary>
        /// 將 16位元無符號整數 轉換為 小端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="n">要轉換的 16 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt16_To_LE(ushort n, byte[] bs, int off)
        {
            bs[off] = (byte)n;          // 低 8 位
            bs[off + 1] = (byte)(n >> 8);   // 高 8 位
        }

        /// <summary>
        /// 將小端序位元組陣列轉換為 16位元無符號整數
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <returns>轉換後的 16 位元整數</returns>
        internal static ushort LE_To_UInt16(byte[] bs)
        {
            return (ushort)(bs[0] | (bs[1] << 8));  // 第二個位元組左移 8 位後與第一個位元組進行位或運算
        }

        /// <summary>
        /// 從小端序位元組陣列的指定偏移位置轉換 16位元無符號整數
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <returns>轉換後的 16 位元整數</returns>
        internal static ushort LE_To_UInt16(byte[] bs, int off)
        {
            return (ushort)(bs[off] | (bs[off + 1] << 8));
        }

        /// <summary>
        /// 將 32位元無符號整數 轉換為 小端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 32 位元整數</param>
        /// <returns>含有轉換結果的新位元組陣列</returns>
        internal static byte[] UInt32_To_LE(uint n)
        {
            byte[] array = new byte[4];      // 建立一個 4 位元組的陣列
            UInt32_To_LE(n, array, 0);       // 調用重載方法執行轉換
            return array;
        }

        /// <summary>
        /// 將 32位元無符號整數 轉換為 小端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 32 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        internal static void UInt32_To_LE(uint n, byte[] bs)
        {
            bs[0] = (byte)n;          // 取最低 8 位 (位元組 0)
            bs[1] = (byte)(n >> 8);   // 取次低 8 位 (位元組 1)
            bs[2] = (byte)(n >> 16);  // 取次高 8 位 (位元組 2)
            bs[3] = (byte)(n >> 24);  // 取最高 8 位 (位元組 3)
        }

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
        /// 將 32位元無符號整數陣列 轉換為 小端序位元組陣列
        /// </summary>
        /// <param name="ns">要轉換的 32 位元整數陣列</param>
        /// <returns>含有轉換結果的新位元組陣列</returns>
        internal static byte[] UInt32_To_LE(uint[] ns)
        {
            byte[] array = new byte[4 * ns.Length];  // 每個 uint 佔 4 位元組
            UInt32_To_LE(ns, array, 0);              // 調用重載方法執行轉換
            return array;
        }

        /// <summary>
        /// 將 32位元無符號整數陣列 轉換為 小端序位元組陣列，並寫入指定偏移位置
        /// </summary>
        /// <param name="ns">要轉換的 32 位元整數陣列</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        /// <param name="off">寫入的起始位置偏移量</param>
        internal static void UInt32_To_LE(uint[] ns, byte[] bs, int off)
        {
            for (int i = 0; i < ns.Length; i++)
            {
                UInt32_To_LE(ns[i], bs, off);  // 逐個轉換並寫入
                off += 4;                       // 每個 uint 後移 4 位元組
            }
        }

        /// <summary>
        /// 將小端序位元組陣列轉換為 32位元無符號整數
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <returns>轉換後的 32 位元整數</returns>
        internal static uint LE_To_UInt32(byte[] bs)
        {
            // 小端序：低位元組在前，高位元組在後
            return (uint)(bs[0] | (bs[1] << 8) | (bs[2] << 16) | (bs[3] << 24));
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
        /// 將小端序位元組陣列轉換為 32位元無符號整數陣列
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <param name="ns">存放轉換結果的 32 位元整數陣列</param>
        internal static void LE_To_UInt32(byte[] bs, int off, uint[] ns)
        {
            for (int i = 0; i < ns.Length; i++)
            {
                ns[i] = LE_To_UInt32(bs, off);  // 逐個轉換並儲存
                off += 4;                        // 每個 uint 佔 4 位元組
            }
        }

        /// <summary>
        /// 將小端序位元組陣列轉換為指定數量的 32位元無符號整數，並存入陣列的指定偏移位置
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <param name="bOff">位元組陣列的起始讀取位置</param>
        /// <param name="ns">存放轉換結果的 32 位元整數陣列</param>
        /// <param name="nOff">整數陣列的起始寫入位置</param>
        /// <param name="count">要轉換的整數數量</param>
        internal static void LE_To_UInt32(byte[] bs, int bOff, uint[] ns, int nOff, int count)
        {
            for (int i = 0; i < count; i++)
            {
                ns[nOff + i] = LE_To_UInt32(bs, bOff);  // 逐個轉換並儲存到指定位置
                bOff += 4;                              // 每個 uint 佔 4 位元組
            }
        }

        /// <summary>
        /// 將小端序位元組陣列轉換為指定數量的 32位元無符號整數陣列
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <param name="count">要轉換的整數數量</param>
        /// <returns>包含轉換結果的新 32 位元整數陣列</returns>
        internal static uint[] LE_To_UInt32(byte[] bs, int off, int count)
        {
            uint[] array = new uint[count];  // 創建指定大小的整數陣列
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = LE_To_UInt32(bs, off);  // 逐個轉換並儲存
                off += 4;                          // 每個 uint 佔 4 位元組
            }

            return array;
        }

        /// <summary>
        /// 將 64位元無符號整數 轉換為 小端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 64 位元整數</param>
        /// <returns>含有轉換結果的新位元組陣列</returns>
        internal static byte[] UInt64_To_LE(ulong n)
        {
            byte[] array = new byte[8];      // 建立一個 8 位元組的陣列
            UInt64_To_LE(n, array, 0);       // 調用重載方法執行轉換
            return array;
        }

        /// <summary>
        /// 將 64位元無符號整數 轉換為 小端序位元組陣列
        /// </summary>
        /// <param name="n">要轉換的 64 位元整數</param>
        /// <param name="bs">用於存儲結果的位元組陣列</param>
        internal static void UInt64_To_LE(ulong n, byte[] bs)
        {
            // 小端序：低位在前，高位在後
            UInt32_To_LE((uint)n, bs);            // 低 32 位放在前 4 個位元組
            UInt32_To_LE((uint)(n >> 32), bs, 4); // 高 32 位放在後 4 個位元組
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
        /// 將 64位元無符號整數陣列 轉換為 小端序位元組陣列
        /// </summary>
        /// <param name="ns">要轉換的 64 位元整數陣列</param>
        /// <returns>含有轉換結果的新位元組陣列</returns>
        internal static byte[] UInt64_To_LE(ulong[] ns)
        {
            byte[] array = new byte[8 * ns.Length];  // 每個 ulong 佔 8 位元組
            UInt64_To_LE(ns, array, 0);              // 調用重載方法執行轉換
            return array;
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
        /// 將小端序位元組陣列轉換為 64位元無符號整數
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <returns>轉換後的 64 位元整數</returns>
        internal static ulong LE_To_UInt64(byte[] bs)
        {
            // 小端序：低位在前，高位在後
            uint num = LE_To_UInt32(bs);                    // 取低 32 位
            return ((ulong)LE_To_UInt32(bs, 4) << 32) | num; // 高 32 位左移後與低 32 位合併
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

        /// <summary>
        /// 將小端序位元組陣列轉換為 64位元無符號整數陣列
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <param name="off">讀取的起始位置偏移量</param>
        /// <param name="ns">存放轉換結果的 64 位元整數陣列</param>
        internal static void LE_To_UInt64(byte[] bs, int off, ulong[] ns)
        {
            for (int i = 0; i < ns.Length; i++)
            {
                ns[i] = LE_To_UInt64(bs, off);  // 逐個轉換並儲存
                off += 8;                        // 每個 ulong 佔 8 位元組
            }
        }

        /// <summary>
        /// 將小端序位元組陣列轉換為指定數量的 64位元無符號整數，並存入陣列的指定偏移位置
        /// </summary>
        /// <param name="bs">小端序格式的位元組陣列</param>
        /// <param name="bsOff">位元組陣列的起始讀取位置</param>
        /// <param name="ns">存放轉換結果的 64 位元整數陣列</param>
        /// <param name="nsOff">整數陣列的起始寫入位置</param>
        /// <param name="nsLen">要轉換的整數數量</param>
        internal static void LE_To_UInt64(byte[] bs, int bsOff, ulong[] ns, int nsOff, int nsLen)
        {
            for (int i = 0; i < nsLen; i++)
            {
                ns[nsOff + i] = LE_To_UInt64(bs, bsOff);  // 逐個轉換並儲存到指定位置
                bsOff += 8;                               // 每個 ulong 佔 8 位元組
            }
        }
    }

}
