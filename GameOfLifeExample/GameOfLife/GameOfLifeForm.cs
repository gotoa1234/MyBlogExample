using System.Drawing.Imaging;
using Timer = System.Windows.Forms.Timer;

namespace GameOfLifeExample.GameOfLife
{
    public partial class GameOfLifeForm : Form
    {
        private const int WidthCells = 256;
        private const int HeightCells = 256;
        private const int CellSize = 3;
        private byte[,] current;
        private byte[,] next;
        private Bitmap bitmap;
        private Timer timer;

        public GameOfLifeForm()
        {
            InitializeComponent();
            InitialThisForm();

            void InitialThisForm()
            {
                // 根據縮放比例調整視窗大小
                this.ClientSize = new Size(WidthCells * CellSize, HeightCells * CellSize);

                // 初始化畫面
                this.ClientSize = new Size(WidthCells, HeightCells);
                this.DoubleBuffered = true;
                this.Text = "Game of Life (CPU Only)";              
            }
        }

        private void GameOfLifeForm_Load(object sender, EventArgs e)
        {
            GameOfLifeCpu();
        }

        public void GameOfLifeCpu()
        {
            // 初始化資料
            current = new byte[WidthCells, HeightCells];
            next = new byte[WidthCells, HeightCells];
            Random rand = new();

            for (int xAxis = 0; xAxis < WidthCells; xAxis++)
            {
                for (int yAxis = 0; yAxis < HeightCells; yAxis++)
                {
                    current[xAxis, yAxis] = (byte)(rand.NextDouble() > 0.7 ? 1 : 0);
                }
            }

            bitmap = new Bitmap(WidthCells, HeightCells, PixelFormat.Format24bppRgb);

            // 設定定時更新
            timer = new Timer { Interval = 100 };
            timer.Tick += (s, e) => Step();
            timer.Start();
        }

        private void Step()
        {
            // CPU 版本的 Game of Life 計算
            for (int xAxis = 0; xAxis < WidthCells; xAxis++)
            {
                for (int yAxis = 0; yAxis < HeightCells; yAxis++)
                {
                    // 計算鄰居數量
                    int count = 0;
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            if (dx == 0 && dy == 0) continue;

                            // 處理邊界（環繞）
                            int nx = (xAxis + dx + WidthCells) % WidthCells;
                            int ny = (yAxis + dy + HeightCells) % HeightCells;
                            count += current[nx, ny];
                        }
                    }

                    byte alive = current[xAxis, yAxis];

                    // Game of Life 規則
                    if (alive == 1 && (count < 2 || count > 3))
                        next[xAxis, yAxis] = 0; // 死亡
                    else if (alive == 0 && count == 3)
                        next[xAxis, yAxis] = 1; // 誕生
                    else
                        next[xAxis, yAxis] = alive; // 保持原狀
                }
            }

            // 交換 current / next
            (current, next) = (next, current);

            // 更新畫面
            DrawBitmap();
            Invalidate();
        }

        private void DrawBitmap()
        {
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, WidthCells, HeightCells),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                for (int yAxis = 0; yAxis < HeightCells; yAxis++)
                {
                    for (int xAxis = 0; xAxis < WidthCells; xAxis++)
                    {
                        int value = current[xAxis, yAxis] * 255;
                        ptr[yAxis * stride + xAxis * 3 + 0] = (byte)value; // B
                        ptr[yAxis * stride + xAxis * 3 + 1] = (byte)value; // G
                        ptr[yAxis * stride + xAxis * 3 + 2] = (byte)value; // R
                    }
                }
            }

            bitmap.UnlockBits(data);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (bitmap != null)
            {
                // 使用最近鄰插值來保持像素的清晰度
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                // 縮放繪製
                e.Graphics.DrawImage(bitmap,
                new Rectangle(0, 0, WidthCells * CellSize, HeightCells * CellSize),
                    new Rectangle(0, 0, WidthCells, HeightCells),
                    GraphicsUnit.Pixel);
            }

            //e.Graphics.DrawImage(bitmap, 0, 0);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            timer?.Stop();
            timer?.Dispose();
            bitmap?.Dispose();
        }
    }
}
