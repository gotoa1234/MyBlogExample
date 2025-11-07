using Microsoft.AspNetCore.SignalR;

namespace ReappearSignalRATaskWasCanceldExample.SignalR
{
    public class CustomerFilter : IHubFilter
    {
        public async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            var result = new object();            

            try
            {
                result = await next(invocationContext);
            }
            catch (Exception ex)
            {
                // 問題在此捕捉到 A Task was Canceled.
                Console.WriteLine(ex.Message);                
            }

            return result;
        }

        public Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            return next(context);
        }

        public Task OnDisconnectedAsync(
            HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next)
        {
            return next(context, exception);
        }
    }
}
