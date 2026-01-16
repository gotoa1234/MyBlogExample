using Framework.Database.Enum;
using Framework.Database.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace Framework.Database.Implementations.Mysql
{
    public class MySqlUnitOfWorkFactory(
        IDbConnectionFactory connectionFactory,
        ILoggerFactory loggerFactory) : IUnitOfWorkFactory
    {
        public async Task<IUnitOfWork> CreateAsync(MysqlDbConnectionEnum dbSource, CancellationToken ct = default)
        {
            // 1. 取得連線
            var connection = await connectionFactory.CreateConnectionAsync(dbSource, ct);

            // 2. 建立 UoW 實例 
            var logger = loggerFactory.CreateLogger<MySqlUnitOfWork>();
            return new MySqlUnitOfWork(logger, (DbConnection)connection);
        }
    }
}
