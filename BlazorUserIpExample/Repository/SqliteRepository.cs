using BlazorUserIpExample.Model;
using BlazorUserIpExample.Util;
using Dapper;

namespace BlazorUserIpExample.Repository
{
    public class SqliteRepository : ISqliteRepository
    {
        /// <summary>
        /// 更新IP資料
        /// </summary>
        public void CreatedRow(UserIpRecordModel row)
        {
            var sql = $@"
INSERT OR REPLACE INTO UserIpRecord (UserIp, Date, CreatedTime)
                             VALUES (@UserIp, @Date,  datetime('now', 'localtime'))
;";
            SqlLiteDbUtil.Master.Execute(sql, row);
        }
    }
}
