using Line.Messaging;
using Line.Messaging.Webhooks;
using System.Text.RegularExpressions;

namespace LineBot2026Example.Service
{
    public class LineBotService : ILineBotService
    {
        private readonly ILogger<LineBotService> _logger;
        private readonly LineMessagingClient _lineClient;        

        public LineBotService(ILogger<LineBotService> logger, LineMessagingClient lineClient)
        {
            _logger = logger;
            _lineClient = lineClient;            
        }

        /// <summary>
        /// 依照收到的事件類型進行處理，目前僅處理文字訊息事件，並回覆相同的文字內容。
        /// </summary>
        public async Task HandleEventsAsync(IEnumerable<WebhookEvent> events)
        {
            foreach (var ev in events)
            {
                if (ev is MessageEvent messageEvent &&
                    messageEvent.Message.Type == EventMessageType.Text)
                {
                    var textMessage = (TextEventMessage)messageEvent.Message;
                    string userMessage = textMessage.Text;
                    string replyToken = messageEvent.ReplyToken;

                    // 如果收到特定指令，回覆來源 ID；否則回覆相同的文字內容
                    if (userMessage == "MilkTeaGreen_GetGroupId")
                    {
                        var (userId, groupId, roomId) = GetSourceIds(ev);
                        await _lineClient.ReplyMessageAsync(replyToken,
                            $"您的 UserId : {userId} , GroupId : {groupId} , RoomId : {roomId}");
                    }
                    else
                    {
                        await _lineClient.ReplyMessageAsync(replyToken, userMessage);
                    }
                }
            }
        }

        /// <summary>
        /// 進行推播
        /// </summary>
        public async Task HandleEventsPublishAsync(string message, string groupId)
        {
            // 推播訊息至指定群組
            await _lineClient.PushMessageAsync(groupId, $@"推播訊息 : {message}");
        }


        /// <summary>
        /// 取得來源 Id
        /// </summary>
        private (string UserId, string GroupId, string RoomId) GetSourceIds(WebhookEvent ev)
        {
            // 取得來源 ID
            var userId = string.Empty;// 永遠都有 UserId
            var roomId = string.Empty;
            var groupId = string.Empty;

            switch (ev.Source.Type)
            {
                case EventSourceType.User:
                    userId = ev.Source.UserId;
                    break;

                case EventSourceType.Group:
                    userId = ev.Source.UserId;
                    // 群組聊天 - GroupId 就是 ev.Source.Id
                    groupId = ev.Source.Id;
                    break;

                case EventSourceType.Room:
                    userId = ev.Source.UserId;
                    // 多人聊天室 - RoomId 就是 ev.Source.Id
                    roomId = ev.Source.Id;
                    break;
            }
            return (userId, groupId, roomId);
        }
    }
}
