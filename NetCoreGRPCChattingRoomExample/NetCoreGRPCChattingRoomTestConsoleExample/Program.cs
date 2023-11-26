using System.Threading.Tasks;
using ChatApp;
using Grpc.Core;
using Grpc.Net.Client;
using (var channel = GrpcChannel.ForAddress("https://localhost:7184"))
{
    var client = new ChatApp.ChatService.ChatServiceClient(channel);

    // 訂閱消息
    using (var subscribeCall = client.Subscribe())
    {
        var subscribeRequest = new SubscribeRequest { SubscriberName = "ClientAppConsoleSubscriber" };
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