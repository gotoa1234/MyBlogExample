namespace NethereumUtilLibraryIntroduce_4_2_9_Example.Demo.DependOn
{
    // 3-3. HexByteConvertorExtensions
    /// <summary>
    /// 提供十六進制字串與位元組陣列之間轉換的擴展方法集合
    /// 這些方法主要用於加密貨幣相關操作中的資料轉換處理
    /// </summary>
    public static class HexByteConvertorExtensions
    {
        /// <summary>
        /// 表示空位元組陣列的常數
        /// </summary>
        private static readonly byte[] Empty = new byte[0];

        /// <summary>
        /// 將位元組陣列轉換為十六進制字串
        /// </summary>
        /// <param name="value">要轉換的位元組陣列</param>
        /// <param name="prefix">是否包含 "0x" 前綴，預設為 false</param>
        /// <returns>十六進制字串表示形式</returns>
        public static string ToHex(this byte[] value, bool prefix = false)
        {
            // 將每個位元組轉換為兩位十六進制數，並連接起來
            // 如需前綴，則添加 "0x"
            return string.Concat(prefix ? "0x" : "", string.Concat(value.Select((byte b) => b.ToString("x2")).ToArray()));
        }

        /// <summary>
        /// 檢查字串是否具有十六進制前綴 "0x"
        /// </summary>
        /// <param name="value">要檢查的字串</param>
        /// <returns>如果字串以 "0x" 開頭則返回 true，否則返回 false</returns>
        public static bool HasHexPrefix(this string value)
        {
            return value.StartsWith("0x");
        }

        /// <summary>
        /// 檢查字串是否為有效的十六進制字串
        /// </summary>
        /// <param name="value">要檢查的字串</param>
        /// <returns>如果字串只包含十六進制字符則返回 true，否則返回 false</returns>
        public static bool IsHex(this string value)
        {
            // 首先移除可能存在的 "0x" 前綴
            string text = value.RemoveHexPrefix();

            // 檢查每個字符是否為有效的十六進制字符 (0-9, a-f, A-F)
            foreach (char c in text)
            {
                if ((c < '0' || c > '9') && (c < 'a' || c > 'f') && (c < 'A' || c > 'F'))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 從字串中移除十六進制前綴 "0x"（如果存在）
        /// </summary>
        /// <param name="value">可能包含十六進制前綴的字串</param>
        /// <returns>移除前綴後的字串</returns>
        public static string RemoveHexPrefix(this string value)
        {
            return value.Substring(value.StartsWith("0x") ? 2 : 0);
        }

        /// <summary>
        /// 檢查兩個十六進制字串是否相等（忽略大小寫和前綴）
        /// </summary>
        /// <param name="first">第一個十六進制字串</param>
        /// <param name="second">第二個十六進制字串</param>
        /// <returns>如果兩個字串表示相同的十六進制值則返回 true</returns>
        public static bool IsTheSameHex(this string first, string second)
        {
            // 確保兩個字串都有前綴並轉換為小寫進行比較
            return string.Equals(first.EnsureHexPrefix().ToLower(), second.EnsureHexPrefix().ToLower(), StringComparison.Ordinal);
        }

        /// <summary>
        /// 確保字串具有十六進制前綴 "0x"
        /// </summary>
        /// <param name="value">可能不包含前綴的十六進制字串</param>
        /// <returns>確保具有 "0x" 前綴的字串</returns>
        public static string EnsureHexPrefix(this string value)
        {
            if (value == null)
            {
                return null;
            }

            // 如果不以 "0x" 開頭，則添加前綴
            if (!value.HasHexPrefix())
            {
                return "0x" + value;
            }

            return value;
        }

        /// <summary>
        /// 確保字串陣列中的每個字串都具有十六進制前綴 "0x"
        /// </summary>
        /// <param name="values">十六進制字串陣列</param>
        /// <returns>確保每個元素都有前綴的陣列</returns>
        public static string[] EnsureHexPrefix(this string[] values)
        {
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i].EnsureHexPrefix();
                }
            }

            return values;
        }

        /// <summary>
        /// 將位元組陣列轉換為緊湊的十六進制字串（去除前導零）
        /// </summary>
        /// <param name="value">要轉換的位元組陣列</param>
        /// <returns>緊湊的十六進制字串表示形式</returns>
        public static string ToHexCompact(this byte[] value)
        {
            return value.ToHex().TrimStart('0');
        }

        /// <summary>
        /// 將十六進制字串轉換為緊湊形式（移除前綴和前導零）
        /// </summary>
        /// <param name="value">要轉換的十六進制字串</param>
        /// <returns>緊湊的十六進制字串</returns>
        public static string ToHexCompact(this string value)
        {
            return value.RemoveHexPrefix().TrimStart('0');
        }

        /// <summary>
        /// 將十六進制字串轉換為位元組陣列的內部實現
        /// </summary>
        /// <param name="value">要轉換的十六進制字串</param>
        /// <returns>對應的位元組陣列</returns>
        private static byte[] HexToByteArrayInternal(string value)
        {
            byte[] array = null;

            // 處理空或 null 輸入
            if (string.IsNullOrEmpty(value))
            {
                array = Empty;
            }
            else
            {
                int length = value.Length;

                // 確定是否有 "0x" 前綴並跳過
                int num = (value.StartsWith("0x", StringComparison.Ordinal) ? 2 : 0);

                // 計算實際的十六進制字符數量
                int num2 = length - num;

                // 處理奇數長度的十六進制字串（前面隱含添加一個 '0'）
                bool flag = false;
                if (num2 % 2 != 0)
                {
                    flag = true;
                    num2++;
                }

                // 創建目標位元組陣列
                array = new byte[num2 / 2];
                int num3 = 0;

                // 處理奇數長度的特殊情況
                if (flag)
                {
                    array[num3++] = FromCharacterToByte(value[num], num);
                    num++;
                }

                // 每次處理兩個十六進制字符，轉換為一個位元組
                for (int i = num; i < value.Length; i += 2)
                {
                    byte b = FromCharacterToByte(value[i], i, 4);     // 高四位
                    byte b2 = FromCharacterToByte(value[i + 1], i + 1); // 低四位
                    array[num3++] = (byte)(b | b2);                     // 合併為一個位元組
                }
            }

            return array;
        }

        /// <summary>
        /// 將十六進制字串轉換為位元組陣列
        /// </summary>
        /// <param name="value">要轉換的十六進制字串</param>
        /// <returns>對應的位元組陣列</returns>
        /// <exception cref="FormatException">如果輸入不是有效的十六進制字串</exception>
        public static byte[] HexToByteArray(this string value)
        {
            try
            {
                return HexToByteArrayInternal(value);
            }
            catch (FormatException innerException)
            {
                throw new FormatException($"String '{value}' could not be converted to byte array (not hex?).", innerException);
            }
        }

        /// <summary>
        /// 將單個十六進制字符轉換為對應的位元組值
        /// </summary>
        /// <param name="character">十六進制字符</param>
        /// <param name="index">字符在原始字串中的索引（用於錯誤報告）</param>
        /// <param name="shift">位移量，用於高位元組處理，默認為 0</param>
        /// <returns>轉換後的位元組值</returns>
        /// <exception cref="FormatException">如果字符不是有效的十六進制字符</exception>
        private static byte FromCharacterToByte(char character, int index, int shift = 0)
        {
            byte b = (byte)character;

            // 處理 A-F 或 a-f
            if ((64 < b && 71 > b) || (96 < b && 103 > b))
            {
                if (64 == (0x40 & b))
                {
                    // 將 A-F 或 a-f 轉換為 10-15 的數值，並根據 shift 進行位移
                    b = ((32 != (0x20 & b)) ? ((byte)(b + 10 - 65 << shift)) : ((byte)(b + 10 - 97 << shift)));
                }
            }
            else
            {
                // 處理 0-9
                if (41 >= b || 64 <= b)
                {
                    throw new FormatException($"Character '{character}' at index '{index}' is not valid alphanumeric character.");
                }

                // 將 '0'-'9' 轉換為 0-9 的數值，並根據 shift 進行位移
                b = (byte)(b - 48 << shift);
            }

            return b;
        }
    }
}
