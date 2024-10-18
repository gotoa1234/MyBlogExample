using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace MysqlConnectionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MySqlConnection _mySqlConnection;

        public HomeController(ILogger<HomeController> logger,
            MySqlConnection mysql)
        {
            _logger = logger;
            _mySqlConnection = mysql;
        }

        /// <summary>
        /// 1. 取得頁面
        /// </summary>        
        public IActionResult Index()
        {
            ViewBag.ConnectionString = MysqlConnectionStr();
            ViewBag.Message = MysqlGetTableData();
            return View();
        }

        /// <summary>
        /// 2. 獲取連接字串
        /// </summary>        
        private string MysqlConnectionStr()
        {
            _logger.LogInformation(_mySqlConnection.ConnectionString);
            return _mySqlConnection.ConnectionString ?? string.Empty;
        }

        /// <summary>
        /// 3. 獲取連線狀態
        /// </summary>        
        private string MysqlGetTableData()
        {
            var message = string.Empty;
            _mySqlConnection.Open();
            try
            {
                var collectionData = new List<TestAspireTable>();
                var command = new MySqlCommand("SELECT SeqNo, Comment FROM testaspiretable", _mySqlConnection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // 讀取資料
                        var newItem = new TestAspireTable();
                        newItem.SeqNo = int.Parse(reader["SeqNo"].ToString());
                        newItem.Comment = reader["Comment"].ToString();
                        collectionData.Add(newItem);
                    }
                }
                // Json 序列化參數
                var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                message = JsonSerializer.Serialize(collectionData, options);
            }
            catch (Exception ex)
            {
                message = "發生錯誤：" + ex.Message;
            }
            finally
            {
                // 關閉連接
                if (_mySqlConnection != null)
                {
                    _mySqlConnection.Close();
                }
            }
            _logger.LogDebug(message);
            return message;
        }

        /// <summary>
        /// 測試 Mysql 連線的資料表
        /// </summary>
        private class TestAspireTable
        {
            public int SeqNo { get; set; }
            public string Comment { get; set; } = string.Empty;
        }
    }
}
