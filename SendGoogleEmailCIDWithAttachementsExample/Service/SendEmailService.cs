using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace SendGoogleEmailCIDWithAttachementsExample.Service
{
    public class SendEmailService : ISendEmailService
    {
        public async Task SendEmail()
        {
            try
            {
                // SMTP 設定
                string smtpServer = "smtp.gmail.com"; // 替換為 SMTP 伺服器
                int smtpPort = 587; // 465 (SSL) / 587 (TLS)
                string senderEmail = "cap8826@gmail.com";
                string senderPassword = "fdwxzlnbaeakmran";
                string recipientEmail = "cap8825@gmail.com";

                // 建立 MailMessage
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = "個人資料 - 附帶圖片",
                    IsBodyHtml = true
                };
                mail.To.Add(recipientEmail);

                // 附件圖片
                //string imagePath = @"C:\path\to\image.jpg"; // 替換為你的圖片路徑
                //Attachment inlineImage = new Attachment(imagePath);

                // 圖片的 URL
                string imageUrl = "https://gotoa1234.github.io/assets/image/ContinuousDeployment/docker/2025_03_08/005.png"; // 替換為你的圖片網址
                byte[] imageBytes = await DownloadImageAsync(imageUrl);

                if (imageBytes == null || imageBytes.Length == 0)
                {
                    Console.WriteLine("圖片下載失敗！");
                    return;
                }
                MemoryStream imageStream = new MemoryStream(imageBytes);
                Attachment inlineImage = new Attachment(imageStream, "louis.jpg", "image/jpeg");
                inlineImage.ContentId = "MyImage"; // 這個 Content-ID 用於 HTML 內嵌
                inlineImage.ContentDisposition.Inline = true;
                inlineImage.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                mail.Attachments.Add(inlineImage);

                // 設定 HTML 內容，使用 <img> 內嵌圖片
                mail.Body = @"
                <html>
                <body>
                    <p>Dear 先生:</p>
                    <p>你好，我是 XXX (這邊附上 Attachments 圖片)</p>
                    <img src=""cid:MyImage"" width=""300"" alt=""個人照片"" />
                    <p>這是我的個人資料 ....</p>
                    <br />
                    <p>Thanks, have a good day.</p>
                </body>
                </html>";

                // 設定 SMTP 用戶端
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                };

                // 發送郵件
                smtpClient.Send(mail);
                Console.WriteLine("郵件發送成功！");
            }
            catch (Exception ex)
            {
                Console.WriteLine("郵件發送失敗：" + ex.Message);
            }

        }

        /// <summary>
        /// 下載網路圖片
        /// </summary>
        private static async Task<byte[]> DownloadImageAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    return await client.GetByteArrayAsync(url);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("圖片下載錯誤：" + ex.Message);
                    return null;
                }
            }
        }
    }
}
