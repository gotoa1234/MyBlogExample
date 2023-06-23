using BlazorJWTLoginExample2.NewFolder;
using Dapper;

namespace BlazorJWTLoginExample2.Repository
{
    public class SqliteRepository : ISqliteRepository
    {
        /// <summary>
        /// 1. 紀錄令牌
        /// </summary>
        public void InsertOrUpdateToken(string account, string token)
        {
            var sql = $@"
INSERT OR REPLACE INTO AccountToken (AccountName, Token, IsValid, LastDateTime)
                             VALUES (@AccountName, @Token, @IsValid, datetime('now', 'localtime'))
;";
            SqlLiteDbUtil.Master.Execute(sql, new { AccountName = account, IsValid = 1, Token = token });
        }

        /// <summary>
        /// 2. 取得令牌
        /// </summary>
        public string GetToken(string account)
        {
            var sql = $@"
SELECT Token
  FROM AccountToken
 WHERE AccountName = @AccountName
;";
            return SqlLiteDbUtil.Master.QueryFirstOrDefault<string>(sql, new { AccountName = account });
        }

        /// <summary>
        /// 3. 設成無效令牌
        /// </summary>        
        public void UpdateTokenInValid(string account, string token)
        {
            var sql = $@"
UPDATE AccountToken
   SET IsValid = @IsValid
 WHERE AccountName= @AccountName
   AND Token = @Token
;";
            SqlLiteDbUtil.Master.Execute(sql, new { AccountName = account, IsValid = 1, Token = token });
        }
    }
}
