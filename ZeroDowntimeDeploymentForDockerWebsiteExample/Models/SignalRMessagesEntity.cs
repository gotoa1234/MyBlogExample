namespace ZeroDowntimeDeploymentForDockerWebsiteExample.Models
{
    public class SignalRMessagesEntity
    {
        public string UserName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string SiteName { get; set; }

        public long CreateTime { get; set; }
    }
}
