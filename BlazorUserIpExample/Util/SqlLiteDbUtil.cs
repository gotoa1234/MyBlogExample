using System.Data;
using System.Data.SQLite;
using Dapper;

namespace BlazorUserIpExample.Util
{
    /// <summary>
    /// 本地Sqlite DataBase連線
    /// </summary>
    public static class SqlLiteDbUtil
    {
        public const string DatabaseFileName = @"MyUserLoginIp.db";
        public const string ConnectionString = "Data Source=" + DatabaseFileName;

        static SqlLiteDbUtil()
        {
            Master = new SQLiteConnection(ConnectionString);
            CreateDatabaseIfNotExists();
        }

        public static IDbConnection Master { get; private set; }

        #region 資料庫建構
        private static void CreateDatabaseIfNotExists()
        {
            if (!File.Exists(DatabaseFileName))
            {
                //Create Local Database
                Master.Open();

                //Initial Tables
                CreateDatabase();
            }

            void CreateDatabase()
            {
                Master.Execute($@"
CREATE TABLE UserIpRecord (
    UserIp TEXT NOT NULL,
    Date TEXT NOT NULL,
    CreatedTime DATETIME,
    PRIMARY KEY (UserIp, Date)
);");
            }
            #endregion
        }
    }
}
