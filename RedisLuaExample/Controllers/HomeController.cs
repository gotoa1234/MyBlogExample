using Microsoft.AspNetCore.Mvc;
using RedisLuaExample.Service;

namespace RedisLuaExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGeneratorService _generator;

        public HomeController(ILogger<HomeController> logger,
            IGeneratorService generator)
        {
            _logger = logger;
            _generator = generator;
        }

        /// <summary>
        /// �������
        /// </summary>
        public async Task<IActionResult> Index()
        {            
            var getResult = await _generator.GeneratorReport();
            return View(getResult);
        }
        

        //private void LuaExample()
        //{
        //    var db = _redis.GetDatabase();
        //}

        ///// <summary>
        ///// �妸 & �޹D�Ҧ�
        ///// </summary>
        //private void MultiExecPipelineAndBatchExample()
        //{
        //    var db = _redis.GetDatabase();
        //    var batch = db.CreateBatch();
        //    List<Task> tasks = new List<Task>();

        //    for (int i = 0; i < 30000; i++)
        //    {
        //        string hashKey = $"Account:{i % 30}";
        //        string field = (i % 1000).ToString();
        //        string value = CommonUtil.GetTestJSon; //JsonConvert.SerializeObject(new { test = 123, data = new string('x', 1000) });
        //        tasks.Add(batch.HashSetAsync(hashKey, field, value));
        //    }

        //    batch.Execute();
        //    Task.WhenAll(tasks).Wait();
        //    // ��O 42 �� 30000����� - �޹D�˱��l�ʨϥβ����B�z
        //}

        ///// <summary>
        ///// ����Ҧ�
        ///// </summary>
        //private void MultiExecTrancationExample()
        //{ 
        //    var db = _redis.GetDatabase();

        //    var insertDatas = GenerateMillionDatas(1000);
        //    var sw = new Stopwatch();

        //    try
        //    {
        //        sw.Start();
        //        var batchSize = 100;
        //        for (int i = 0; i < insertDatas.Count(); i += batchSize)
        //        {
        //            var transaction = db.CreateTransaction();
        //            var batch = insertDatas.Skip(i).Take(batchSize);
        //            foreach (var item in batch)
        //            {
        //                transaction.HashSetAsync(item.RedisKey, item.HashKey, item.Data);
        //            }
        //            // ����ư�
        //            bool committed = transaction.Execute();
        //            Console.WriteLine(committed ? $"Batch starting at {i} committed." : $"Batch starting at {i} failed.");
        //        }
        //        sw.Stop();
        //        var timeMessage = $@"[Pipeline] {insertDatas.Count()} ����� - �`�p��O�ɶ��G{sw.Elapsed.TotalSeconds} ��";
        //        Console.WriteLine(timeMessage);
        //    }
        //    catch(Exception ex)
        //    {                
        //        Console.WriteLine(ex.Message);
        //    }
        //    // ��O 60 �� 30000�� - �O�d��l��
        //}

        ///// <summary>
        ///// Gzip ���Y & �޹D�Ҧ�
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //private void MultiPipelineCompressExample()
        //{
        //    var db = _redis.GetDatabase();
        //    var batch = db.CreateBatch();
        //    List<Task> tasks = new List<Task>();

        //    for (int i = 0; i < 30000; i++)
        //    {
        //        string hashKey = $"Account:{i % 30}";
        //        string field = (i % 1000).ToString();
        //        string json = CommonUtil.GetFormalJson; //CommonUtil.GetTestJSon;
        //        byte[] compressed = CommonUtil.Compress(json);
        //        tasks.Add(batch.HashSetAsync(hashKey, field, compressed));
        //    }

        //    batch.Execute();
        //    Task.WhenAll(tasks).Wait();
        //    // ��O 28 ��
        //    // ����������� ��O 11 ��
        //}

        ///// <summary>
        ///// MessagePack �G�i���� & �޹D�Ҧ�
        ///// </summary>
        //private void MultiPipelineMessagePackExample()
        //{
        //    var db = _redis.GetDatabase();
        //    var batch = db.CreateBatch();
        //    var tasks = new Task[30000];

        //    for (int i = 0; i < 30000; i++)
        //    {
        //        string hashKey = $"Account:{i % 30}";
        //        string field = (i % 1000).ToString();
        //        var data = CommonUtil.GetFormalJson;
        //        byte[] value = MessagePackSerializer.Serialize(data);
        //        tasks[i] = batch.HashSetAsync(hashKey, field, value);
        //    }

        //    batch.Execute();
        //    Task.WhenAll(tasks).Wait();
        //    // ��O 22 ��
        //}

        ///// <summary>
        ///// Gzip + MessagePack �G�i���� & �޹D�Ҧ�
        ///// </summary>
        //private void MultiPipelineGzipAndMessagePackExample()
        //{
        //    var db = _redis.GetDatabase();
        //    var batch = db.CreateBatch();
        //    var tasks = new Task[30000];

        //    for (int i = 0; i < 30000; i++)
        //    {
        //        string hashKey = $"Account:{i % 30}";
        //        string field = (i % 1000).ToString();
        //        var data = CommonUtil.GetFormalJson;
        //        byte[] value = MessagePackSerializer.Serialize(data);
        //        byte[] compressed = Compress(value);
        //        tasks[i] = batch.HashSetAsync(hashKey, field, compressed);
        //    }

        //    batch.Execute();
        //    Task.WhenAll(tasks).Wait();
        //    // ��O 18 ��
        //    var temp = 1;


        //    byte[] Compress(byte[] data)
        //    {
        //        using (var mso = new MemoryStream())
        //        {
        //            using (var gzip = new GZipStream(mso, CompressionLevel.Optimal))
        //            {
        //                gzip.Write(data, 0, data.Length);
        //            }
        //            return mso.ToArray();
        //        }
        //    }
        //}

        //public void BulkLoadData()
        //{
        //    var db = _redis.GetDatabase();

        //    // �ǳƼƾ�
        //    StringBuilder sb = new StringBuilder();
        //    using (var writer = new StreamWriter($@"D:\bulk_data.txt"))
        //    {
        //        for (int i = 0; i < 30000; i++)
        //        {
        //            string key = $"Account:{i / 1000}";
        //            string field = $"{i % 1000}";
        //            string value = CommonUtil.GetFormalJson; // ���]�� JSON ��
        //            writer.WriteLine($"HSET \"{key}\" \"{field}\" \"{value}\"");
        //        }
        //    }

        //    // Use redis-cli to import the data
        //    using (var process = new System.Diagnostics.Process())
        //    {
        //        process.StartInfo.FileName = $@"D:\Redis-7.4.1-Windows-x64-msys2\redis-cli.exe";
        //        process.StartInfo.Arguments = "--pipe";
        //        process.StartInfo.RedirectStandardInput = true; // �ҥμзǿ�J���w�V
        //        process.StartInfo.UseShellExecute = false; // �T�Υ~�߰���
        //        process.StartInfo.CreateNoWindow = true; // ���Ыص��f
        //        process.StartInfo.Arguments = "cat D:\\bulk_data.txt | redis-cli --pipe";
        //        process.Start();
               

        //        process.StandardInput.Close(); // �����зǿ�J
        //        process.WaitForExit(); // ���ݶi�{����
        //    }
        //}

        //private string GenerateRedisCommand(string command, params string[] args)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append($"*{args.Length + 1}\r\n");
        //    sb.Append($"${command.Length}\r\n{command}\r\n");
        //    foreach (var arg in args)
        //    {
        //        sb.Append($"${arg.Length}\r\n{arg}\r\n");
        //    }
        //    return sb.ToString();
        //}

 

    }
}
