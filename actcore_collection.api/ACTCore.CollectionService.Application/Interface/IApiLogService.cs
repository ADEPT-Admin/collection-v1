using ACTCore.CollectionService.Application.Model;
using SharedKernel.Models;

namespace ACTCore.CollectionService.Application.Interface
{
    public interface IApiLogService
    {
        Task ApiLogAsync(ApiLog log);
        Task<List<ApiLog>> GetApiLogsAsync(ApiLogFilter filter);
    }
}
