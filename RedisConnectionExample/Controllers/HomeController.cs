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

        public IActionResult Index()
        {
            var db = _redis.GetDatabase();
            var temp = db.StringGet("louis");
            return View();
        }
    }
}
