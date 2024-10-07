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
            db.StringSet("louis", "123456");
            var temp = db.StringGet("louis");
            TestMultiExec();
            return View();
        }

        private void TestMultiExec()
        {
            try
            {

                var db = _redis.GetDatabase();

                // 使用事務
                var tran = db.CreateTransaction();

                tran.HashSetAsync("user:1001", "name", "Alice");
                tran.HashSetAsync("user:1001", "age", 30);
                tran.HashSetAsync("user:1002", "name", "Bob");
                tran.HashSetAsync("user:1002", "age", 25);

                // 提交事務
                bool committed = tran.Execute();
                Console.WriteLine(committed ? "Transaction committed." : "Transaction failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
