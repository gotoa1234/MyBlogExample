using ChatApp;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreGRPCChattingRoomExample.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatApp.ChatService.ChatServiceClient _grpcClient;

        public ChatController(ChatApp.ChatService.ChatServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendMessage(string user, string message)
        {
            // 訂閱消息
            var request = new MessageRequest { Username = user, Message = message };            
            //_grpcClient.Subscribe(new SubscribeRequest { SubscriberName = user });

            var resultMessage = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "：";
            var messageResponse = new MessageResponse();
            using (var call = _grpcClient.SendMessageAsync(request))
            {
                messageResponse = await call.ResponseAsync;
            }

            using (var call = _grpcClient.SendMessage2Async(new Message { Content = messageResponse.Username + ":" + messageResponse.Message }))
            {
               var messageResponse2 = await call.ResponseAsync;
            }

            return Ok(new {
                time = resultMessage,
                response = messageResponse
            });
        }
    }
}

