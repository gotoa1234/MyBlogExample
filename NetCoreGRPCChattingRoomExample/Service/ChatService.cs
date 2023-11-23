using ChatApp;
using Grpc.Core;
using static ChatApp.ChatService;

namespace NetCoreGRPCChattingRoomExample.Service
{
    public class ChatService : ChatServiceBase
    {
        private static readonly List<string> ActiveUsers = new List<string>();
        private static readonly List<IServerStreamWriter<MessageResponse>> StreamObservers = new List<IServerStreamWriter<MessageResponse>>();
        private static readonly List<IServerStreamWriter<JoinResponse>> StreamJoinObservers = new List<IServerStreamWriter<JoinResponse>>();

        public override Task<JoinResponse> Join(JoinRequest request, ServerCallContext context)
        {
            ActiveUsers.Add(request.Username);
            BroadcastActiveUsers();

            var response = new JoinResponse
            {
                ActiveUsers = { ActiveUsers }
            };

            return Task.FromResult(response);
        }

        public override async Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            var response = new MessageResponse
            {
                Username = request.Username,
                Message = request.Message
            };

            await BroadcastMessage(response);

            return response;
        }


        private async Task BroadcastMessage(MessageResponse message)
        {
            foreach (var observer in StreamObservers)
            {
                await observer.WriteAsync(message);
            }
        }

        private void BroadcastActiveUsers()
        {
            var response = new JoinResponse
            {
                ActiveUsers = { ActiveUsers }
            };

            foreach (var observer in StreamJoinObservers)
            {
                observer.WriteAsync(response);
            }
        }
    }


}
