using Framework.Database.Enum;
using Framework.Database.Interfaces;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Collections.Concurrent;
using System.Data.Common;

public class DbConnectionFactory(
    Dictionary<MysqlDbConnectionEnum, string> dbConnections,
    ILogger<DbConnectionFactory> logger) : IDbConnectionFactory
{
    private readonly ConcurrentDictionary<MysqlDbConnectionEnum, string> _connectionStringCache = new();

    public async Task<DbConnection> CreateConnectionAsync(MysqlDbConnectionEnum dbSource, CancellationToken ct = default)
    {
        // 取得連線字串 (使用快取避免頻繁讀取字典或處理字串)
        if (!_connectionStringCache.TryGetValue(dbSource, out var connString))
        {
            connString = await GetDbConnectionStringInternalAsync(dbSource, ct);
            _connectionStringCache.TryAdd(dbSource, connString);
        }

        var connection = new MySqlConnection(connString);

        return connection;
    }

    private Task<string> GetDbConnectionStringInternalAsync(MysqlDbConnectionEnum dbSource, CancellationToken ct = default)
    {
        // 從初始化時傳入的字典中獲取
        if (dbConnections.TryGetValue(dbSource, out var connString))
        {
            return Task.FromResult(connString);
        }

        var errorMsg = $"無法在設定中找到 {dbSource} 對應的連線字串。";
        logger.LogCritical(errorMsg);
        throw new KeyNotFoundException(errorMsg);
    }
}