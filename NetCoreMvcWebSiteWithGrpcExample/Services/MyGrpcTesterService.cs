using Grpc.Core;
using NetCoreMvcWebSiteWithGrpcExample;

namespace NetCoreMvcWebSiteWithGrpcExample.Services
{
    public class MyGrpcTesterService : MyGrpcTester.MyGrpcTesterBase
    {
        private readonly ILogger<MyGrpcTesterService> _logger;
        public MyGrpcTesterService(ILogger<MyGrpcTesterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}