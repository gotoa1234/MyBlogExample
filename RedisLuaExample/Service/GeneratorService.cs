using MessagePack;
using RedisLuaExample.Model;
using RedisLuaExample.Util;
using StackExchange.Redis;
using System.Diagnostics;
using System.IO.Compression;

namespace RedisLuaExample.Service
{
    public class GeneratorService : IGeneratorService
    {
        private readonly IConnectionMultiplexer _redisDb;
        private readonly IDatabase _db;
        private readonly StackExchange.Redis.IServer _server;        

        public GeneratorService(IConnectionMultiplexer redisDb, IConfiguration config)
        {            
            _redisDb = redisDb;
            _db = redisDb.GetDatabase();
            _server = redisDb.GetServer(_redisDb.GetEndPoints()[0]);
        }

        /// <summary>
        /// 產生效能報告
        /// </summary>
        public async Task<string> GeneratorReport()
        {
            var infos = new List<string>();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var useData = GenerateImmenseData(1000);
            infos.Add($@"執行產生資料 {nameof(GenerateImmenseData)}方法，資料數：{useData.Count()}，耗費：{sw.Elapsed.TotalMilliseconds} 毫秒");
            sw.Stop();

            sw.Restart();
            MultiExecTrancationExample(useData);
            sw.Stop();
            infos.Add($@"1. 執行【交易模式】 {nameof(MultiExecTrancationExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
         
            sw.Restart();
            MultiExecPipelineAndBatchExample(useData);
            sw.Stop();
            infos.Add($@"2. 執行【管道批次】 {nameof(MultiExecPipelineAndBatchExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
         
            sw.Restart();
            MultiPipelineCompressExample(useData);
            sw.Stop();
            infos.Add($@"3. 執行【Gzip 壓縮 & 批次 & 管道模式】 {nameof(MultiPipelineCompressExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
           
            sw.Restart();
            MultiPipelineMessagePackExample(useData);
            sw.Stop();
            infos.Add($@"4. 執行【MessagePack 二進制資料 & 批次 & 管道模式】 {nameof(MultiPipelineMessagePackExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
            
            sw.Restart();
            MultiPipelineGzipAndMessagePackExample(useData);
            sw.Stop(); 
            infos.Add($@"5. 執行【Gzip + MessagePack 二進制資料 & 批次 & 管道模式】 {nameof(MultiPipelineGzipAndMessagePackExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
           
            sw.Restart();
            await UseLuaScript(useData);
            sw.Stop();
            infos.Add($@"6. 執行【使用 Lua 腳本，每 3000 筆為 1 組批次寫入】 {nameof(UseLuaScript)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
     
            return string.Join(Environment.NewLine, infos);
        }

        private async Task ClearDataBase()
        {
            await _server.FlushDatabaseAsync();
        }

        /// <summary>
        /// 1. 交易模式
        /// </summary>
        private void MultiExecTrancationExample(List<RedisHashSetModel> insertData)
        {
            var batchSize = 100;
            for (int i = 0; i < insertData.Count(); i += batchSize)
            {
                var transaction = _db.CreateTransaction();
                var batch = insertData.Skip(i).Take(batchSize);
                foreach (var item in batch)
                {
                    transaction.HashSetAsync(item.RedisKey, item.HashKey, item.Data);
                }
                // 提交事務
                bool committed = transaction.Execute();
            }
        }

        /// <summary>
        /// 2. 批次 & 管道模式
        /// </summary>
        private void MultiExecPipelineAndBatchExample(List<RedisHashSetModel> insertData)
        {            
            var batch = _db.CreateBatch();
            List<Task> tasks = new List<Task>();

            foreach (var item in insertData)
            {
                tasks.Add(batch.HashSetAsync(item.RedisKey, item.HashKey, item.Data));
            }
            batch.Execute();
            Task.WhenAll(tasks).Wait();
        }

        /// <summary>
        /// 3. Gzip 壓縮 & 批次 & 管道模式
        /// </summary>
        private void MultiPipelineCompressExample(List<RedisHashSetModel> insertData)
        {            
            var batch = _db.CreateBatch();
            List<Task> tasks = new List<Task>();

            foreach (var item in insertData)
            {
                byte[] gzipCompressed = CommonUtil.Compress(item.Data);
                tasks.Add(batch.HashSetAsync(item.RedisKey, item.HashKey, gzipCompressed));
            }
            batch.Execute();
            Task.WhenAll(tasks).Wait();           
        }

        /// <summary>
        /// 4. MessagePack 二進制資料 & 批次 & 管道模式
        /// </summary>
        private void MultiPipelineMessagePackExample(List<RedisHashSetModel> insertData)
        {
            var batch = _db.CreateBatch();
            var tasks = new List<Task>();

            foreach (var item in insertData)
            {
                byte[] gzipCompressed = MessagePackSerializer.Serialize(item.Data);
                tasks.Add(batch.HashSetAsync(item.RedisKey, item.HashKey, gzipCompressed));
            }
            batch.Execute();
            Task.WhenAll(tasks).Wait();  
        }

        /// <summary>
        /// 5. Gzip + MessagePack 二進制資料 & 批次 & 管道模式
        /// </summary>
        private void MultiPipelineGzipAndMessagePackExample(List<RedisHashSetModel> insertData)
        {
            var batch = _db.CreateBatch();
            var tasks = new List<Task>();

            foreach (var item in insertData)
            {
                byte[] gzipCompressed = MessagePackSerializer.Serialize(item.Data);
                tasks.Add(batch.HashSetAsync(item.RedisKey, item.HashKey, Compress(gzipCompressed)));
            }
            batch.Execute();
            Task.WhenAll(tasks).Wait();

            byte[] Compress(byte[] data)
            {
                using (var mso = new MemoryStream())
                {
                    using (var gzip = new GZipStream(mso, CompressionLevel.Optimal))
                    {
                        gzip.Write(data, 0, data.Length);
                    }
                    return mso.ToArray();
                }
            }
        }


        /// <summary>
        /// 6. 使用 Lua 腳本，每3000 筆為 1 組批次寫入
        /// </summary>        
        private async Task UseLuaScript(List<RedisHashSetModel> insertData)
        {
            var accountGroups = insertData.GroupBy(x => x.RedisKey);
            int totalHashes = insertData.Count();
            int luaSize = 3000;//Server 如果沒有限制預設值是 8000

            var keys = new List<RedisKey>();
            var argv = new List<RedisValue>();

            int count = 0;
            foreach (var item in accountGroups)
            {
                keys.Add(item.Key);
                foreach (var hashData in item)
                {
                    argv.Add(hashData.HashKey);
                    argv.Add(hashData.Data);
                    count++;
                    // 當達到批次大小或最後一個 hash key，執行 Lua 腳本
                    if ((count + 1) % luaSize == 0 || count == totalHashes - 1)
                    {
                        var currentKeys = keys.ToArray();
                        var currentArgv = argv.ToArray();

                        var result = await _db.ScriptEvaluateAsync(GetluaScript(), currentKeys, currentArgv);
                        argv.Clear();
                    }
                }
                // 當達到批次大小或最後一個 hash key，執行 Lua 腳本
                if ((count + 1) % luaSize == 0 || count == totalHashes - 1)
                {
                    var currentKeys = keys.ToArray();
                    var currentArgv = argv.ToArray();

                    var result = await _db.ScriptEvaluateAsync(GetluaScript(), currentKeys, currentArgv);
                    keys.Clear();
                    argv.Clear();
                }
            }

            // Lua 腳本，簡單的 HashKey Value
            string GetluaScript()
            {
                return @"
        for i, key in ipairs(KEYS) do
            local args = {}
            for j = 1, #ARGV, 2 do
                table.insert(args, ARGV[j])      -- 字段
                table.insert(args, ARGV[j + 1])  -- 對應的值
            end
            -- 使用 HSET 一次設置多個字段
            redis.call('HSET', key, unpack(args))
        end
        return 'OK'
";
            }
        }

        /// <summary>
        /// 產生資料數
        /// </summary>       
        private List<RedisHashSetModel> GenerateImmenseData(int countSize = 1000, int accountSize = 30)
        {
            var testCollection = new List<RedisHashSetModel>();

            var accountIds = GetAccountIds(accountSize: accountSize);
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

            // 人數 
            List<int> GetAccountIds(int accountSize)
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
    }
}
