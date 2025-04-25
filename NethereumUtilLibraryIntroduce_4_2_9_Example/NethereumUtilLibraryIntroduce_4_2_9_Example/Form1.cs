using NethereumUtilLibraryIntroduce_4_2_9_Example.Demo;
using NethereumUtilLibraryIntroduce_4_2_9_Example.DemoMin;
using System.Text;

namespace NethereumUtilLibraryIntroduce_4_2_9_Example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 執行 Hash Compute
        /// </summary>        
        private void button1_Click(object sender, EventArgs e)
        {
            var result = string.Empty;
            try
            { 
                // 建立 Sha3Keccack 類別的實例，用於計算 Keccak-256 雜湊值
                var keccak = new DemoSha3KeccackMin();

                // 將輸入字串轉換為 UTF-8 編碼的位元組陣列
                byte[] bytes = Encoding.UTF8.GetBytes(textBoxInput.Text);

                // 使用 Keccak 演算法計算輸入位元組陣列的雜湊值
                byte[] hash = keccak.CalculateHash(bytes);

                // 將雜湊結果轉換為小寫的十六進制字串表示形式，並移除分隔符號 "-"
                result = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
            catch
            {
                // 異常處理：當 Nethereum.Util.Sha3Keccack 類庫執行發生問題時，
                // 由於該類別未實現 IDisposable 介面，無法正確釋放資源，因此選擇捨棄結果並返回空字串
            }
            textBoxOutput.Text = result;
        }
    }
}
