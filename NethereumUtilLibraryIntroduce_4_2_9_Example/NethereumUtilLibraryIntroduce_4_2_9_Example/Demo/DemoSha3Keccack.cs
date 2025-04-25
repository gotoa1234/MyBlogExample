using NethereumUtilLibraryIntroduce_4_2_9_Example.Demo.DependOn;
using System.Text;

namespace NethereumUtilLibraryIntroduce_4_2_9_Example.Demo
{
    /// <summary>
    /// 介紹 Nethereum.Util.Sha3Keccack 版本 4.2.9 的代碼
    /// <para>如何進行加密</para>
    /// </summary>
    public class DemoSha3Keccack
    {
        // 靜態屬性，提供一個單例模式的 Sha3Keccack 實例，方便全域呼叫
        public static DemoSha3Keccack Current { get; } = new DemoSha3Keccack();

        /// <summary>
        /// 計算字串的 Keccak-256 雜湊值並返回十六進制表示
        /// </summary>
        /// <param name="value">要計算雜湊的輸入字串</param>
        /// <returns>十六進制格式的雜湊結果</returns>
        public string CalculateHash(string value)
        {
            // 將輸入字串轉換為 UTF-8 編碼的位元組陣列
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            // 計算雜湊值並轉換為十六進制字串
            return CalculateHash(bytes).ToHex();
        }

        /// <summary>
        /// 計算多個十六進制字串合併後的 Keccak-256 雜湊值
        /// </summary>
        /// <param name="hexValues">要計算雜湊的十六進制字串陣列</param>
        /// <returns>十六進制格式的雜湊結果</returns>
        public string CalculateHashFromHex(params string[] hexValues)
        {
            // 將所有十六進制字串合併成一個字串，並移除每個字串的十六進制前綴（如 "0x"）
            string value = string.Join("", hexValues.Select((string x) => x.RemoveHexPrefix()).ToArray());
            // 將合併後的十六進制字串轉換成位元組陣列，計算雜湊值，並轉換為十六進制字串
            return CalculateHash(value.HexToByteArray()).ToHex();
        }

        /// <summary>
        /// 核心方法：計算位元組陣列的 Keccak-256 雜湊值
        /// </summary>
        /// <param name="value">要計算雜湊的位元組陣列</param>
        /// <returns>雜湊結果的位元組陣列</returns>
        public byte[] CalculateHash(byte[] value)
        {
            // 建立一個 256 位元的 KeccakDigest 物件
            KeccakDigest keccakDigest = new KeccakDigest(256);
            // 建立一個與摘要大小相同的位元組陣列來存放結果
            byte[] array = new byte[keccakDigest.GetDigestSize()];
            // 將輸入資料更新到雜湊計算中
            keccakDigest.BlockUpdate(value, 0, value.Length);
            // 完成雜湊計算並將結果寫入輸出陣列
            keccakDigest.DoFinal(array, 0);
            return array;
        }

        /// <summary>
        /// 計算字串的 Keccak-256 雜湊值並返回位元組陣列
        /// </summary>
        /// <param name="value">要計算雜湊的輸入字串</param>
        /// <returns>雜湊結果的位元組陣列</returns>
        public byte[] CalculateHashAsBytes(string value)
        {
            // 將輸入字串轉換為 UTF-8 編碼的位元組陣列
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            // 計算並返回雜湊值的位元組陣列
            return CalculateHash(bytes);
        }
    }
}
