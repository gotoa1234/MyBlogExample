using System.Text;

namespace NethereumUtilLibraryIntroduce_4_2_9_Example.DemoMin.DependOn
{
    /// <summary>
    /// 提供陣列操作的工具類別，包括陣列比較、複製、轉換等常用功能
    /// 此類別作為一個工具集，提供處理各種類型陣列的實用方法
    /// </summary>
    public abstract class ArraysMin
    {
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
    }
}
