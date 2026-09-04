using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Model;
using ACTCore.CollectionService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;

namespace ACTCore.CollectionService.Application
{
    public class ApiLogService : IApiLogService
    {
        private AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiLogService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetTraceId()
        {
            return _httpContextAccessor.HttpContext?.Items["TraceId"]?.ToString() ?? Guid.NewGuid().ToString();
        }

        public async Task ApiLogAsync(ApiLog log)
        {
            log.TraceId = Guid.Parse(GetTraceId());
            await _dbContext.ApiLogs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ApiLog>> GetApiLogsAsync(ApiLogFilter filter)
        {
            var query = _dbContext.ApiLogs.AsQueryable();

            //if (!string.IsNullOrEmpty(filter.UserId))
            //    query = query.Where(x => x.UserId == filter.UserId);

            //if (!string.IsNullOrEmpty(filter.Action))
            //    query = query.Where(x => x.Action == filter.Action);

            //if (!string.IsNullOrEmpty(filter.EntityName))
            //    query = query.Where(x => x.EntityName == filter.EntityName);

            //if (!string.IsNullOrEmpty(filter.Status))
            //    query = query.Where(x => x.Status == filter.Status);

            //if (filter.FromDate.HasValue)
            //    query = query.Where(x => x.Timestamp >= filter.FromDate.Value);

            //if (filter.ToDate.HasValue)
            //    query = query.Where(x => x.Timestamp <= filter.ToDate.Value);

            return await query.OrderByDescending(x => x.Timestamp).ToListAsync();
        }
    }
}
