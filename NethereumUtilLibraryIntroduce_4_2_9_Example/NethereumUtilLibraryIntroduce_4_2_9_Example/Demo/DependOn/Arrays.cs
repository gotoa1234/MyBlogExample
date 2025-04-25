using System.Text;

namespace NethereumUtilLibraryIntroduce_4_2_9_Example.Demo.DependOn
{
    // 3-4. Arrays
    /// <summary>
    /// 提供陣列操作的工具類別，包括陣列比較、複製、轉換等常用功能
    /// 此類別作為一個工具集，提供處理各種類型陣列的實用方法
    /// </summary>
    public abstract class Arrays
    {
        /// <summary>
        /// 比較兩個 boolean 陣列是否具有相同內容
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若兩陣列內容相同則返回 true；否則返回 false</returns>
        public static bool AreEqual(bool[] a, bool[] b)
        {
            if (a == b)  // 引用相同或都為 null
            {
                return true;
            }

            if (a == null || b == null)  // 其中一個為 null
            {
                return false;
            }

            return HaveSameContents(a, b);  // 檢查內容
        }

        /// <summary>
        /// 比較兩個字元陣列是否具有相同內容
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若兩陣列內容相同則返回 true；否則返回 false</returns>
        public static bool AreEqual(char[] a, char[] b)
        {
            if (a == b)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            return HaveSameContents(a, b);
        }

        /// <summary>
        /// 比較兩個位元組陣列是否具有相同內容
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若兩陣列內容相同則返回 true；否則返回 false</returns>
        public static bool AreEqual(byte[] a, byte[] b)
        {
            if (a == b)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            return HaveSameContents(a, b);
        }

        /// <summary>
        /// 比較兩個位元組陣列是否相同（已棄用，請使用 AreEqual 方法）
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若兩陣列相同則返回 true；否則返回 false</returns>
        [Obsolete("Use 'AreEqual' method instead")]
        public static bool AreSame(byte[] a, byte[] b)
        {
            return AreEqual(a, b);
        }

        /// <summary>
        /// 以恆定時間比較兩個位元組陣列，抵抗計時攻擊
        /// 即使在兩個陣列不同的情況下，也會比較所有元素，確保執行時間相同
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若兩陣列內容相同則返回 true；否則返回 false</returns>
        public static bool ConstantTimeAreEqual(byte[] a, byte[] b)
        {
            int num = a.Length;
            if (num != b.Length)  // 長度不同，直接返回 false
            {
                return false;
            }

            int num2 = 0;  // 用於累積差異的變數
            while (num != 0)
            {
                num--;
                num2 |= a[num] ^ b[num];  // 使用位元 XOR 運算檢測差異
            }

            return num2 == 0;  // 若所有位元都相同，結果為 0
        }

        /// <summary>
        /// 比較兩個整數陣列是否具有相同內容
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若兩陣列內容相同則返回 true；否則返回 false</returns>
        public static bool AreEqual(int[] a, int[] b)
        {
            if (a == b)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            return HaveSameContents(a, b);
        }

        /// <summary>
        /// 比較兩個無符號整數陣列是否具有相同內容
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若兩陣列內容相同則返回 true；否則返回 false</returns>
        public static bool AreEqual(uint[] a, uint[] b)
        {
            if (a == b)
            {
                return true;
            }

            if (a == null || b == null)
            {
                return false;
            }

            return HaveSameContents(a, b);
        }

        /// <summary>
        /// 比較兩個布林陣列的內容是否相同
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若內容相同則返回 true；否則返回 false</returns>
        private static bool HaveSameContents(bool[] a, bool[] b)
        {
            int num = a.Length;
            if (num != b.Length)  // 長度不同，返回 false
            {
                return false;
            }

            while (num != 0)
            {
                num--;
                if (a[num] != b[num])  // 比較每個元素
                {
                    return false;
                }
            }

            return true;  // 所有元素都相同
        }

