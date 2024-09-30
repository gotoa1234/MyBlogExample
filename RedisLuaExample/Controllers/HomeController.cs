using Microsoft.AspNetCore.Mvc;
using RedisLuaExample.Util;
using StackExchange.Redis;
using System.Diagnostics;

namespace RedisLuaExample.Controllers
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
            MultiExecExample();
            return View();
        }  

        private void LuaExample()
        {
            var db = _redis.GetDatabase();

        }

        private void MultiExecExample()
        { 
            var db = _redis.GetDatabase();

            var insertDatas = GenerateMillionDatas(1000);
            var sw = new Stopwatch();

            try
            {
                sw.Start();
                var batchSize = 100;
                for (int i = 0; i < insertDatas.Count(); i += batchSize)
                {
                    var transaction = db.CreateTransaction();
                    var batch = insertDatas.Skip(i).Take(batchSize);
                    foreach (var item in batch)
                    {
                        transaction.HashSetAsync(item.RedisKey, item.HashKey, item.Data);
                    }
                    // 提交事務
                    bool committed = transaction.Execute();
                    Console.WriteLine(committed ? $"Batch starting at {i} committed." : $"Batch starting at {i} failed.");
                }
                sw.Stop();
                var timeMessage = $@"[Pipeline] {insertDatas.Count()} 筆資料 - 總計花費時間：{sw.Elapsed.TotalSeconds} 秒";
                Console.WriteLine(timeMessage);
            }
            catch(Exception ex)
            {                
                Console.WriteLine(ex.Message);
            }
        }

        private List<RedisHashSetModel> GenerateMillionDatas(int countSize = 50000)
        {
            var testCollection = new List<RedisHashSetModel>();

            var accountIds = GetAccountIds(accountSize: 30);           
            for (int index = 0; index < countSize; index++)
            {
                foreach (var accountId in accountIds)
                {
                    var hashSetKey = index;
                    var redisKey = $@"Account:{accountId}";
                    testCollection.Add(new RedisHashSetModel()
                    {
                        HashKey = $@"{index}",
                        RedisKey = redisKey,
                        Data = CommonUtil.GetTestJSon
                    });
                }
            }

            List<int> GetAccountIds(int accountSize = 30)
            { 
                var result = new List<int>();
                for (int accountId = 0; accountId < accountSize; accountId++)
                {
                    result.Add(accountId);
                }
                return result;
            }

            return testCollection;
        }

        public class RedisHashSetModel
        {
            public string RedisKey { get; set; } = string.Empty;

            public string HashKey { get; set; } = string.Empty;

            public string Data { get; set; } = string.Empty;
        }
    }
}
