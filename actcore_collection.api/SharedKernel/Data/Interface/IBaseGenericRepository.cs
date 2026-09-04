using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace SharedKernel.Data.Interface
{
    public interface IBaseGenericRepository<T> where T : class
    {
        IQueryable<T> GetQueryableAsync { get; }
        Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool asNoTracking = true);

        Task<PagedResult<T>> GetAllPagedAsync(Expression<Func<T, bool>> filter = null
            , int pageSize = 0, int pageNumber = 1
            , string sortColumn = "", string sortDirection = "asc"
            , FilterContainer filterColumns = null
            , string includeProperties = "", bool asNoTracking = true);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "", bool asNoTracking = true);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task<Task> BulkUpdateAsync(IEnumerable<T> entities, BulkConfig bulkConfig = null);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
        Task<List<Dictionary<string, object>>> ExecuteSqlQueryAsync(string sql, params SqlParameter[] parameters);
        Task<List<T>> ExecuteStoredProcedureAsync(string storedProcedureName, params object[] parameters);
        Task<List<Dictionary<string, object>>> ExecuteDynamicStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters);
    }
}
