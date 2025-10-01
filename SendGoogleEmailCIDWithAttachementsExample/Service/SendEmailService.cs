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
                
                mail.Body = @"
<!DOCTYPE html>
<html>

<head>
    <title>Home</title>
</head>

<body style=""margin: 0; padding: 0; width: 100%;"">
    <table role=""presentation"" style=""width: 100%; margin-top: 50px; border-collapse: collapse;"">
        <tr>
            <td align=""center"" style=""width: 100%; "">
                <div style=""max-width: 600px; text-align: start; font-size: 16px;"">
                    <img src=""cid:MyImage"" width=""150"" height=""150"" alt="""">
                    <p>您好, 欢迎使用 <span style=""font-weight: bold;"">OC88！</span> </p>
                    <p style=""  margin: 20px 0;"">
                        准备好在预测市场交易了吗？只需于注册页面输入下方安全码：
                    </p>

                    <div
                        style=""width: 400px; height: 88px; font-size: 42px; font-weight: bold; text-align: center; line-height: 88px; color: #008CFF; background-color: #F5F8FF; border-radius: 4px; margin: 20px 0; letter-spacing: 20px;"">
                        {securityCode}</div>
                    <p style=""margin: 20px 0;"">我们希望很快就能见到您使用
                        <span style=""font-weight: bold;"">OC88！</span>
                    </p>
                    <hr style=""border: 1px solid #B3B3B3"">
                    <p style=""color:#B3B3B3;font-size: 14px;"">此为自动发送的邮件，无法回复此电子邮件。</p>
                </div>
            </td>
        </tr>
    </table>
</body>

</html>





";

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
