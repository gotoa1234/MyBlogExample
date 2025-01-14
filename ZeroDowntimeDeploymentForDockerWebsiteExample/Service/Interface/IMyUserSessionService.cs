using ZeroDowntimeDeploymentForDockerWebsiteExample.Models;

namespace ZeroDowntimeDeploymentForDockerWebsiteExample.Service.Interface
{
    public interface IMyUserSessionService
    {
        Task SaveUserSessionAsync(UserInfo userInfo);
        Task<UserInfo> GetUserSessionAsync(int userId);
        Task RemoveUserSessionAsync(int userId);
    }
}
