using Line.Messaging.Webhooks;

namespace LineBot2026Example.Service
{
    public interface ILineBotService
    {
        Task HandleEventsAsync(IEnumerable<WebhookEvent> events);

        Task HandleEventsPublishAsync(string message, string groupId);

        Task HandleFlexPublishAsync(string message, string groupId);
    }
}
