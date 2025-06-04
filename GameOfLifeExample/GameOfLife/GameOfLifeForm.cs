using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing.Imaging;
using Timer = System.Windows.Forms.Timer;

namespace GameOfLifeExample.GameOfLife
{
    public partial class GameOfLifeForm : Form
    {
        private const int WidthCells = 256;
        private const int HeightCells = 256;
        private const int CellSize = 2;
        private const int PeriodCount = 600;
        private byte[,] current;
        private byte[,] next;
        private Bitmap bitmap;
        private Timer timer;        

        public GameOfLifeForm()
        {
            InitializeComponent();
            InitialThisForm();

            // 1. 初始化配置
            void InitialThisForm()
            {
                // 根據縮放比例調整視窗大小
                this.ClientSize = new Size(WidthCells * CellSize, HeightCells * CellSize);

                // 初始化畫面                
                this.DoubleBuffered = true;
                this.Text = $"Game of Life (CPU Only) {WidthCells} * {Height}";              
            }
        }

        /// <summary>
        /// 2-1. 載入 Winform 觸發
        /// </summary>        
        private void GameOfLifeForm_Load(object sender, EventArgs e)
        {
            //2-2. 執行 CPU 運算的 Game Of Life
            GameOfLifeCpu();
        }

        /// <summary>
        /// 3. 實際代碼
        /// </summary>
        public void GameOfLifeCpu()
        {
            // 3-1. 初始化資料，決定像素數量 WidthCells * HeightCells
            current = new byte[WidthCells, HeightCells];
            next = new byte[WidthCells, HeightCells];
            Random rand = new();

            // 3-2. 初始化配置每個細胞 生 與 死
            for (int xAxis = 0; xAxis < WidthCells; xAxis++)
            {
                for (int yAxis = 0; yAxis < HeightCells; yAxis++)
                {
                    current[xAxis, yAxis] = (byte)(rand.NextDouble() > 0.7 ? 1 : 0);
                }
            }

            // 3-3. 建立放大後的 bitmap
            bitmap = new Bitmap(WidthCells * CellSize, HeightCells * CellSize, PixelFormat.Format24bppRgb);

            // 3-4. 設定定時更新
            Stopwatch stopwatch = new Stopwatch();// 紀錄時間用
            int counter = 0;   // 執行次數計數器
            timer = new Timer { Interval = 100 };
            timer.Tick += (s, e) => {
                Step();          // 執行你的邏輯
                counter++;       // 每次 Tick +1
                if (counter >= PeriodCount)
                {
                    timer.Stop();         // 停止 Timer
                                          // 你也可以加上結束後的邏輯
                    MessageBox.Show($"已執行 {PeriodCount} 生命週期，實際耗時：{stopwatch.Elapsed.TotalSeconds:F2} 秒");                    
                }
            };
            stopwatch.Start();
            timer.Start();
        }

        /// <summary>
        /// 4. 背景的 Timer 持續執行該方法 Interval 決定觸發的生命週期
        /// </summary>
        private void Step()
        {
            // 4-1. Game of Life 計算
            // 備註: xAxis, yAxis 代表細胞座標
            // 備註: gridX, gridY 代表鄰居偏移量
            for (int yAxis = 0; yAxis < HeightCells; yAxis++) 
            {
                for (int xAxis = 0; xAxis < WidthCells; xAxis++)
                {
                    // 4-2. 以當前細胞為基準點，計算鄰居數量                    
                    int count = 0;                    
                    for (int gridY = -1; gridY <= 1; gridY++)
                    {
                        for (int gridX = -1; gridX <= 1; gridX++)
                        {
                            if (gridX == 0 && gridY == 0) 
                                continue;

                            // ※處理邊界（環繞）
                            int nx = (xAxis + gridX + WidthCells) % WidthCells;
                            int ny = (yAxis + gridY + HeightCells) % HeightCells;
                            count += current[nx, ny];                           
                        }
                    }

                    byte alive = current[xAxis, yAxis];

                    // 4-3. Game of Life 規則
                    if (alive == 1 && (count < 2 || count > 3))
                        next[xAxis, yAxis] = 0; // 死亡
                    else if (alive == 0 && count == 3)
                        next[xAxis, yAxis] = 1; // 誕生
                    else
                        next[xAxis, yAxis] = alive; // 保持原狀
                }
            }

            // 4-4. 將新的生命週期細胞狀態替換 current / next
            (current, next) = (next, current);

            // 4-5. 更新畫面
            DrawBitmap();
            Invalidate();
        }

        /// <summary>
        /// 5. 更新到 Bitmap 上
        /// </summary>
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

        /// <summary>
        /// 6. 觸發畫 BitMap 時
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (bitmap != null)
            {
                // 6-1. 使用最近鄰插值來保持像素的清晰度
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                // 6-2. 繪製
                e.Graphics.DrawImage(bitmap,
                new Rectangle(0, 0, WidthCells * CellSize, HeightCells * CellSize),
                    new Rectangle(0, 0, WidthCells, HeightCells),
                    GraphicsUnit.Pixel);
            }
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
