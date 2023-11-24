using System.Threading.Tasks;
using ChatApp;
using Grpc.Core;
using Grpc.Net.Client;
using NetCoreMvcWebSiteWithGrpcExample;

//// The port number must match the port of the gRPC server.
//using var channel = GrpcChannel.ForAddress("http://localhost:5262");
//var client = new MyGrpcTester.MyGrpcTesterClient(channel);
//var reply = await client.SayHelloAsync(
//                  new HelloRequest { Name = "GreeterClient" });
//Console.WriteLine("Greeting: " + reply.Message);
//Console.WriteLine("Press any key to exit...");
//Console.ReadKey();


//using (var channel = GrpcChannel.ForAddress("https://localhost:7184"))
//{
//    var client = new ChatApp.ChatService.ChatServiceClient(channel);

//    // 訂閱消息
//    using (var call = client.Subscribe())
//    {
//        var subscribeRequest = new SubscribeRequest { SubscriberName = "ClientSubscriber" };
//        await call.RequestStream.WriteAsync(subscribeRequest);

//        await foreach (var message in call.ResponseStream.ReadAllAsync())
//        {
//            Console.WriteLine($"Received message: {message.Content}");
//        }
//    }
//}

using (var channel = GrpcChannel.ForAddress("https://localhost:7184"))
{
    var client = new ChatApp.ChatService.ChatServiceClient(channel);

    // 訂閱消息
    using (var subscribeCall = client.Subscribe())
    {
        var subscribeRequest = new SubscribeRequest { SubscriberName = "ClientSubscriber" };
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