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
        /// 1. ���o����
        /// </summary>        
        public IActionResult Index()
        {
            ViewBag.ConnectionString = MysqlConnectionStr();
            ViewBag.Message = MysqlGetTableData();
            return View();
        }

        /// <summary>
        /// 2. ����s���r��
        /// </summary>        
        private string MysqlConnectionStr()
        {
            _logger.LogInformation(_mySqlConnection.ConnectionString);
            return _mySqlConnection.ConnectionString ?? string.Empty;
        }

        /// <summary>
        /// 3. ����s�u���A
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
                        // Ū�����
                        var newItem = new TestAspireTable();
                        newItem.SeqNo = int.Parse(reader["SeqNo"].ToString());
                        newItem.Comment = reader["Comment"].ToString();
                        collectionData.Add(newItem);
                    }
                }
                // Json �ǦC�ưѼ�
                var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                message = JsonSerializer.Serialize(collectionData, options);
            }
            catch (Exception ex)
            {
                message = "�o�Ϳ��~�G" + ex.Message;
            }
            finally
            {
                // �����s��
                if (_mySqlConnection != null)
                {
                    _mySqlConnection.Close();
                }
            }
            _logger.LogDebug(message);
            return message;
        }

        /// <summary>
        /// ���� Mysql �s�u����ƪ�
        /// </summary>
        private class TestAspireTable
        {
            public int SeqNo { get; set; }
            public string Comment { get; set; } = string.Empty;
        }
    }
}
