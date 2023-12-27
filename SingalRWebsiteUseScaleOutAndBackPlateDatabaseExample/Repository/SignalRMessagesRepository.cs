using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.DbConnection;
using Dapper;
using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Model;

namespace SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Repository
{
    public class SignalRMessagesRepository : ISignalRMessagesRepository
    {
        private IMyDb _myDb;
        public SignalRMessagesRepository(IMyDb myDb)
        {
            _myDb = myDb;
        }
        
        /// 1. 取得資料庫聊天室訊息變化
        public async Task<IEnumerable<SignalRMessagesEntity>> GetMessage(int siteNumber)
        {
            var sql = $@"
 SELECT	SignalRMessagesId,
	    UserId,
	    Message,
	    SiteValues,
	    CreateTime,
	    UpdateTime
  FROM signalrmessages
 WHERE ( SiteValues & @Number ) = 0

";
            return await _myDb.Master.QueryAsync<SignalRMessagesEntity>(
                sql, new { Number = siteNumber });
            
        }

        /// 2. SignalR Server 發送後，更新自己的紀錄
        public async Task UpdateSended(string ids, int siteNumber)
        {
            try
            {
                if (_myDb.Master.State == System.Data.ConnectionState.Closed)
                {
                    _myDb.Master.Open();
                }
                
                using (var transaction = _myDb.Master.BeginTransaction())
                {
                    try
                    {
                        var sql = $@"
 UPDATE signalrmessages
    SET SiteValues = SiteValues + @Number ,
        UpdateTime = NOW()
  WHERE SignalRMessagesId IN (
  {ids}
)
";
                        await _myDb.Master.ExecuteAsync(sql, new { Number = siteNumber });

                        // 提交事務
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // 如果有異常發生，進行回滾
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            { 
            }
        }

        /// 3. 聊天室發送訊息時，產生一筆紀錄到資料表中
        public async Task InsertMessage(string userId, string message)
        {
            var sql = $@"
INSERT INTO signalrmessages (UserId, Message) 
VALUES (@USERID, @MESSAGE); 
";
            await _myDb.Master.ExecuteAsync(sql, 
                new {
                    USERID = userId,
                    MESSAGE = message
                }
             );
        }
    }
}
