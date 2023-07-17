using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace WebSiteCaptchaLoginExample.Common.Util
{
    public static class CaptchaUtil
    {
        /// <summary>
        /// 產生驗證碼圖片
        /// </summary>
        /// <param name="captCha">驗證碼文字</param>
        /// <param name="bmpWidth">產生圖片寬</param>
        /// <param name="bmpHeight">產生圖片高</param>
        /// <param name="noisePotCount">雜點數量</param>
        /// <param name="noiseLineCount">雜訊線條數量</param>
        /// <returns></returns>
        public static byte[] GetCapChatImg(string captCha,
            int bmpWidth = 200,
            int bmpHeight = 80,
            int noisePotCount = 60,
            int noiseLineCount = 90)
        {
            using (Bitmap bmp = new Bitmap(bmpWidth, bmpHeight))
            {
                int xAxis1;
                int yAxis1;
                int xAxis2;
                int yAxis2;
                var random = new Random();
                Graphics graphItem = Graphics.FromImage(bmp);

                var fontStyle = Enum.GetValues(typeof(FontStyle)).Cast<FontStyle>().ToArray();
                var randomFontStyle = fontStyle[random.Next(0, fontStyle.Length)];

                Font font = new Font("Courier New", random.Next(36, 46), randomFontStyle);

                //設定圖片背景
                graphItem.Clear(Color.White);

                //產生雜點
                var noiseWidth = bmpWidth / 4;
                var noiseHeight = bmpHeight / 4;
                for (int noisePots = 0; noisePots < noisePotCount; noisePots++)
                {
                    xAxis1 = random.Next(0, bmp.Width);
                    yAxis1 = random.Next(0, bmp.Height);
                    bmp.SetPixel(xAxis1, yAxis1, Color.Brown);
                }

                //產生擾亂弧線
                for (int noiseLines = 0; noiseLines < noiseLineCount; noiseLines++)
                {
                    xAxis1 = random.Next(bmp.Width - noiseWidth);
                    yAxis1 = random.Next(bmp.Height - noiseHeight);
                    xAxis2 = random.Next(1, noiseWidth);
                    yAxis2 = random.Next(1, noiseHeight);
                    var startAngle = random.Next(0, 90);
                    var sweepAngle = random.Next(-270, 270);
                    graphItem.DrawArc(new Pen(Brushes.Gray), xAxis1, yAxis1, xAxis2, yAxis2, startAngle, sweepAngle);
                }

                var randomDrawX = random.Next(3, 30);
                var randomDrawY = random.Next(3, 12);

                graphItem.DrawString(captCha, font, GetRandomBrushes(), randomDrawX, randomDrawY);

                using (var memoryStream = new MemoryStream())
                {
                    bmp.Save(memoryStream, ImageFormat.Gif);

                    memoryStream.Close();
                    return memoryStream.GetBuffer();
                }
            }

            //筆刷顏色，為了用戶體驗與安全性，RGB隨機性在96~160
            Brush GetRandomBrushes(byte startRGB = 96, byte endRGB = 160)
            {
                Random r = new Random();
                int red = r.Next(startRGB, endRGB + 1);
                int green = r.Next(startRGB, endRGB + 1);
                int blue = r.Next(startRGB, endRGB + 1);
                return new SolidBrush(Color.FromArgb(red, green, blue));
            }
        }

        private const string baseNumberWord = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// 取得指定長度的字串
        /// </summary>        
        public static string GetRandomCaptcha(int length = 5)
        {
            var random = new Random();
            var strBuilder = new StringBuilder(32);
            for (var index = 0; index < length; index++)
            {
                strBuilder.Append(baseNumberWord.Substring(random.Next(baseNumberWord.Length), 1));
            }
            return strBuilder.ToString();
        }

    }
}
