using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;

namespace ACTCore.CollectionService.Application.Security
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorLogService(DbContextOptions<AppDbContext> options,
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


        public async Task ErrrorLogAsync(ErrorLog log)
        {
            log.TraceId = Guid.Parse(GetTraceId());
            if (log.Timestamp == default)
                log.Timestamp = DateTime.UtcNow;

            await using var ctx = new AppDbContext(_options);
            await ctx.ErrorLogs.AddAsync(log);
            await ctx.SaveChangesAsync();
        }
    }
}
