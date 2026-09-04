using ACTCore.CollectionService.Application.Model;
using SharedKernel.Models;

namespace ACTCore.CollectionService.Application.Interface
{
    public interface IActivityLogService
    {
        Task ActivityLogAsync(ActivityLog log);
        Task ActivityLogAsync(IEnumerable<ActivityLog> logs);
        Task ActivityLogCreateWithDetailsAsync(ActivityLog log, Dictionary<string, dynamic> changes);
        Task BulkInsertActivityLogCreateWithDetailsAsync(List<ActivityLog> logs);
        Task ActivityLogEditWithDetailsAsync(ActivityLog log, Dictionary<string, (string OldValue, string NewValue)> changes);
        Task<List<ActivityLog>> GetActivityLogsAsync(ActivityLogFilter filter);
    }
}
