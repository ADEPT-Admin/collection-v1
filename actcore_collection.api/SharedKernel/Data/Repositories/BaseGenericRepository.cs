using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Data.Interface;
using SharedKernel.Helpers;
using SharedKernel.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq.Expressions;

namespace SharedKernel.Data.Repositories
{
    public abstract class BaseGenericRepository<T> : IBaseGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseGenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetQueryableAsync
        {
            get
            {
                return _dbSet;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true)
        {
            if (asNoTracking)
                return await _dbSet.AsNoTracking().ToListAsync();
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (asNoTracking)
                query = query.AsNoTracking();
            if (orderBy != null)
            {

                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        /*public virtual async Task<PagedResult<T>> GetAllPagedAsync(
            Expression<Func<T, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            Dictionary<string, object> filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;

            // Search data
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply Filters by Column
            if (filterColumns != null)
            {
                var entityType = typeof(T);
                foreach (var filterColumn in filterColumns)
                {
                    var property = entityType.GetProperty(filterColumn.Key);
                    if (property != null)
                    {
                        var propertyType = property.PropertyType;
                        var value = filterColumn.Value;

                        if (propertyType == typeof(string))
                        {
                            if (value == null)
                            {
                                query = query.Where(u => EF.Property<string>(u, filterColumn.Key) == null);
                            }
                            else if (value is string strVal && string.IsNullOrEmpty(strVal))
                            {
                                query = query.Where(u => string.IsNullOrEmpty(EF.Property<string>(u, filterColumn.Key)));
                            }
                            else
                            {
                                query = query.Where(u =>
                                    EF.Functions.Like(EF.Property<string>(u, filterColumn.Key), $"%{value}%")
                                );
                            }
                        }
                        else if (propertyType == typeof(int) || propertyType == typeof(int?))
                        {
                            if (value == null)
                            {
                                query = query.Where(u => EF.Property<int?>(u, filterColumn.Key) == null);
                            }
                            else if (int.TryParse(Convert.ToString(value), out int intValue))
                            {
                                query = query.Where(u =>
                                    EF.Property<int?>(u, filterColumn.Key) == intValue
                                );
                            }
                        }
                        else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                        {
                            if (value == null)
                            {
                                query = query.Where(u => EF.Property<DateTime?>(u, filterColumn.Key) == null);
                            }
                            else if (DateTime.TryParse(Convert.ToString(value), out DateTime dateValue))
                            {
                                query = query.Where(u =>
                                    EF.Property<DateTime?>(u, filterColumn.Key) == dateValue
                                );
                            }
                        }
                    }
                }
            }

            // Get total count before paging
            var totalCount = await query.CountAsync();

            // Include related entities
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // AsNoTracking
            if (asNoTracking)
                query = query.AsNoTracking();

            List<T> items;
            // Sorting
            if (!string.IsNullOrEmpty(sortColumn))
            {
                if (IsNotMappedProperty<T>(sortColumn))
                {
                    // ⬇️ NotMapped → ดึงข้อมูลก่อน แล้ว sort ใน memory
                    items = await query.ToListAsync();

                    var compiledSort = GetPropertyExpression<T>(sortColumn).Compile();
                    items = sortDirection == "asc"
                        ? items.OrderBy(compiledSort).ToList()
                        : items.OrderByDescending(compiledSort).ToList();
                }
                else
                {
                    // ⬇️ Mapped → ให้ EF จัดการ
                    var sortExpr = GetPropertyExpression<T>(sortColumn);

                    query = sortDirection == "asc"
                        ? query.OrderBy(sortExpr)
                        : query.OrderByDescending(sortExpr);

                    items = await query.ToListAsync();
                }
            }
            else
            {
                items = await query.ToListAsync();
            }

            // Paging
            if (pageSize > 0)
            {
                if (pageSize > 100) pageSize = 100;
                items = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount
            };
        } */

        public virtual async Task<PagedResult<T>> GetAllPagedAsync(
            Expression<Func<T, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterContainer = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;

            // Search data - Base filter
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Dynamic filters (String, Number, Date, Enum)
            if (filterContainer?.DynamicFilters?.Any() == true)
            {
                // ใช้ EfFilterHelper สร้าง Expression<Func<T,bool>>
                var dynamicFilterExpression = FilterHelper.BuildFilterExpression<T>(filterContainer);

                if (dynamicFilterExpression != null)
                {
                    // Apply filter expression ที่สร้างขึ้น
                    query = query.Where(dynamicFilterExpression);
                }

            }

            // Get total count before paging
            var totalCount = await query.CountAsync();

            // Include related entities
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // AsNoTracking
            if (asNoTracking)
                query = query.AsNoTracking();


            // Sorting
            List<T> items;
            if (!string.IsNullOrEmpty(sortColumn))
            {
                if (IsNotMappedProperty<T>(sortColumn))
                {
                    // ⬇️ NotMapped → ดึงข้อมูลก่อน แล้ว sort ใน memory
                    items = await query.ToListAsync();

                    var compiledSort = GetPropertyExpression<T>(sortColumn).Compile();
                    items = sortDirection == "asc"
                        ? items.OrderBy(compiledSort).ToList()
                        : items.OrderByDescending(compiledSort).ToList();
                }
                else
                {
                    // ⬇️ Mapped → ให้ EF จัดการ
                    var sortExpr = GetPropertyExpression<T>(sortColumn);

                    query = sortDirection == "asc"
                        ? query.OrderBy(sortExpr)
                        : query.OrderByDescending(sortExpr);

                    items = await query.ToListAsync();
                }
            }
            else
            {
                items = await query.ToListAsync();
            }

            // Paging
            if (pageSize > 0)
            {
                if (pageSize > 100) pageSize = 100;
                items = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        private static Expression<Func<T, object>> GetPropertyExpression<T>(string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "x");
            Expression body = param;
            Type currentType = typeof(T);

            foreach (var member in propertyName.Split('.'))
            {
                var prop = currentType.GetProperty(member);
                if (prop == null)
                    throw new ArgumentException($"Property {member} not found on {currentType.Name}");

                if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)
                    && prop.PropertyType != typeof(string))
                {
                    // 🚩 กรณี Collection → ใช้ Min(x => x.XXX)
                    var elementType = prop.PropertyType.GetGenericArguments().FirstOrDefault();
                    if (elementType == null)
                        throw new InvalidOperationException($"Cannot determine element type for collection {prop.Name}");

                    body = Expression.PropertyOrField(body, member); // x.UserGroupAccesses

                    // สมมติว่าจะ sort ด้วย property ต่อจากนี้ เช่น UserGroup.UserGroupName
                    var nextMembers = propertyName.Split('.').SkipWhile(m => m != member).Skip(1).ToArray();
                    if (!nextMembers.Any())
                        throw new InvalidOperationException("Collection property must be followed by a member to sort by");

                    // lambda parameter ของ element
                    var innerParam = Expression.Parameter(elementType, "y");
                    Expression innerBody = innerParam;
                    Type innerType = elementType;

                    foreach (var nextMember in nextMembers)
                    {
                        var innerProp = innerType.GetProperty(nextMember);
                        if (innerProp == null)
                            throw new ArgumentException($"Property {nextMember} not found on {innerType.Name}");

                        innerBody = Expression.PropertyOrField(innerBody, nextMember);
                        innerType = innerProp.PropertyType;
                    }

                    var innerLambda = Expression.Lambda(innerBody, innerParam);

                    // สร้าง Min(body.Select(y => y.NextMember))
                    body = Expression.Call(
                        typeof(Enumerable),
                        "Min",
                        new Type[] { elementType, innerType },
                        body,
                        innerLambda
                    );

                    // จบ loop เลย เพราะเรา resolve ทั้ง chain แล้ว
                    currentType = innerType;
                    break;
                }
                else
                {
                    body = Expression.PropertyOrField(body, member);
                    currentType = prop.PropertyType;
                }
            }

            return Expression.Lambda<Func<T, object>>(Expression.Convert(body, typeof(object)), param);
        }

        private static bool IsNotMappedProperty<T>(string propertyPath)
        {
            var type = typeof(T);
            foreach (var member in propertyPath.Split('.'))
            {
                var prop = type.GetProperty(member);
                if (prop == null) return false;

                if (Attribute.IsDefined(prop, typeof(NotMappedAttribute)))
                    return true;

                type = prop.PropertyType;
            }
            return false;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);

        }
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (asNoTracking)
                query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }
        public virtual async Task<Task> BulkUpdateAsync(IEnumerable<T> entities, BulkConfig bulkConfig = null)
        {
            var cfg = bulkConfig 
                ?? new BulkConfig
                {
                    // Fast Mode
                    UseTempDB = false,
                    TrackingEntities = false,
                    SetOutputIdentity = false
                };

            await _context.BulkUpdateAsync(entities, cfg);

            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
                return await _dbSet.CountAsync(filter);
            else
                return await _dbSet.CountAsync();
        }
        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException("SQL command cannot be null or empty.", nameof(sql));

            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public virtual async Task<List<Dictionary<string, object>>> ExecuteSqlQueryAsync(string sql, params SqlParameter[] parameters)
        {
            var result = new List<Dictionary<string, object>>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                if (parameters != null && parameters.Length > 0)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                if (command.Connection.State != ConnectionState.Open)
                    await command.Connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                        }
                        result.Add(row);
                    }
                }
            }

            return result;
        }

        public virtual async Task<List<T>> ExecuteStoredProcedureAsync(string storedProcedureName, params object[] parameters)
        {
            var sql = $"EXEC {storedProcedureName}";

            if (parameters != null && parameters.Length > 0)
            {
                var paramPlaceholders = parameters.Select((_, i) => $"{{{i}}}").ToArray();
                sql += " " + string.Join(", ", paramPlaceholders);
            }

            return await _dbSet.FromSqlRaw(sql, parameters ?? Array.Empty<object>()).ToListAsync();
        }

        public virtual async Task<List<Dictionary<string, object>>> ExecuteDynamicStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            var result = new List<Dictionary<string, object>>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = storedProcedureName;
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null && parameters.Length > 0)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                if (command.Connection.State != ConnectionState.Open)
                    await command.Connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                        }
                        result.Add(row);
                    }
                }
            }

            return result;
        }

    }
}