        /// <summary>
        /// 比較兩個字元陣列的內容是否相同
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若內容相同則返回 true；否則返回 false</returns>
        private static bool HaveSameContents(char[] a, char[] b)
        {
            int num = a.Length;
            if (num != b.Length)
            {
                return false;
            }

            while (num != 0)
            {
                num--;
                if (a[num] != b[num])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 比較兩個位元組陣列的內容是否相同
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若內容相同則返回 true；否則返回 false</returns>
        private static bool HaveSameContents(byte[] a, byte[] b)
        {
            int num = a.Length;
            if (num != b.Length)
            {
                return false;
            }

            while (num != 0)
            {
                num--;
                if (a[num] != b[num])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 比較兩個整數陣列的內容是否相同
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若內容相同則返回 true；否則返回 false</returns>
        private static bool HaveSameContents(int[] a, int[] b)
        {
            int num = a.Length;
            if (num != b.Length)
            {
                return false;
            }

            while (num != 0)
            {
                num--;
                if (a[num] != b[num])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 比較兩個無符號整數陣列的內容是否相同
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>若內容相同則返回 true；否則返回 false</returns>
        private static bool HaveSameContents(uint[] a, uint[] b)
        {
            int num = a.Length;
            if (num != b.Length)
            {
                return false;
            }

            while (num != 0)
            {
                num--;
                if (a[num] != b[num])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 將物件陣列轉換為字串表示形式
        /// </summary>
        /// <param name="a">物件陣列</param>
        /// <returns>格式化後的字串表示</returns>
        public static string ToString(object[] a)
        {
            StringBuilder stringBuilder = new StringBuilder(91);
            if (a.Length != 0)
            {
                stringBuilder.Append(a[0]);
                for (int i = 1; i < a.Length; i++)
                {
                    stringBuilder.Append(", ").Append(a[i]);
                }
            }

            stringBuilder.Append(']');  // 注意：這裡假設前面有一個 '[' 字符
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 計算位元組陣列的雜湊碼
        /// </summary>
        /// <param name="data">位元組陣列</param>
        /// <returns>雜湊碼</returns>
        public static int GetHashCode(byte[] data)
        {
            if (data == null)
            {
                return 0;
            }

            int num = data.Length;
            int num2 = num + 1;  // 初始值包含長度資訊
            while (--num >= 0)
            {
                num2 *= 257;     // 使用質數乘法
                num2 ^= data[num];  // 使用 XOR 位運算
            }

            return num2;
        }

        /// <summary>
        /// 計算位元組陣列指定範圍的雜湊碼
        /// </summary>
        /// <param name="data">位元組陣列</param>
        /// <param name="off">起始偏移量</param>
        /// <param name="len">長度</param>
        /// <returns>雜湊碼</returns>
        public static int GetHashCode(byte[] data, int off, int len)
        {
            if (data == null)
            {
                return 0;
            }

            int num = len;
            int num2 = num + 1;
            while (--num >= 0)
            {
                num2 *= 257;
                num2 ^= data[off + num];
            }

            return num2;
        }

        /// <summary>
        /// 計算整數陣列的雜湊碼
        /// </summary>
        /// <param name="data">整數陣列</param>
        /// <returns>雜湊碼</returns>
        public static int GetHashCode(int[] data)
        {
            if (data == null)
            {
                return 0;
            }

            int num = data.Length;
            int num2 = num + 1;
            while (--num >= 0)
            {
                num2 *= 257;
                num2 ^= data[num];
            }

            return num2;
        }

        /// <summary>
        /// 計算整數陣列指定範圍的雜湊碼
        /// </summary>
        /// <param name="data">整數陣列</param>
        /// <param name="off">起始偏移量</param>
        /// <param name="len">長度</param>
        /// <returns>雜湊碼</returns>
        public static int GetHashCode(int[] data, int off, int len)
        {
            if (data == null)
            {
                return 0;
            }

            int num = len;
            int num2 = num + 1;
            while (--num >= 0)
            {
                num2 *= 257;
                num2 ^= data[off + num];
            }

            return num2;
        }

        /// <summary>
        /// 計算無符號整數陣列的雜湊碼
        /// </summary>
        /// <param name="data">無符號整數陣列</param>
        /// <returns>雜湊碼</returns>
        [CLSCompliant(false)]
        public static int GetHashCode(uint[] data)
        {
            if (data == null)
            {
                return 0;
            }

            int num = data.Length;
            int num2 = num + 1;
            while (--num >= 0)
            {
                num2 *= 257;
                num2 ^= (int)data[num];
            }

            return num2;
        }

        /// <summary>
        /// 計算無符號整數陣列指定範圍的雜湊碼
        /// </summary>
        /// <param name="data">無符號整數陣列</param>
        /// <param name="off">起始偏移量</param>
        /// <param name="len">長度</param>
        /// <returns>雜湊碼</returns>
        [CLSCompliant(false)]
        public static int GetHashCode(uint[] data, int off, int len)
        {
            if (data == null)
            {
                return 0;
            }

            int num = len;
            int num2 = num + 1;
            while (--num >= 0)
            {
                num2 *= 257;
                num2 ^= (int)data[off + num];
            }

            return num2;
        }

        /// <summary>
        /// 計算無符號長整數陣列的雜湊碼
        /// </summary>
        /// <param name="data">無符號長整數陣列</param>
        /// <returns>雜湊碼</returns>
        [CLSCompliant(false)]
        public static int GetHashCode(ulong[] data)
        {
            if (data == null)
            {
                return 0;
            }

            int num = data.Length;
            int num2 = num + 1;
            while (--num >= 0)
            {
                ulong num3 = data[num];
                num2 *= 257;
                num2 ^= (int)num3;        // 處理低 32 位
                num2 *= 257;
                num2 ^= (int)(num3 >> 32);  // 處理高 32 位
            }

            return num2;
        }

        /// <summary>
        /// 計算無符號長整數陣列指定範圍的雜湊碼
        /// </summary>
        /// <param name="data">無符號長整數陣列</param>
        /// <param name="off">起始偏移量</param>
        /// <param name="len">長度</param>
        /// <returns>雜湊碼</returns>
        [CLSCompliant(false)]
        public static int GetHashCode(ulong[] data, int off, int len)
        {
            if (data == null)
            {
                return 0;
            }

            int num = len;
            int num2 = num + 1;
            while (--num >= 0)
            {
                ulong num3 = data[off + num];
                num2 *= 257;
                num2 ^= (int)num3;
                num2 *= 257;
                num2 ^= (int)(num3 >> 32);
            }

            return num2;
        }

        /// <summary>
        /// 複製位元組陣列
        /// </summary>
        /// <param name="data">要複製的陣列</param>
        /// <returns>新的陣列副本，若輸入為 null 則返回 null</returns>
        public static byte[] Clone(byte[] data)
        {
            if (data != null)
            {
                return (byte[])data.Clone();
            }

            return null;
        }

        /// <summary>
        /// 複製位元組陣列到現有陣列
        /// </summary>
        /// <param name="data">要複製的源陣列</param>
        /// <param name="existing">目標陣列</param>
        /// <returns>複製後的陣列，若 existing 長度不符合則創建新陣列</returns>
        public static byte[] Clone(byte[] data, byte[] existing)
        {
            if (data == null)
            {
                return null;
            }

            if (existing == null || existing.Length != data.Length)
            {
                return Clone(data);
            }

            Array.Copy(data, 0, existing, 0, existing.Length);
            return existing;
        }

        /// <summary>
        /// 複製整數陣列
        /// </summary>
        /// <param name="data">要複製的陣列</param>
        /// <returns>新的陣列副本，若輸入為 null 則返回 null</returns>
        public static int[] Clone(int[] data)
        {
            if (data != null)
            {
                return (int[])data.Clone();
            }

            return null;
        }

        /// <summary>
        /// 複製無符號整數陣列
        /// </summary>
        /// <param name="data">要複製的陣列</param>
        /// <returns>新的陣列副本，若輸入為 null 則返回 null</returns>
        internal static uint[] Clone(uint[] data)
        {
            if (data != null)
            {
                return (uint[])data.Clone();
            }

            return null;
        }

        /// <summary>
        /// 複製長整數陣列
        /// </summary>
        /// <param name="data">要複製的陣列</param>
        /// <returns>新的陣列副本，若輸入為 null 則返回 null</returns>
        public static long[] Clone(long[] data)
        {
            if (data != null)
            {
                return (long[])data.Clone();
            }

            return null;
        }

        /// <summary>
        /// 複製無符號長整數陣列
        /// </summary>
        /// <param name="data">要複製的陣列</param>
        /// <returns>新的陣列副本，若輸入為 null 則返回 null</returns>
        [CLSCompliant(false)]
        public static ulong[] Clone(ulong[] data)
        {
            if (data != null)
            {
                return (ulong[])data.Clone();
            }

            return null;
        }

        /// <summary>
        /// 複製無符號長整數陣列到現有陣列
        /// </summary>
        /// <param name="data">要複製的源陣列</param>
        /// <param name="existing">目標陣列</param>
        /// <returns>複製後的陣列，若 existing 長度不符合則創建新陣列</returns>
        [CLSCompliant(false)]
        public static ulong[] Clone(ulong[] data, ulong[] existing)
        {
            if (data == null)
            {
                return null;
            }

            if (existing == null || existing.Length != data.Length)
            {
                return Clone(data);
            }

            Array.Copy(data, 0, existing, 0, existing.Length);
            return existing;
        }

        /// <summary>
        /// 檢查位元組陣列是否包含指定的位元組
        /// </summary>
        /// <param name="a">要搜尋的陣列</param>
        /// <param name="n">要尋找的位元組</param>
        /// <returns>若陣列包含指定位元組則返回 true；否則返回 false</returns>
        public static bool Contains(byte[] a, byte n)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == n)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 檢查短整數陣列是否包含指定的短整數
        /// </summary>
        /// <param name="a">要搜尋的陣列</param>
        /// <param name="n">要尋找的短整數</param>
        /// <returns>若陣列包含指定短整數則返回 true；否則返回 false</returns>
        public static bool Contains(short[] a, short n)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == n)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 檢查整數陣列是否包含指定的整數
        /// </summary>
        /// <param name="a">要搜尋的陣列</param>
        /// <param name="n">要尋找的整數</param>
        /// <returns>若陣列包含指定整數則返回 true；否則返回 false</returns>
        public static bool Contains(int[] a, int n)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == n)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 用指定的位元組填充整個陣列
        /// </summary>
        /// <param name="buf">要填充的陣列</param>
        /// <param name="b">填充值</param>
        public static void Fill(byte[] buf, byte b)
        {
            int num = buf.Length;
            while (num > 0)
            {
                buf[--num] = b;
            }
        }

        /// <summary>
        /// 以指定長度複製位元組陣列
        /// </summary>
        /// <param name="data">源陣列</param>
        /// <param name="newLength">新陣列的長度</param>
        /// <returns>指定長度的新陣列</returns>
        public static byte[] CopyOf(byte[] data, int newLength)
        {
            byte[] array = new byte[newLength];
            Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
            return array;
        }

        /// <summary>
        /// 以指定長度複製字元陣列
        /// </summary>
        /// <param name="data">源陣列</param>
        /// <param name="newLength">新陣列的長度</param>
        /// <returns>指定長度的新陣列</returns>
        public static char[] CopyOf(char[] data, int newLength)
        {
            char[] array = new char[newLength];
            Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
            return array;
        }

        /// <summary>
        /// 以指定長度複製整數陣列
        /// </summary>
        /// <param name="data">源陣列</param>
        /// <param name="newLength">新陣列的長度</param>
        /// <returns>指定長度的新陣列</returns>
        public static int[] CopyOf(int[] data, int newLength)
        {
            int[] array = new int[newLength];
            Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
            return array;
        }

        /// <summary>
        /// 以指定長度複製長整數陣列
        /// </summary>
        /// <param name="data">源陣列</param>
        /// <param name="newLength">新陣列的長度</param>
        /// <returns>指定長度的新陣列</returns>
        public static long[] CopyOf(long[] data, int newLength)
        {
            long[] array = new long[newLength];
            Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
            return array;
        }

        /// <summary>
        /// 複製位元組陣列的指定範圍
        /// </summary>
        /// <param name="data">源陣列</param>
        /// <param name="from">起始索引（包含）</param>
        /// <param name="to">結束索引（不包含）</param>
        /// <returns>包含指定範圍元素的新陣列</returns>
        public static byte[] CopyOfRange(byte[] data, int from, int to)
        {
            int length = GetLength(from, to);
            byte[] array = new byte[length];
            Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
            return array;
        }

        /// <summary>
        /// 複製整數陣列的指定範圍
        /// </summary>
        /// <param name="data">源陣列</param>
        /// <param name="from">起始索引（包含）</param>
        /// <param name="to">結束索引（不包含）</param>
        /// <returns>包含指定範圍元素的新陣列</returns>
        public static int[] CopyOfRange(int[] data, int from, int to)
        {
            int length = GetLength(from, to);
            int[] array = new int[length];
            Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
            return array;
        }

        /// <summary>
        /// 複製長整數陣列的指定範圍
        /// </summary>
        /// <param name="data">源陣列</param>
        /// <param name="from">起始索引（包含）</param>
        /// <param name="to">結束索引（不包含）</param>
        /// <returns>包含指定範圍元素的新陣列</returns>
        public static long[] CopyOfRange(long[] data, int from, int to)
        {
            int length = GetLength(from, to);
            long[] array = new long[length];
            Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
            return array;
        }

        /// <summary>
        /// 計算範圍長度並驗證範圍的有效性
        /// </summary>
        /// <param name="from">起始索引</param>
        /// <param name="to">結束索引</param>
        /// <returns>範圍長度</returns>
        /// <exception cref="ArgumentException">如果範圍無效（起始索引大於結束索引）</exception>
        private static int GetLength(int from, int to)
        {
            int num = to - from;
            if (num < 0)
            {
                throw new ArgumentException(from + " > " + to);
            }

            return num;
        }

        /// <summary>
        /// 將一個位元組附加到位元組陣列尾部
        /// </summary>
        /// <param name="a">原陣列</param>
        /// <param name="b">要附加的位元組</param>
        /// <returns>附加後的新陣列</returns>
        public static byte[] Append(byte[] a, byte b)
        {
            if (a == null)
            {
                return new byte[1] { b };  // 如果原陣列為 null，創建只包含 b 的新陣列
            }

            int num = a.Length;
            byte[] array = new byte[num + 1];
            Array.Copy(a, 0, array, 0, num);
            array[num] = b;
            return array;
        }

        /// <summary>
        /// 將一個短整數附加到短整數陣列尾部
        /// </summary>
        /// <param name="a">原陣列</param>
        /// <param name="b">要附加的短整數</param>
        /// <returns>附加後的新陣列</returns>
        public static short[] Append(short[] a, short b)
        {
            if (a == null)
            {
                return new short[1] { b };
            }

            int num = a.Length;
            short[] array = new short[num + 1];
            Array.Copy(a, 0, array, 0, num);
            array[num] = b;
            return array;
        }

        /// <summary>
        /// 將一個整數附加到整數陣列尾部
        /// </summary>
        /// <param name="a">原陣列</param>
        /// <param name="b">要附加的整數</param>
        /// <returns>附加後的新陣列</returns>
        public static int[] Append(int[] a, int b)
        {
            if (a == null)
            {
                return new int[1] { b };
            }

            int num = a.Length;
            int[] array = new int[num + 1];
            Array.Copy(a, 0, array, 0, num);
            array[num] = b;
            return array;
        }

        /// <summary>
        /// 連接兩個位元組陣列
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>連接後的新陣列，若其中一個為 null，則返回另一個的副本</returns>
        public static byte[] Concatenate(byte[] a, byte[] b)
        {
            if (a == null)
            {
                return Clone(b);
            }

            if (b == null)
            {
                return Clone(a);
            }

            byte[] array = new byte[a.Length + b.Length];
            Array.Copy(a, 0, array, 0, a.Length);
            Array.Copy(b, 0, array, a.Length, b.Length);
            return array;
        }

        /// <summary>
        /// 連接多個位元組陣列
        /// </summary>
        /// <param name="vs">要連接的位元組陣列集合</param>
        /// <returns>連接後的新陣列，忽略所有 null 陣列</returns>
        public static byte[] ConcatenateAll(params byte[][] vs)
        {
            byte[][] array = new byte[vs.Length][];
            int num = 0;  // 非 null 陣列的數量
            int num2 = 0;  // 總位元組數

            // 統計非 null 陣列及總長度
            foreach (byte[] array2 in vs)
            {
                if (array2 != null)
                {
                    array[num++] = array2;
                    num2 += array2.Length;
                }
            }

            // 創建結果陣列並複製資料
            byte[] array3 = new byte[num2];
            int num3 = 0;
            for (int j = 0; j < num; j++)
            {
                byte[] array4 = array[j];
                Array.Copy(array4, 0, array3, num3, array4.Length);
                num3 += array4.Length;
            }

            return array3;
        }

        /// <summary>
        /// 連接兩個整數陣列
        /// </summary>
        /// <param name="a">第一個陣列</param>
        /// <param name="b">第二個陣列</param>
        /// <returns>連接後的新陣列，若其中一個為 null，則返回另一個的副本</returns>
        public static int[] Concatenate(int[] a, int[] b)
        {
            if (a == null)
            {
                return Clone(b);
            }

            if (b == null)
            {
                return Clone(a);
            }

            int[] array = new int[a.Length + b.Length];
            Array.Copy(a, 0, array, 0, a.Length);
            Array.Copy(b, 0, array, a.Length, b.Length);
            return array;
        }

        /// <summary>
        /// 在位元組陣列前面添加一個位元組
        /// </summary>
        /// <param name="a">原陣列</param>
        /// <param name="b">要添加的位元組</param>
        /// <returns>添加後的新陣列</returns>
        public static byte[] Prepend(byte[] a, byte b)
        {
            if (a == null)
            {
                return new byte[1] { b };
            }

            int num = a.Length;
            byte[] array = new byte[num + 1];
            Array.Copy(a, 0, array, 1, num);
            array[0] = b;
            return array;
        }

        /// <summary>
        /// 在短整數陣列前面添加一個短整數
        /// </summary>
        /// <param name="a">原陣列</param>
        /// <param name="b">要添加的短整數</param>
        /// <returns>添加後的新陣列</returns>
        public static short[] Prepend(short[] a, short b)
        {
            if (a == null)
            {
                return new short[1] { b };
            }

            int num = a.Length;
            short[] array = new short[num + 1];
            Array.Copy(a, 0, array, 1, num);
            array[0] = b;
            return array;
        }

        /// <summary>
        /// 在整數陣列前面添加一個整數
        /// </summary>
        /// <param name="a">原陣列</param>
        /// <param name="b">要添加的整數</param>
        /// <returns>添加後的新陣列</returns>
        public static int[] Prepend(int[] a, int b)
        {
            if (a == null)
            {
                return new int[1] { b };
            }

            int num = a.Length;
            int[] array = new int[num + 1];
            Array.Copy(a, 0, array, 1, num);
            array[0] = b;
            return array;
        }

        /// <summary>
        /// 反轉位元組陣列的元素順序
        /// </summary>
        /// <param name="a">要反轉的陣列</param>
        /// <returns>反轉後的新陣列，若輸入為 null 則返回 null</returns>
        public static byte[] Reverse(byte[] a)
        {
            if (a == null)
            {
                return null;
            }

            int num = 0;
            int num2 = a.Length;
            byte[] array = new byte[num2];
            while (--num2 >= 0)
            {
                array[num2] = a[num++];  // 將源陣列頭部元素放入目標陣列尾部
            }

            return array;
        }

        /// <summary>
        /// 反轉整數陣列的元素順序
        /// </summary>
        /// <param name="a">要反轉的陣列</param>
        /// <returns>反轉後的新陣列，若輸入為 null 則返回 null</returns>
        public static int[] Reverse(int[] a)
        {
            if (a == null)
            {
                return null;
            }

            int num = 0;
            int num2 = a.Length;
            int[] array = new int[num2];
            while (--num2 >= 0)
            {
                array[num2] = a[num++];
            }

            return array;
        }
    }
}
