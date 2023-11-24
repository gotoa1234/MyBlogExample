
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;


namespace NetCoreGRPCChattingRoomExample.Background
{
    public class TimeService
    {
        private readonly Channel<DateTime> _channel = Channel.CreateUnbounded<DateTime>();

        public ChannelReader<DateTime> Reader => _channel.Reader;

        public async Task StartProducingTimeAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                _channel.Writer.TryWrite(DateTime.Now);
            }
        }
    }

}
