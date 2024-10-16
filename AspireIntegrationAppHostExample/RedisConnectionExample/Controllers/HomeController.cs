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
        /// 1. ���o����
        /// </summary>        
        public IActionResult Index()
        {
            ViewBag.ConnectionString = RedisConnectionStr();
            ViewBag.Message = RedisConnection();
            return View();
        }

        /// <summary>
        /// 2. ����s���r��
        /// </summary>        
        private string RedisConnectionStr()
        {
            _logger.LogInformation(_redis.Configuration);
            return _redis.Configuration ?? string.Empty;
        }

        /// <summary>
        /// 3. ����s�u���A
        /// </summary>        
        private string RedisConnection()
        {
            var message = string.Empty;
            try
            {
                var db = _redis.GetDatabase();

                // �ϥ� PING �R�O���ˬd�s�u
                var response = db.Execute("PING");

                if (response.ToString() == "PONG")
                {
                    message = "���\�s�u�� Redis ���A���I";
                }
                else
                {
                    message = "�L�k�T�{�s�u���A�A�^���G" + response;
                }
            }
            catch (RedisConnectionException ex)
            {
                message = "�L�k�s�u�� Redis ���A���G" + ex.Message;
            }
            catch (Exception ex)
            {
                message = "�o�Ϳ��~�G" + ex.Message;
            }
            finally
            {
                // �����s��
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
