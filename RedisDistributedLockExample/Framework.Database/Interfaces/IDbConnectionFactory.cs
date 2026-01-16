using Framework.Database.Enum;
using System.Data.Common;

namespace Framework.Database.Interfaces
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> CreateConnectionAsync(MysqlDbConnectionEnum dbSource, CancellationToken ct = default);
    }
}
