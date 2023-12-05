using ChatApp;
using Grpc.Core;
using Grpc.Net.Client;
using System.Security.Cryptography.X509Certificates;
using System.Text;

//1. 將正確的 proto 複製到 Protos 資料夾下
//2. 加入服務參考 -> 用戶端 -> 最新的.proto 檔案
//3. 從Server 環境下取得憑證檔案 EX: /etc/nginx/cert.crt 
//4. 指向檔案路徑 (※這個是測試憑證，基本上沒什麼用處)
var rootCert = File.ReadAllText(@"C:\Users\louis671\Desktop\Example\新增資料夾\MyBlogExample\NetCoreGRPCChattingRoomForHttpsExample\HttpsGrpcConnetionTest\Certification\cert.crt");


//5. 設定憑證

var handler = new HttpClientHandler();
handler.ClientCertificates.Add(new X509Certificate2(Encoding.UTF8.GetBytes(rootCert)));
//6. 測試環境可以開啟這個，因為憑證是我們產的測試Https 憑證，但生產需要將此行註解
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

//6. 建立HttpClient
var channelOptions = new GrpcChannelOptions
{
    HttpClient = new HttpClient(handler)
};

using var channel = GrpcChannel.ForAddress("https://192.168.51.62:50051", channelOptions);

//8. 建立聯繫
var client =  new ChatApp.ChatService.ChatServiceClient(channel);

var sendResponseStream = client.SendMessageAsync(new MessageRequest() { Message = "Hello", Username = "Louis" });

using (var subScribeResponseStream = client.Subscribe())
{
    using (var subscribeCall = client.Subscribe())
    {
        var subscribeRequest = new SubscribeRequest { SubscriberName = "Https Subscriber From Client" };
        await subscribeCall.RequestStream.WriteAsync(subscribeRequest);

        // 接收消息
        var receiveTask = Task.Run(async () =>
        {
            await foreach (var message in subscribeCall.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"Received message: {message.Content}");
            }
        });
        // 等待接收消息的任務完成
        await receiveTask;
    }
}