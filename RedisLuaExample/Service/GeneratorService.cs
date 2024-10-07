using MessagePack;
using RedisLuaExample.Model;
using RedisLuaExample.Util;
using StackExchange.Redis;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;

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
            sw.Stop();
            infos.Add($@"執行產生資料 {nameof(GenerateImmenseData)}方法，資料數：{useData.Count()}，耗費：{sw.Elapsed.TotalMilliseconds} 毫秒");
            
            sw.Restart();            
            MultiExecTrancationExample(useData); 
            infos.Add($@"1. 執行【交易模式】 {nameof(MultiExecTrancationExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
            sw.Stop();

            sw.Restart();
            MultiExecPipelineAndBatchExample(useData);
            infos.Add($@"2. 執行【管道批次】 {nameof(MultiExecPipelineAndBatchExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
            sw.Stop();

            sw.Restart();
            MultiPipelineCompressExample(useData);
            infos.Add($@"3. 執行【Gzip 壓縮 & 批次 & 管道模式】 {nameof(MultiPipelineCompressExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
            sw.Stop();

            sw.Restart();
            MultiPipelineMessagePackExample(useData);
            infos.Add($@"4. 執行【MessagePack 二進制資料 & 批次 & 管道模式】 {nameof(MultiPipelineMessagePackExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
            sw.Stop();

            sw.Restart();
            MultiPipelineGzipAndMessagePackExample(useData);
            infos.Add($@"5. 執行【Gzip + MessagePack 二進制資料 & 批次 & 管道模式】 {nameof(MultiPipelineGzipAndMessagePackExample)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
            sw.Stop();

            sw.Restart();
            await UseLuaScript(useData);
            infos.Add($@"6. 執行【管道批次】 {nameof(UseLuaScript)}方法，耗費：{sw.Elapsed.TotalSeconds} 秒");
            sw.Stop();


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
            // 花費 23 秒 30000筆 - 保留原子性
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
            // 花費 22 秒 30000筆資料 - 管道捨棄原子性使用異部處理
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
            // 正式版的資料 花費 11 秒
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
                tasks.Add(batch.HashSetAsync(item.RedisKey, item.HashKey, gzipCompressed));
            }
            batch.Execute();
            Task.WhenAll(tasks).Wait();

            // 花費 18 秒
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
        /// 6. 使用 Lua腳本，每5組批次寫入
        /// </summary>        
//        private async Task UseLuaScript(List<RedisHashSetModel> insertData)
//        {
//            int batchSize = 5; // 每批處理 5 個 hash keys

//            // 初始化 KEYS 和 ARGV
//            var keys = new List<RedisKey>();
//            var argv = new List<RedisValue>();

//            // 根據 RedisKey 分組資料
//            var accountIdKeys = insertData.GroupBy(item => item.RedisKey);

//            foreach (var group in accountIdKeys)
//            {
//                // 將 RedisKey 添加到 KEYS
//                keys.Add(group.Key);

//                var fields = new StringBuilder();
//                var values = new StringBuilder();

//                // 為當前組中的每個項目構建 fields 和 values 字串
//                foreach (var item in group)
//                {                    
//                    fields.Append($"{item.HashKey}:{item.HashKey},"); // 假設 item.Field 是要設置的字段名
//                    values.Append($"{item.HashKey}:{item.Data},");
//                }

//                // 移除最後的逗號
//                var fieldsStr = fields.ToString().TrimEnd(',');
//                var valuesStr = values.ToString().TrimEnd(',');

//                argv.Add(fieldsStr);
//                argv.Add(valuesStr);

//                // 當達到批次大小或最後一個 hash key，執行 Lua 腳本
//                if (keys.Count % batchSize == 0 || group == accountIdKeys.Last())
//                {
//                    var currentKeys = keys.ToArray();
//                    var currentArgv = argv.ToArray();

//                    var result = await _db.ScriptEvaluateAsync(GetluaScript(), currentKeys, currentArgv);
//                    Console.WriteLine(result); // lua 輸出 "OK"

//                    // 重置批次
//                    keys.Clear();
//                    argv.Clear();
//                }
//            }

//            string GetluaScript()
//            {
//                return @"
//for i, key in ipairs(KEYS) do
//    local fields = {}
//    local values = {}
    
//    for field, value in string.gmatch(ARGV[(i-1)*2 + 1], '([^:]+):([^,]+)') do
//        table.insert(fields, field)
//        table.insert(values, value)
//    end
    
//    -- 合併 fields 和 values 為一個參數列表
//    local args = {}
//    for j = 1, #fields do
//        table.insert(args, fields[j])
//        table.insert(args, values[j])
//    end
    
//    -- 使用 HSET 一次設置多個字段
//    redis.call('HSET', key, unpack(args))
//end
//return 'OK'
//    ";
//            }
//        }


        private async Task UseLuaScript(List<RedisHashSetModel> insertData)
        {
            int totalHashes = 30;
            int fieldsPerHash = 1000;
            int batchSize = 2; // 每批處理 5 個 hash keys

            // 初始化 KEYS 和 ARGV
            var keys = new List<RedisKey>();
            var argv = new List<RedisValue>();

            for (int index = 0; index < totalHashes; index++)
            {
                keys.Add($"Account:{index}");

                for (int j = 0; j < fieldsPerHash; j++)
                {
                    var serialized = CommonUtil.GetFormalJson; // 確保這裡是方法調用
                    argv.Add($"{j}");        // 將字段添加到 ARGV
                    argv.Add(CommonUtil.Compress(serialized));        // 將對應的值添加到 ARGV
                }

                // 當達到批次大小或最後一個 hash key，執行 Lua 腳本
                if ((index + 1) % batchSize == 0 || index == totalHashes - 1)
                {
                    var currentKeys = keys.ToArray();
                    var currentArgv = argv.ToArray();

                    var result = await _db.ScriptEvaluateAsync(GetluaScript(), currentKeys, currentArgv);
                    Console.WriteLine(result); // lua 輸出 "OK"

                    // 重置批次
                    keys.Clear();
                    argv.Clear();
                }
            }

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
                        Data = CommonUtil2.GetFormalJson//CommonUtil.GetTestJSon
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
