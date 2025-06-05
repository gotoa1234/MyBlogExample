using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using System.Diagnostics;
using System.Drawing.Imaging;
using Timer = System.Windows.Forms.Timer;

namespace GameOfLifeExample.GameOfLifeGPU
{
    public partial class GameOfLifeGPUForm : Form
    {
        private const int WidthCells = 256;
        private const int HeightCells = 256;
        private const int CellSize = 2;
        private const int PeriodCount = 600;
        private byte[,] current;  
        private byte[,] next;     
        private Bitmap bitmap;
        private Timer timer;

        private Context context;
        private Accelerator accelerator;
        private Action<Index2D, ArrayView2D<byte, Stride2D.DenseX>, ArrayView2D<byte, Stride2D.DenseX>> kernel;

        public GameOfLifeGPUForm()
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
                this.Text = $"Game of Life GPU {WidthCells} * {HeightCells}";
            }
        }

        /// <summary>
        /// 2-1. 載入 Winform 觸發
        /// </summary> 
        private void GameOfLifeGPU_Load(object sender, EventArgs e)
        {
            //2-2. 執行 GPU 運算的 Game Of Life
            GameOfLifeGPU();
        }

        /// <summary>
        /// 3. 實際代碼
        /// </summary>
        public void GameOfLifeGPU()
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

            // 3-4. 初始化 ILGPU
            context = Context.CreateDefault();            
            try
            {
                // GPU
                accelerator = context.CreateCudaAccelerator(0);//電腦沒有顯示卡，此行會報錯          
            }
            catch
            {
                // CPU
                accelerator = context.CreateCPUAccelerator(0);
            }
            // 3-5. 設定 kernel 方法
            kernel = accelerator.LoadAutoGroupedStreamKernel<Index2D, ArrayView2D<byte, Stride2D.DenseX>, ArrayView2D<byte, Stride2D.DenseX>>(GpuKernel);

            // 3-6. 設定定時更新
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
            // 4-1. 設定 GPU buffer - 直接從陣列分配並複製
            using var bufferCurrent = accelerator.Allocate2DDenseX<byte>(current);
            using var bufferNext = accelerator.Allocate2DDenseX<byte>(new Index2D(WidthCells, HeightCells));

            // 4-2. 執行 kernel - 傳遞 View 給 kernel (在步驟 3-5. 宣告方法)
            kernel(new Index2D(WidthCells, HeightCells), bufferCurrent.View, bufferNext.View);

            // 4-3. 等待 GPU 完成
            accelerator.Synchronize();

            // 4-4. 取得計算結果
            next = bufferNext.GetAsArray2D();

            // 4-5. 交換 current / next
            (current, next) = (next, current);

            // 4-6. 更新畫面
            DrawBitmap();
            Invalidate();
        }


        /// <summary>
        /// 6. 更新到 Bitmap 上
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

                for (int y = 0; y < HeightCells; y++)
                {
                    for (int x = 0; x < WidthCells; x++)
                    {
                        int value = current[x, y] * 255;  // 修正為二維陣列存取
                        ptr[y * stride + x * 3 + 0] = (byte)value; // B
                        ptr[y * stride + x * 3 + 1] = (byte)value; // G
                        ptr[y * stride + x * 3 + 2] = (byte)value; // R
                    }
                }
            }

            bitmap.UnlockBits(data);
        }

        /// <summary>
        /// 7. 觸發畫 BitMap 時
        /// </summary>
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
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            timer?.Stop();
            timer?.Dispose();
            bitmap?.Dispose();
            accelerator?.Dispose();
            context?.Dispose();
        }

        /// <summary>
        /// 5. GPU 核心程式 - 康威生命遊戲的規則 與 3-5 , 4-2 相互關聯
        /// </summary>        
        static void GpuKernel(Index2D index, ArrayView2D<byte, Stride2D.DenseX> current, ArrayView2D<byte, Stride2D.DenseX> next)
        {
            // 5-1. Game of Life 計算
            // 備註: xAxis, yAxis 代表細胞座標
            // 備註: gridX, gridY 代表鄰居偏移量
            int x = index.X;
            int y = index.Y;
            int width = current.IntExtent.X;
            int height = current.IntExtent.Y;

            // 5-2. 計算鄰居數量
            int count = 0;
            for (int yAxis = -1; yAxis <= 1; yAxis++)
            {
                for (int xAxis = -1; xAxis <= 1; xAxis++)
                {
                    if (xAxis == 0 && yAxis == 0) continue;

                    // 處理邊界（環繞）
                    int nx = (x + xAxis + width) % width;
                    int ny = (y + yAxis + height) % height;
                    count += current[new Index2D(nx, ny)];
                }
            }

            byte alive = current[index];

            // 5-3. Game of Life 規則
            if (alive == 1 && (count < 2 || count > 3))
                next[index] = 0; // 死亡
            else if (alive == 0 && count == 3)
                next[index] = 1; // 誕生
            else
                next[index] = alive; // 保持原狀
        }
    }
}
