using ChatApp;
using Grpc.Core;
using static ChatApp.ChatService;

namespace NetCoreGRPCChattingRoomExample.Service
{
    public class ChatService : ChatServiceBase
    {
        /// <summary>
        /// 訂閱者
        /// </summary>
        private static readonly List<IServerStreamWriter<Message>> Subscribers = new List<IServerStreamWriter<Message>>();

        /// <summary>
        /// 將訂閱者加入
        /// </summary>        
        public override async Task Subscribe(IAsyncStreamReader<SubscribeRequest> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            var subscriberName = "";
            await foreach (var request in requestStream.ReadAllAsync())
            {
                //1. 加入訂閱
                subscriberName = request.SubscriberName;
                Subscribers.Add(responseStream);
            }

            // 2. 等待客戶端斷開連線            
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);  // 或使用其他適當的延遲時間
            }

            // 3. 客戶端斷開連線後，移除訂閱者
            Subscribers.RemoveAll(s => s == responseStream);           
        }

        /// <summary>
        /// 發送訊息 + 推播
        /// </summary>                
        public override async Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            var response = new MessageResponse
            {
                Username = request.Username,
                Message = request.Message
            };

            var pushMessage = new Message() { 
                Content = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " => " + response.Username +"：" + response.Message + Environment.NewLine };
            await BroadcastMessageAsync(pushMessage);

            return response;
        }

        /// <summary>
        /// 推播訊息
        /// </summary>                
        public static async Task BroadcastMessageAsync(Message message)
        {
            foreach (var subscriber in Subscribers.ToList())
            {
                try
                {
                    await subscriber.WriteAsync(message);
                }
                catch (Exception ex)
                {                    
                    Console.WriteLine($"Error broadcasting message to a subscriber: {ex.Message}");
                    Subscribers.Remove(subscriber);
                }
            }
        }
    }


}
