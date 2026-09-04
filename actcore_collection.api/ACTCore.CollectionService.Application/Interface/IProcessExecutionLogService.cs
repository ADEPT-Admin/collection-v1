
using SharedKernel.Models;

namespace ACTCore.CollectionService.Application.Interface
{
    public interface IProcessExecutionLogService
    {
        Task<long> StartProcessExecLogAsync(string jobName, string jobType, string sourceSystem, string targetTable
            , string directoryPath, DateTime startTime, CancellationToken ct = default);

        Task EndProcessExecLogAsync(long id, int totalFiles = 0, string description = null, CancellationToken ct = default);

        Task<long> StartProcessExecLogDetailAsync(long logId, string fileName, CancellationToken ct = default);

        Task EndProcessExecLogDetailAsync(long detailId, int? totalRecords = null, int? successCount = null
            , int? failedCount = null, string status = "", string errorMessage = null, string stackTrace = null
            , CancellationToken ct = default);

        Task<long> AddProcessExecLogDetailAsync(ProcessExecutionLogDetail detail, CancellationToken ct = default);
    }
}
