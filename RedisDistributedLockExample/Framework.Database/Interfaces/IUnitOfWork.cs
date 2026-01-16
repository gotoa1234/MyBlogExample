using System.Data;

namespace Framework.Database.Interfaces
{
    /// <summary>
    /// 定義工作單元介面，負責管理資料庫事務與連線生命週期
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Repository 使用
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// 交易物件，允許為 null 代表尚未啟動交易
        /// </summary>
        IDbTransaction? Transaction { get; }

        /// <summary>
        /// 檢查當前是否已有啟動中的交易
        /// </summary>
        bool HasActiveTransaction => Transaction != null;

        /// <summary>
        /// 交易事務
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 提交事務
        /// </summary>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 回滾事務
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
