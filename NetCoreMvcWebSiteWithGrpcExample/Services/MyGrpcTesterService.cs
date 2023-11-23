using Grpc.Core;
using NetCoreMvcWebSiteWithGrpcExample;

namespace NetCoreMvcWebSiteWithGrpcExample.Services
{
    //1. 主類名稱 MyGrpcTesterService 其中MyGrpcTester與 mygrpctest.proto 配置的要相同
    public class MyGrpcTesterService : MyGrpcTester.MyGrpcTesterBase
    {
        private readonly ILogger<MyGrpcTesterService> _logger;
        public MyGrpcTesterService(ILogger<MyGrpcTesterService> logger)
        {
            _logger = logger;
        }

        //2. 測試代碼，HTTP/2 接口名稱 SayHello ，如果有人帶 HelloRequest 的請求參數
        //   我們返回 HelloReply 的返回內容
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}