using System.Data;

namespace SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.DbConnection
{
    public interface IMyDb
    {
        IDbConnection Master { get; }
    }
}
