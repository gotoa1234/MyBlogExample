namespace SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Model
{
	public class SignalRMessagesEntity
	{
		public int SignalRMessagesId { get; set; }

		public string UserId { get; set; } = string.Empty;

		public string Message { get; set; } = string.Empty;

		public int SiteValues { get; set; }

		public DateTime CreateTime { get; set; }

		public DateTime UpdateTime { get; set; }
	}
}
