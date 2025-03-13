using SendGoogleEmailCIDWithAttachementsExample.Models;

namespace SendGoogleEmailCIDWithAttachementsExample.Service
{
    public interface ISendEmailService
    {
        public Task<string> SendEmail(EmailDTO emailDto);
    }
}
