using Framework.Database.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Framework.Database.Implementations.Mysql
{
    public class MySqlUnitOfWork(ILogger logger, DbConnection connection)
        : IUnitOfWork
    {
        protected readonly ILogger Logger = logger;
        private DbTransaction? _transaction;
        private int _transactionCount;
        private bool _disposed;

        public IDbConnection Connection => connection;
        public IDbTransaction? Transaction => _transaction;

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(MySqlUnitOfWork));

            _transactionCount++;

            if (_transaction == null)
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync(cancellationToken);
                }

                // 啟動交易
                _transaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {

            if (_transaction == null) return;

            _transactionCount--;

            if (_transactionCount > 0) return;// 巢狀內部，尚未到最外層，不執行真正的 Commit

            if (_transaction is null)
            {
                return;
            }

            try
            {
                await _transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred during database commit.");
                throw;
            }
            finally
            {
                await ResetTransactionAsync();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                connection.Dispose();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null) return;

            try
            {
                await _transaction.RollbackAsync(cancellationToken);
                Logger.LogWarning("Database transaction rolled back.");
            }
            finally
            {
                _transactionCount = 0; // Rollback 應強制歸零
                await ResetTransactionAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }

                await connection.DisposeAsync();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        #region 私有方法
        private async Task ResetTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        #endregion
    }
}
