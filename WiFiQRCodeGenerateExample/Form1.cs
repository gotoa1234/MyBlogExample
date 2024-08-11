using QRCoder;

namespace WiFiQRCodeGenerateExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // WiFi 信息
            string ssid = "Mesh_ChenFamily";
            string password = "xxxx";
            bool isWPA = true; // 若是WPA/WPA2加密，设为true；若是WEP加密，设为false

            // 创建WiFi QR Code数据
            string wifiQRCode = $"WIFI:T:{(isWPA ? "WPA" : "WEP")};S:{ssid};P:{password};;";

            // 使用QRCoder生成QR Code
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(wifiQRCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            // 将QR Code生成图片
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            // 保存图片到本地
            string filePath = "wifi_qrcode.png";
            qrCodeImage.Save(filePath);

            Console.WriteLine($"QR Code 已保存到: {filePath}");
        }
    }
}
