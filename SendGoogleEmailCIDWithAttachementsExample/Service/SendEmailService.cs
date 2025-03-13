using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using SendGoogleEmailCIDWithAttachementsExample.Models;

namespace SendGoogleEmailCIDWithAttachementsExample.Service
{
    public class SendEmailService : ISendEmailService
    {
        /// <summary>
        /// 使用附件方式功能，發送郵件
        /// </summary>
        public async Task<string> SendEmail(EmailDTO emailDto)
        {
            try
            {
                // 1. 輸入自己的信箱密碼 - 這個要輸入自己安全應用程式上的產生密碼
                string senderPassword = emailDto.SenderPassword;

                // 2-1. 建立 MailMessage
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(emailDto.SenderEmail),
                    Subject = "個人資料 - 附帶圖片",
                    IsBodyHtml = true,
                };
                // 2-2. 郵件副本對象
                mail.To.Add(emailDto.RecipientEmail);

                // 2-3. 可將圖片網址做為傳參
                var imageBytes = await DownloadImageAsync();

                // 2-4. 將圖片存成附件，讓 Mail 中，不依賴 Url 而是存在 Mail 中
                //      優點：未來Url失效時，此郵件仍可檢閱圖片

                var imageStream = new MemoryStream(imageBytes);
                var inlineImage = new Attachment(imageStream, "louis.jpg", "image/jpeg");

                // 關鍵：設定，並讓這個 Content-ID 用於 HTML 內嵌
                inlineImage.ContentId = "MyImage";
                inlineImage.ContentDisposition.Inline = true;
                inlineImage.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                mail.Attachments.Add(inlineImage);

                // 2-5. 撰寫 HTML 內容，並且使用 <img> 內嵌圖片
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

                // 3. 設定 SMTP 用戶端
                SmtpClient smtpClient = new SmtpClient(emailDto.SmtpServer, emailDto.SmtpPort)
                {
                    Credentials = new NetworkCredential(emailDto.SenderEmail, senderPassword),
                    EnableSsl = true
                };

                // 4. 發送郵件
                smtpClient.Send(mail);
                return "郵件發送成功！";
            }
            catch (Exception ex)
            {
                return $@"郵件發送失敗：{ex.Message}";
            }

        }

        /// <summary>
        /// 下載網路圖片
        /// </summary>
        private static async Task<byte[]> DownloadImageAsync(
            string url = "https://gotoa1234.github.io/assets/image/ContinuousDeployment/docker/2025_03_08/005.png")
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var getResult = await client.GetByteArrayAsync(url);

                    if (getResult == null ||
                        getResult.Length == 0)
                    {
                        throw new Exception("圖片下載失敗！");
                    }
                    return getResult;
                }
                catch (Exception ex)
                {
                    throw new Exception("圖片下載錯誤：" + ex.Message);
                }
            }
        }
    }
}
