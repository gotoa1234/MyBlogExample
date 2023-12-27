using System.Data;

namespace SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.DbConnection
{
    public class MyDb : IMyDb
    {
        IDbConnection _master;
        public MyDb(IDbConnection dbConnection)
        {
            _master = dbConnection;
        }
        public IDbConnection Master
        {
            get { return _master; }
        }

    }
}
