using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.Data.Interface;
using System.Collections.Concurrent;

namespace SharedKernel.Data.Repositories
{
    public abstract class BaseUnitOfWork : IBaseUnitOfWork
    {
        private bool _disposed;
        private readonly DbContext _dbContext;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();
        private IDbContextTransaction _currentTransaction;

        public BaseUnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbContext DbContext => _dbContext;

        public IBaseGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                // Fix: Use a concrete implementation of BaseRepository<T>
                var repo = new ConcreteRepository<T>(_dbContext);
                _repositories[type] = repo;
            }
            return (IBaseGenericRepository<T>)_repositories[type];
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public bool HasActiveTransaction => _dbContext.Database.CurrentTransaction != null || _currentTransaction != null;

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            if (HasActiveTransaction)
                return _dbContext.Database.CurrentTransaction ?? _currentTransaction!;

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(ct);
            return _currentTransaction;
        }

        // clearAfterCommit: true = เคลียร์ ChangeTracker หลัง commit (ปกติไม่จำเป็น)
        public async Task CommitTransactionAsync(bool clearAfterCommit = false, CancellationToken ct = default)
        {
            if (!HasActiveTransaction) return;

            try
            {
                await _dbContext.SaveChangesAsync(ct);

                await (_dbContext.Database.CurrentTransaction ?? _currentTransaction)!.CommitAsync(ct);

                if (clearAfterCommit)
                    _dbContext.ChangeTracker.Clear();
            }
            catch
            {
                await RollbackTransactionAsync(clearAfterRollback: true, ct);
                throw;
            }
            finally
            {
                await DisposeCurrentTransactionAsync();
            }
        }

        // clearAfterRollback: true = เคลียร์ ChangeTracker หลัง rollback (แนะนำให้ true)
        public async Task RollbackTransactionAsync(bool clearAfterRollback = true, CancellationToken ct = default)
        {
            if (!HasActiveTransaction) return;

            try
            {
                await (_dbContext.Database.CurrentTransaction ?? _currentTransaction)!.RollbackAsync(ct);
            }
            finally
            {
                if (clearAfterRollback)
                    _dbContext.ChangeTracker.Clear();

                await DisposeCurrentTransactionAsync();
            }
        }

        public void ClearChangeTracker() => _dbContext.ChangeTracker.Clear();

        private async Task DisposeCurrentTransactionAsync()
        {
            try
            {
                if (_dbContext.Database.CurrentTransaction != null)
                    await _dbContext.Database.CurrentTransaction.DisposeAsync();

                if (_currentTransaction != null)
                    await _currentTransaction.DisposeAsync();
            }
            finally
            {
                _currentTransaction = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                    _currentTransaction?.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    // Fix: Add a concrete implementation of BaseRepository<T>
    public class ConcreteRepository<T> : BaseGenericRepository<T> where T : class
    {
        public ConcreteRepository(DbContext context) : base(context)
        {
        }
    }
}