using Nethereum.Util;
using NethereumUtilLibraryIntroduce_4_2_9_Example.DemoMin;
using Org.BouncyCastle.Crypto.Digests;
using System.Security.Cryptography;
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
            //����1
            var keccak = new Sha3Keccack();
            byte[] inputBytes = Encoding.UTF8.GetBytes("���ո��123");
            byte[] hashBytes = keccak.CalculateHash(inputBytes);

            var hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            //����2
            //var digest = new KeccakDigest(256);
            //byte[] inputBytes = Encoding.UTF8.GetBytes("���ո��123");
            //byte[] hashBytes = new byte[32]; // 256 bits = 32 bytes
            //digest.BlockUpdate(inputBytes, 0, inputBytes.Length);
            //digest.DoFinal(hashBytes, 0);
            //string hashHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();



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
