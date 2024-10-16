using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisConnectionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConnectionMultiplexer _redis;

        public HomeController(ILogger<HomeController> logger,
            IConnectionMultiplexer redisDb)
        {
            _logger = logger;
            _redis = redisDb;
        }

        /// <summary>
        /// 1. 取得頁面
        /// </summary>        
        public IActionResult Index()
        {
            ViewBag.ConnectionString = RedisConnectionStr();
            ViewBag.Message = RedisConnection();
            return View();
        }

        /// <summary>
        /// 2. 獲取連接字串
        /// </summary>        
        private string RedisConnectionStr()
        {
            _logger.LogInformation(_redis.Configuration);
            return _redis.Configuration ?? string.Empty;
        }

        /// <summary>
        /// 3. 獲取連線狀態
        /// </summary>        
        private string RedisConnection()
        {
            var message = string.Empty;
            try
            {
                var db = _redis.GetDatabase();

                // 使用 PING 命令來檢查連線
                var response = db.Execute("PING");

                if (response.ToString() == "PONG")
                {
                    message = "成功連線到 Redis 伺服器！";
                }
                else
                {
                    message = "無法確認連線狀態，回應：" + response;
                }
            }
            catch (RedisConnectionException ex)
            {
                message = "無法連線到 Redis 伺服器：" + ex.Message;
            }
            catch (Exception ex)
            {
                message = "發生錯誤：" + ex.Message;
            }
            finally
            {
                // 關閉連接
                if (_redis != null)
                {
                    _redis.Close();
                }
            }
            _logger.LogDebug(message);
            return message;
        }
    }
}
