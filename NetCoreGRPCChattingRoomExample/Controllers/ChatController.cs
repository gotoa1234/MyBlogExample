using ChatApp;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreGRPCChattingRoomExample.Controllers
{
    /// <summary>
    /// 全域配置
    /// </summary>
    public class GlobalConst {

        /// <summary>
        /// 加入聊天室的用戶會記錄於此
        /// </summary>
        public static Dictionary<string, string> DicMessages = new Dictionary<string, string>();

        public static string Self_GRPC_URL = "";
    }
    
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

        /// <summary>
        /// 傳送訊息 + 推播給所有訂閱用戶
        /// </summary>        
        [HttpGet]
        public async Task<IActionResult> SendMessage(string user, string message)
        {
            // 1. 訂閱消息 - 簡單用 Static 做為當前Server訂閱(加入聊天室)的人
            if (!GlobalConst.DicMessages.ContainsKey(user))
            {
                GlobalConst.DicMessages.Add(user, "");
                Task.Run(() => SubscribeToMessages(user));
            }

            //2. 組成返回資訊
            var resultMessage = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "：";
            var messageResponse = new MessageResponse();
            var request = new MessageRequest { Username = user, Message = message };
            using (var call = _grpcClient.SendMessageAsync(request))
            {
                messageResponse = await call.ResponseAsync;
            }

            //3. 回傳訊息給叫用API
            return Ok(new {
                time = resultMessage,
                response = messageResponse
            });
        }

        /// <summary>
        /// 取得當前訂閱內容
        /// </summary>        
        [HttpGet]
        public string GetMessage(string user)
        {
            if (user == null)
                return "";
           return GlobalConst.DicMessages.ContainsKey(user) ? GlobalConst.DicMessages[user] : "";
        }

        /// <summary>
        /// 訂閱
        /// </summary>        
        private static async Task SubscribeToMessages(string username)
        {
            using (var channel = GrpcChannel.ForAddress(GlobalConst.Self_GRPC_URL))
            {
                var client = new ChatApp.ChatService.ChatServiceClient(channel);

                // 訂閱消息
                using (var subscribeCall = client.Subscribe())
                {
                    var subscribeRequest = new SubscribeRequest { SubscriberName = username };
                    await subscribeCall.RequestStream.WriteAsync(subscribeRequest);

                    // 接收消息
                    await foreach (var message in subscribeCall.ResponseStream.ReadAllAsync())
                    {
                        GlobalConst.DicMessages[username] += message.Content;
                    }
                }
            }
        }
    }
}

