namespace RedisLuaExample.Model
{
    public class RedisHashSetModel
    {
        /// <summary>
        /// Redis Key
        /// </summary>
        public string RedisKey { get; set; } = string.Empty;

        /// <summary>
        /// 雜湊Key
        /// </summary>
        public string HashKey { get; set; } = string.Empty;

        /// <summary>
        /// 實際資料 - Redis 存儲格式種類眾多
        /// </summary>
        public string Data { get; set; } = string.Empty;
    }
}
