using GameOfLifeExample.GameOfLife;
using GameOfLifeExample.GameOfLifeGPU;


namespace GameOfLifeExample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Execute CPU
        /// </summary>        
        private void buttonExecute_Click(object sender, EventArgs e)
        {
            // 1. �إ߫��w���
            var gameForm = new GameOfLifeForm();
            //var gameForm = new GameOfLifeGPUForm();

            // 2-1. �]�w�� GameOfLifeForm �����ɭn���檺�ƥ�
            gameForm.FormClosed += (s, args) =>
            {
                this.Show();// 2-2. ������Ĳ�o��ܭ쥻���D����
            };

            // 2-3. ���åD����
            this.Hide();

            // 3. ��ܫ��w���
            gameForm.Show();
        }

        /// <summary>
        /// Execute GPU
        /// </summary>        
        private void button1_Click(object sender, EventArgs e)
        {
            // 1. �إ߫��w���            
            var gameForm = new GameOfLifeGPUForm();

            // 2-1. �]�w�� GameOfLifeForm �����ɭn���檺�ƥ�
            gameForm.FormClosed += (s, args) =>
            {
                this.Show();// 2-2. ������Ĳ�o��ܭ쥻���D����
            };

            // 2-3. ���åD����
            this.Hide();

            // 3. ��ܫ��w���
            gameForm.Show();
        }
    }
}