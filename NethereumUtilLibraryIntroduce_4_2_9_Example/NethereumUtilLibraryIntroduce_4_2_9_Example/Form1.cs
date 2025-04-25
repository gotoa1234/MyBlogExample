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
        /// ���� Hash Compute
        /// </summary>        
        private void button1_Click(object sender, EventArgs e)
        {
            var result = string.Empty;
            try
            { 
                // �إ� Sha3Keccack ���O����ҡA�Ω�p�� Keccak-256 �����
                var keccak = new DemoSha3KeccackMin();

                // �N��J�r���ഫ�� UTF-8 �s�X���줸�հ}�C
                byte[] bytes = Encoding.UTF8.GetBytes(textBoxInput.Text);

                // �ϥ� Keccak �t��k�p���J�줸�հ}�C�������
                byte[] hash = keccak.CalculateHash(bytes);

                // �N���굲�G�ഫ���p�g���Q���i��r���ܧΦ��A�ò������j�Ÿ� "-"
                result = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
            catch
            {
                // ���`�B�z�G�� Nethereum.Util.Sha3Keccack ���w����o�Ͱ��D�ɡA
                // �ѩ�����O����{ IDisposable �����A�L�k���T����귽�A�]����ܱ˱󵲪G�ê�^�Ŧr��
            }
            textBoxOutput.Text = result;
        }
    }
}
