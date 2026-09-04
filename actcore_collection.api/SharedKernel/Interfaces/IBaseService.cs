using System.Linq.Expressions;

namespace SharedKernel.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<IEnumerable<T>> GetAllPaginationAsync(Expression<Func<T, bool>> filter = null, int pageSize = 0, int pageNumber = 1);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter = null);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
