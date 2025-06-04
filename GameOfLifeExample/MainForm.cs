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
            // 1. 建立指定表單
            var gameForm = new GameOfLifeForm();
            //var gameForm = new GameOfLifeGPUForm();

            // 2-1. 設定當 GameOfLifeForm 關閉時要執行的事件
            gameForm.FormClosed += (s, args) =>
            {
                this.Show();// 2-2. 關閉時觸發顯示原本的主體表單
            };

            // 2-3. 隱藏主體表單
            this.Hide();

            // 3. 顯示指定表單
            gameForm.Show();
        }

        /// <summary>
        /// Execute GPU
        /// </summary>        
        private void button1_Click(object sender, EventArgs e)
        {
            // 1. 建立指定表單            
            var gameForm = new GameOfLifeGPUForm();

            // 2-1. 設定當 GameOfLifeForm 關閉時要執行的事件
            gameForm.FormClosed += (s, args) =>
            {
                this.Show();// 2-2. 關閉時觸發顯示原本的主體表單
            };

            // 2-3. 隱藏主體表單
            this.Hide();

            // 3. 顯示指定表單
            gameForm.Show();
        }
    }
}