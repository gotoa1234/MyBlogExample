using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;
using System.Drawing.Imaging;
using Timer = System.Windows.Forms.Timer;

namespace GameOfLifeExample
{
    public class GameOfLifeGpu : Form
{
    private const int WidthCells = 256;
    private const int HeightCells = 256;
    private byte[] current;
    private byte[] next;
    private Bitmap bitmap;
    private Timer timer;

    private Context context;
    private Accelerator accelerator;
    private Action<Index2D, ArrayView2D<byte, Stride2D.DenseX>, ArrayView2D<byte, Stride2D.DenseY>> kernel;
    
        public GameOfLifeGpu()
    {
        // 初始化畫面
        this.ClientSize = new Size(WidthCells, HeightCells);
        this.DoubleBuffered = true;
        this.Text = "Game of Life (GPU with ILGPU)";

        // 初始化資料
        current = new byte[WidthCells * HeightCells];
        next = new byte[WidthCells * HeightCells];
        Random rand = new();
        for (int i = 0; i < current.Length; i++)
            current[i] = (byte)(rand.NextDouble() > 0.7 ? 1 : 0);

        bitmap = new Bitmap(WidthCells, HeightCells, PixelFormat.Format24bppRgb);

        // 初始化 ILGPU
        context = Context.CreateDefault();
        accelerator = context.CreateCudaAccelerator(0);
        kernel = accelerator.LoadAutoGroupedStreamKernel<Index2D, ArrayView2D<byte>, ArrayView2D<byte>>(GpuKernel);

        // 設定定時更新
        timer = new Timer { Interval = 100 }; // 每 100ms 更新
        timer.Tick += (s, e) => Step();
        timer.Start();
    }

    private void Step()
    {
        // 設定 GPU buffer
        using var bufferCurrent = accelerator.Allocate2D<byte>(WidthCells, HeightCells);
        using var bufferNext = accelerator.Allocate2D<byte>(WidthCells, HeightCells);

        bufferCurrent.CopyFromCPU(current, new Index2D(WidthCells, HeightCells));
        kernel(new Index2D(WidthCells, HeightCells), bufferCurrent.View, bufferNext.View);
        accelerator.Synchronize();

        bufferNext.CopyToCPU(next, new Index2D(WidthCells, HeightCells));

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
                    int value = current[y * WidthCells + x] * 255;
                    ptr[y * stride + x * 3 + 0] = (byte)value;
                    ptr[y * stride + x * 3 + 1] = (byte)value;
                    ptr[y * stride + x * 3 + 2] = (byte)value;
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
        accelerator.Dispose();
        context.Dispose();
    }

    // GPU 核心程式
    static void GpuKernel(Index2D index, ArrayView2D<byte> current, ArrayView2D<byte> next)
    {
        int x = index.X;
        int y = index.Y;
        int width = current.Extent.X;
        int height = current.Extent.Y;

        int count = 0;
        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = (x + dx + width) % width;
                int ny = (y + dy + height) % height;
                count += current[nx, ny];
            }
        }

        byte alive = current[x, y];
        if (alive == 1 && (count < 2 || count > 3))
            next[x, y] = 0;
        else if (alive == 0 && count == 3)
            next[x, y] = 1;
        else
            next[x, y] = alive;
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new GameOfLifeGpu());
    }
}
}
