using ChatApp;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static ChatApp.ChatService;

namespace NetCoreGRPCChattingRoomExample.Service
{
    public class ChatService : ChatServiceBase
    {
        //private static readonly List<string> ActiveUsers = new List<string>();
        //private static Queue<string> ChatRoomMessage = new Queue<string>();
        //private static readonly List<IServerStreamWriter<MessageResponse>> StreamObservers = new List<IServerStreamWriter<MessageResponse>>();
        //private static readonly List<IServerStreamWriter<JoinResponse>> StreamJoinObservers = new List<IServerStreamWriter<JoinResponse>>();

        //public override Task<JoinResponse> Join(JoinRequest request, ServerCallContext context)
        //{
        //    if (!ActiveUsers.Contains(request.Username))
        //    {
        //        ActiveUsers.Add(request.Username);
        //        BroadcastActiveUsers();

        //        var response = new JoinResponse
        //        {
        //            ActiveUsers = { ActiveUsers }
        //        };
        //        return Task.FromResult(response);
        //    }
        //    return Task.FromResult(new JoinResponse());
        //}

        //public override async Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        //{
        //    var response = new MessageResponse
        //    {
        //        Username = request.Username,
        //        Message = request.Message
        //    };
        //    await BroadcastMessage(response);

        //    return response;
        //}


        //private async Task BroadcastMessage(MessageResponse message)
        //{
        //    foreach (var observer in StreamObservers)
        //    {
        //        await observer.WriteAsync(message);
        //    }
        //}

        //private void BroadcastActiveUsers()
        //{
        //    var response = new JoinResponse
        //    {
        //        ActiveUsers = { ActiveUsers }
        //    };

        //    foreach (var observer in StreamJoinObservers)
        //    {
        //        observer.WriteAsync(response);
        //    }
        //}

        private static readonly List<string> ActiveUsers = new List<string>();
        private static readonly List<IServerStreamWriter<MessageResponse>> StreamObservers = new List<IServerStreamWriter<MessageResponse>>();
        private static readonly List<IServerStreamWriter<Message>> Subscribers = new List<IServerStreamWriter<Message>>();

        public override async Task Subscribe(IAsyncStreamReader<SubscribeRequest> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            var subscriberName = "";
            await foreach (var request in requestStream.ReadAllAsync())
            {
                subscriberName = request.SubscriberName;
                Subscribers.Add(responseStream);
            }

            // 等待客戶端斷開連線            
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);  // 或使用其他適當的延遲時間
            }

            // 客戶端斷開連線後，移除訂閱者
            Subscribers.RemoveAll(s => s == responseStream);
        }

        public override async Task<Empty> SendMessage2(Message request, ServerCallContext context)
        {
            // 向所有訂閱者推送消息
            foreach (var subscriber in Subscribers)
            {
                await subscriber.WriteAsync(request);
            }

            return new Empty { };
        }

        public override async Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            var response = new MessageResponse
            {
                Username = request.Username,
                Message = request.Message
            };

            var pushMessage = new Message() { Content = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + response.Username +"：" + response.Message };
            await BroadcastMessageAsync(pushMessage);
            //await BroadcastMessage(response);

            return response;
        }

        public override Task<Empty> Leave(LeaveRequest request, ServerCallContext context)
        {
            // 在用戶離開時，將其從活動用戶列表中刪除
            ActiveUsers.Remove(request.Username);            
            return Task.FromResult(new Empty());
        }

        private async Task BroadcastMessage(MessageResponse message)
        {
            foreach (var observer in StreamObservers)
            {
                await observer.WriteAsync(message);
            }
        }

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
                    // Handle exceptions (e.g., remove subscriber if communication fails)
                    Console.WriteLine($"Error broadcasting message to a subscriber: {ex.Message}");
                    Subscribers.Remove(subscriber);
                }
            }
        }
    }


}
