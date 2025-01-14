using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisSolveDistributedSessionExample.Service.IService;

namespace RedisSolveDistributedSessionExample.Service
{
    public class CacheService : ICacheService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDistributedCache _cache;
        private readonly double _expireTime = 1.5;

        public CacheService(
            IHttpContextAccessor httpContextAccessor,
            IDistributedCache cache)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        /// <summary>
        /// 1. 保存 Session、Redis
        /// </summary>                
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var jsonData = JsonConvert.SerializeObject(value);

            // 存到 Session
            _httpContextAccessor.HttpContext?.Session.SetString(key, jsonData);

            // 存到 Redis
            await _cache.SetStringAsync(key, jsonData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(_expireTime)
            });
        }

        /// <summary>
        /// 2. 讀取 Session、Redis
        /// </summary>
        public async Task<T> GetAsync<T>(string key)
        {
            // 先從 Session 取
            var sessionData = _httpContextAccessor.HttpContext?.Session.GetString(key);
            if (!string.IsNullOrEmpty(sessionData))
            {
                return JsonConvert.DeserializeObject<T>(sessionData);
            }

            // 從 Redis 取
            var redisData = await _cache.GetStringAsync(key);
            return string.IsNullOrEmpty(redisData)
                ? default(T)
                : JsonConvert.DeserializeObject<T>(redisData);
        }

        /// <summary>
        /// 3. 移除 Session, Redis 資料
        /// </summary>
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
