using NethereumUtilLibraryIntroduce_4_2_9_Example.DemoMin.DependOn;
using System.Text;

namespace NethereumUtilLibraryIntroduce_4_2_9_Example.DemoMin
{
    /// <summary>
    /// 介紹 Nethereum.Util.Sha3Keccack 版本 4.2.9 的代碼
    /// <para>如何進行加密</para>
    /// </summary>
    public class DemoSha3KeccackMin
    {
        /// <summary>
        /// 核心方法：計算位元組陣列的 Keccak-256 雜湊值
        /// </summary>
        /// <param name="value">要計算雜湊的位元組陣列</param>
        /// <returns>雜湊結果的位元組陣列</returns>
        public byte[] CalculateHash(byte[] value)
        {
            // 建立一個 256 位元的 KeccakDigest 物件
            KeccakDigestMin keccakDigest = new KeccakDigestMin(256);
            // 建立一個與摘要大小相同的位元組陣列來存放結果
            byte[] array = new byte[keccakDigest.GetDigestSize()];
            // 將輸入資料更新到雜湊計算中
            keccakDigest.BlockUpdate(value, 0, value.Length);
            // 完成雜湊計算並將結果寫入輸出陣列
            keccakDigest.DoFinal(array, 0);
            return array;
        }
    }
}
