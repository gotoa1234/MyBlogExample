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

        public static string Cert_Path = "";
    }


    public class ChatController : Controller
    {
        private IConfigurationRoot _config;

        public ChatController(IConfigurationRoot config)
        {
            _config = config;
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


            // 1. 調整加入Https cert公開金鑰
            using (var channel = GetGRPChannel())
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
            //2. 訂閱也調整加入Https cert公開金鑰
            using (var channel = GetGRPChannel())
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

        /// <summary>
        /// 取得公開金鑰Cert
        /// </summary>
        /// <returns></returns>
        private static GrpcChannel? GetGRPChannel()
        {
            var rootCert = NetCoreGRPCChattingRoomForHttpsExample.Controllers.GlobalConst.Cert_Path;
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(new X509Certificate2(Encoding.UTF8.GetBytes(rootCert)));            
            //如果憑證為測試憑證必須加入這行，如果生產有產生正式的Https 憑證把此行註解即可
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channelOptions = new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            };
            var client = GrpcChannel.ForAddress(GlobalConst.Self_GRPC_URL, channelOptions);
            return client;
        }
    }
}
