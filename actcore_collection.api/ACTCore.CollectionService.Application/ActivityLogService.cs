using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Model;
using ACTCore.CollectionService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel.CommonConstants;
using SharedKernel.Models;


namespace ACTCore.CollectionService.Application
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActivityLogService(DbContextOptions<AppDbContext> options,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetTraceId()
        {
            var context = _httpContextAccessor.HttpContext;

            // background job / unit test
            if (context == null)
                return Guid.NewGuid().ToString();

            if (context.Items.TryGetValue("TraceId", out var traceIdObj))
                return traceIdObj.ToString();

            var traceId = Guid.NewGuid().ToString();
            context.Items["TraceId"] = traceId;

            return traceId;
        }

        public async Task ActivityLogAsync(ActivityLog log)
        {
            log.TraceId = Guid.Parse(GetTraceId());
            log.IPAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            log.UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

            await using var ctx = new AppDbContext(_options);
            await ctx.ActivityLogs.AddAsync(log);
            await ctx.SaveChangesAsync();
        }

        public async Task ActivityLogAsync(IEnumerable<ActivityLog> logs)
        {
            if (logs == null || !logs.Any()) return;

            foreach (var log in logs)
            {
                log.TraceId = Guid.Parse(GetTraceId());
                log.IPAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
                log.UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
            }

            await using var ctx = new AppDbContext(_options);
            await ctx.ActivityLogs.AddRangeAsync(logs);
            await ctx.SaveChangesAsync();
        }

        public async Task ActivityLogCreateWithDetailsAsync(ActivityLog log, Dictionary<string, dynamic> changes)
        {
            if (log.Status == ActivityLogStatus.WARNING)
                return;

            if (log.Status != ActivityLogStatus.FAILED)
            {
                foreach (var change in changes)
                {
                    log.Details.Add(new ActivityLogDetail
                    {
                        FieldName = change.Key,
                        NewValue = change.Value
                    });
                }
            }
            log.TraceId = Guid.Parse(GetTraceId());
            log.IPAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            log.UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

            await using var ctx = new AppDbContext(_options);
            await ctx.ActivityLogs.AddAsync(log);
            await ctx.SaveChangesAsync();
        }
        public async Task BulkInsertActivityLogCreateWithDetailsAsync(List<ActivityLog> logs)
        {
            if (logs == null || logs.Count == 0)
                return;

            var traceId = Guid.Parse(GetTraceId());
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

            foreach (var log in logs)
            {
                if (log.Status == ActivityLogStatus.WARNING)
                    continue;

                log.TraceId = traceId;
                log.IPAddress = ipAddress;
                log.UserAgent = userAgent;

                if (log.Details != null && log.Details.Any())
                {
                    foreach (var detail in log.Details)
                    {
                        detail.ActivityLog = log; // ensure relationship
                    }
                }
            }

            await using var ctx = new AppDbContext(_options);
            await ctx.ActivityLogs.AddRangeAsync(logs);
            await ctx.SaveChangesAsync();
        }

        public async Task ActivityLogEditWithDetailsAsync(ActivityLog log, Dictionary<string, (string OldValue, string NewValue)> changes)
        {
            if (log.Status != ActivityLogStatus.FAILED)
            {
                foreach (var change in changes)
                {
                    if (change.Value.OldValue != change.Value.NewValue)
                    {
                        log.Details.Add(new ActivityLogDetail
                        {
                            FieldName = change.Key,
                            OldValue = change.Value.OldValue,
                            NewValue = change.Value.NewValue
                        });
                    }
                }
            }
            log.TraceId = Guid.Parse(GetTraceId());
            log.IPAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            log.UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

            await using var ctx = new AppDbContext(_options);
            await ctx.ActivityLogs.AddAsync(log);
            await ctx.SaveChangesAsync();
        }

        public async Task<List<ActivityLog>> GetActivityLogsAsync(ActivityLogFilter filter)
        {
            await using var ctx = new AppDbContext(_options);
            var query = ctx.ActivityLogs.AsQueryable();

            if (filter.UserId != null)
                query = query.Where(x => x.UserId == filter.UserId);

            if (!string.IsNullOrEmpty(filter.UserName))
                query = query.Where(x => x.UserName == filter.UserName);

            if (!string.IsNullOrEmpty(filter.Action))
                query = query.Where(x => x.Action == filter.Action);

            if (!string.IsNullOrEmpty(filter.EntityName))
                query = query.Where(x => x.EntityName == filter.EntityName);

            if (filter.FromDate.HasValue)
                query = query.Where(x => x.Timestamp >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(x => x.Timestamp <= filter.ToDate.Value);

            return await query.OrderByDescending(x => x.Timestamp).ToListAsync();
        }
    }
}
