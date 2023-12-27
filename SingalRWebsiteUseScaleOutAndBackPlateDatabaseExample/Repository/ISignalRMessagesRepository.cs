using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Model;

namespace SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Repository
{
    public interface ISignalRMessagesRepository
    {
        public Task<IEnumerable<SignalRMessagesEntity>> GetMessage(int siteNumber);

        public Task UpdateSended(string ids, int siteNumber);

        public Task InsertMessage(string userId, string message);
    }
}
