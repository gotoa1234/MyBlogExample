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
        private byte[,] current;  // �אּ�G���}�C
        private byte[,] next;     // �אּ�G���}�C
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
            // ��l�Ƶe��
            this.ClientSize = new Size(WidthCells, HeightCells);
            this.DoubleBuffered = true;
            this.Text = "Game of Life (GPU with ILGPU)";

            // ��l�Ƹ�� - �ץ����G���}�C
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

            // ��l�� ILGPU
            context = Context.CreateDefault();
            accelerator = context.CreateCudaAccelerator(0);
            kernel = accelerator.LoadAutoGroupedStreamKernel<Index2D, ArrayView2D<byte, Stride2D.DenseX>, ArrayView2D<byte, Stride2D.DenseX>>(GpuKernel);

            // �]�w�w�ɧ�s
            timer = new Timer { Interval = 100 }; // �C 100ms ��s
            timer.Tick += (s, e) => Step();
            timer.Start();
        }

        private void Step()
        {
            // �]�w GPU buffer - �����q�}�C���t�ýƻs
            using var bufferCurrent = accelerator.Allocate2DDenseX<byte>(current);
            using var bufferNext = accelerator.Allocate2DDenseX<byte>(new Index2D(WidthCells, HeightCells));

            // ���� kernel - �ǻ� View �� kernel
            kernel(new Index2D(WidthCells, HeightCells), bufferCurrent.View, bufferNext.View);

            // ���� GPU ����
            accelerator.Synchronize();

            // ���o�p�⵲�G
            next = bufferNext.GetAsArray2D();

            // �洫 current / next
            (current, next) = (next, current);

            // ��s�e��
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
                        int value = current[x, y] * 255;  // �ץ����G���}�C�s��
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

        // GPU �֤ߵ{��
        static void GpuKernel(Index2D index, ArrayView2D<byte, Stride2D.DenseX> current, ArrayView2D<byte, Stride2D.DenseX> next)
        {
            int x = index.X;
            int y = index.Y;
            int width = current.IntExtent.X;
            int height = current.IntExtent.Y;

            // �p��F�~�ƶq
            int count = 0;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;

                    // �B�z��ɡ]��¶�^
                    int nx = (x + dx + width) % width;
                    int ny = (y + dy + height) % height;
                    count += current[new Index2D(nx, ny)];
                }
            }

            byte alive = current[index];

            // Game of Life �W�h
            if (alive == 1 && (count < 2 || count > 3))
                next[index] = 0; // ���`
            else if (alive == 0 && count == 3)
                next[index] = 1; // �ϥ�
            else
                next[index] = alive; // �O���쪬
        }
    }
}