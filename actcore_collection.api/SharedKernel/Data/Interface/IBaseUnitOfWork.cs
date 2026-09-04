using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace SharedKernel.Data.Interface
{
    public interface IBaseUnitOfWork : IDisposable
    {
        IBaseGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
        DbContext DbContext { get; }

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);

        Task CommitTransactionAsync(bool clearAfterCommit = false, CancellationToken ct = default);

        Task RollbackTransactionAsync(bool clearAfterRollback = true, CancellationToken ct = default);

        void ClearChangeTracker();

    }
}
