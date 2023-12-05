using ChatApp;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NetCoreGRPCChattingRoomForHttpsExample.Controllers
{
    /// <summary>
    /// 全域配置
    /// </summary>
    public class GlobalConst
    {
        /// <summary>
        /// 加入聊天室的用戶會記錄於此
        /// </summary>
        public static Dictionary<string, string> DicMessages = new Dictionary<string, string>();

        public static string Self_GRPC_URL = "";
    }


    public class ChatController : Controller
    {
        public ChatController()
        {

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

            if (!GlobalConst.DicMessages.ContainsKey(user))
            {
                GlobalConst.DicMessages.Add(user, "");
                Task.Run(() => SubscribeToMessages(user));
            }

            var resultMessage = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "：";
            var messageResponse = new MessageResponse();
            var request = new MessageRequest { Username = user, Message = message };            


            // 1. 呼叫遠端 gRPC 傳送訊息
            using (var channel = GrpcChannel.ForAddress(GlobalConst.Self_GRPC_URL))
            {
                var client = new ChatApp.ChatService.ChatServiceClient(channel);
                using (var call = client.SendMessageAsync(request))
                {
                    messageResponse = await call.ResponseAsync;
                }
            }

            return Ok(new
            {
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
            //var handler = new HttpClientHandler();
            //handler.ClientCertificates.Add(new X509Certificate2(Encoding.UTF8.GetBytes(rootCert)));
            ////6. 測試環境可以開啟這個，因為憑證是我們產的測試Https 憑證，但生產需要將此行註解
            //handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            ////6. 建立HttpClient
            //var channelOptions = new GrpcChannelOptions
            //{
            //    HttpClient = new HttpClient(handler)
            //};

            using (var channel = GrpcChannel.ForAddress(GlobalConst.Self_GRPC_URL, channelOptions))
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
