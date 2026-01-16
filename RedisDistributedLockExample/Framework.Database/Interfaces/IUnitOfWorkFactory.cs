using Framework.Database.Enum;

namespace Framework.Database.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        Task<IUnitOfWork> CreateAsync(MysqlDbConnectionEnum dbSource, CancellationToken ct = default);
    }
}
