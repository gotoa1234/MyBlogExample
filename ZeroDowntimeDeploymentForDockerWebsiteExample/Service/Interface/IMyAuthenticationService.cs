using ZeroDowntimeDeploymentForDockerWebsiteExample.Models;

namespace ZeroDowntimeDeploymentForDockerWebsiteExample.Service.Interface
{
    public interface IMyAuthenticationService
    {
        Task SignInAsync(UserInfo userInfo);

        int GetCookieExpireHour();
    }
}
