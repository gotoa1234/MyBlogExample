using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using System.Drawing.Imaging;
using Timer = System.Windows.Forms.Timer;

namespace GameOfLifeExample
{
    public partial class Form1 : Form
    {
        private const int WidthCells = 256;
        private const int HeightCells = 256;
        private byte[,] current;  // 改為二維陣列
        private byte[,] next;     // 改為二維陣列
        private Bitmap bitmap;
        private Timer timer;

        private Context context;
        private Accelerator accelerator;
        private Action<Index2D, ArrayView2D<byte, Stride2D.DenseX>, ArrayView2D<byte, Stride2D.DenseX>> kernel;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GameOfLifeGpu();
        }

        public void GameOfLifeGpu()
        {
            // 初始化畫面
            this.ClientSize = new Size(WidthCells, HeightCells);
            this.DoubleBuffered = true;
            this.Text = "Game of Life (GPU with ILGPU)";

            // 初始化資料 - 修正為二維陣列
            current = new byte[WidthCells, HeightCells];
            next = new byte[WidthCells, HeightCells];
            Random rand = new();
            for (int x = 0; x < WidthCells; x++)
            {
                for (int y = 0; y < HeightCells; y++)
                {
                    current[x, y] = (byte)(rand.NextDouble() > 0.7 ? 1 : 0);
                }
            }

            bitmap = new Bitmap(WidthCells, HeightCells, PixelFormat.Format24bppRgb);

            // 初始化 ILGPU
            context = Context.CreateDefault();
            accelerator = context.CreateCudaAccelerator(0);
            kernel = accelerator.LoadAutoGroupedStreamKernel<Index2D, ArrayView2D<byte, Stride2D.DenseX>, ArrayView2D<byte, Stride2D.DenseX>>(GpuKernel);

            // 設定定時更新
            timer = new Timer { Interval = 100 }; // 每 100ms 更新
            timer.Tick += (s, e) => Step();
            timer.Start();
        }

        private void Step()
        {
            // 設定 GPU buffer - 直接從陣列分配並複製
            using var bufferCurrent = accelerator.Allocate2DDenseX<byte>(current);
            using var bufferNext = accelerator.Allocate2DDenseX<byte>(new Index2D(WidthCells, HeightCells));

            // 執行 kernel - 傳遞 View 給 kernel
            kernel(new Index2D(WidthCells, HeightCells), bufferCurrent.View, bufferNext.View);

            // 等待 GPU 完成
            accelerator.Synchronize();

            // 取得計算結果
            next = bufferNext.GetAsArray2D();

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

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
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

        // GPU 核心程式
        static void GpuKernel(Index2D index, ArrayView2D<byte, Stride2D.DenseX> current, ArrayView2D<byte, Stride2D.DenseX> next)
        {
            int x = index.X;
            int y = index.Y;
            int width = current.IntExtent.X;
            int height = current.IntExtent.Y;

            // 計算鄰居數量
            int count = 0;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;

                    // 處理邊界（環繞）
                    int nx = (x + dx + width) % width;
                    int ny = (y + dy + height) % height;
                    count += current[new Index2D(nx, ny)];
                }
            }

            byte alive = current[index];

            // Game of Life 規則
            if (alive == 1 && (count < 2 || count > 3))
                next[index] = 0; // 死亡
            else if (alive == 0 && count == 3)
                next[index] = 1; // 誕生
            else
                next[index] = alive; // 保持原狀
        }
    }
}