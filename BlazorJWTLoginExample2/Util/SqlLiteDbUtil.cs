using System.Data;
using System.Data.SQLite;
using Dapper;

namespace BlazorJWTLoginExample2.NewFolder
{
    /// <summary>
    /// 本地Sqlite DataBase連線
    /// </summary>
    public static class SqlLiteDbUtil
    {
        //1. 設定連線配置
        public const string DatabaseFileName = @"MyLogin.db";
        public const string ConnectionString = "Data Source=" + DatabaseFileName;

        static SqlLiteDbUtil()
        {
            Master = new SQLiteConnection(ConnectionString);
            CreateDatabaseIfNotExists();
        }

        public static IDbConnection Master { get; private set; }

        #region 2-1. 資料庫建構
        private static void CreateDatabaseIfNotExists()
        {
            if (!File.Exists(DatabaseFileName))
            {
                //Create Local Database
                Master.Open();

                //Initial Tables
                CreateDatabase();
            }

            //2-2. 建立登入表
            void CreateDatabase()
            {
                Master.Execute($@"
CREATE TABLE AccountToken (
    AccountName TEXT NOT NULL,
    Token TEXT NOT NULL,
	IsValid INT NOT NULL,
    LastDateTime DATETIME,
    PRIMARY KEY (AccountName)
);");
            }
            #endregion
        }
    }
}
