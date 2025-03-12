using ZeroDowntimeDeploymentForDockerWebsiteExample.Service.Interface;

namespace ZeroDowntimeDeploymentForDockerWebsiteExample.Service
{
    public class UserBalanceService : IUserBalanceService
    {
        private readonly ICacheService _cacheService;
        private readonly string _balanceName = "Balance_";

        public UserBalanceService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// 更新餘額
        /// </summary>                
        public async Task UpdateBalance(int userId, decimal amount)
        {            
            await _cacheService.SetAsync($"{_balanceName}{userId}", amount);
        }

        /// <summary>
        /// 取得當前餘額
        /// </summary>                
        public async Task<decimal> GetBalance(int userId, decimal amount)
        {
            return await _cacheService.GetAsync<decimal>($"{_balanceName}{userId}");
        }
    }
}
