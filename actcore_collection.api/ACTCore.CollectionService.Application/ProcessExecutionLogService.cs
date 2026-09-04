using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel.CommonConstants;
using SharedKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.Application
{
    public class ProcessExecutionLogService : IProcessExecutionLogService
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public ProcessExecutionLogService(DbContextOptions<AppDbContext> options)
        {
            _options = options;
        }

        public async Task<long> StartProcessExecLogAsync(string jobName, string jobType, string sourceSystem, string targetTable, string directoryPath, DateTime startTime, CancellationToken ct = default)
        {
            var processLog = new ProcessExecutionLog
            {
                JobName = jobName,
                JobType = jobType,
                TargetTable = targetTable,
                SourcePath = directoryPath,
                SourceSystem = sourceSystem,
                BatchNo = $"{targetTable}_{startTime:yyyyMMddHHmmss}",
                StartTime = startTime,
                LogDateTime = startTime,
                Status = ExecutionStatus.RUNNING,
                ServerName = Environment.MachineName,
            };

            await using var ctx = new AppDbContext(_options);
            await ctx.ProcessExecutionLogs.AddAsync(processLog);
            await ctx.SaveChangesAsync(ct);
            return processLog.Id;
        }

        public async Task EndProcessExecLogAsync(long id,int totalFiles = 0, string description = null, CancellationToken ct = default)
        {
            await using var ctx = new AppDbContext(_options);
            var processLog = await ctx.ProcessExecutionLogs
                .Include(p => p.Details)
                .FirstOrDefaultAsync(x => x.Id ==  id, ct);

            if (processLog == null) return;

            var details = processLog.Details ?? Array.Empty<ProcessExecutionLogDetail>();

            processLog.EndTime = DateTime.Now;
            processLog.TotalFiles = totalFiles;
            processLog.SuccessCount = details.Sum(d => d.SuccessCount ?? 0);
            processLog.FailedCount = details.Sum(d => d.FailedCount ?? 0);
            processLog.TotalRecords = processLog.SuccessCount + processLog.FailedCount;
            processLog.Description = description;

            if (!details.Any())
            {
                processLog.Status = ExecutionStatus.FAILED;
            }
            else
            {
                var anyFailed = details.Any(d => d.Status == ExecutionStatus.FAILED);
                var anySuccess = details.Any(d => d.Status == ExecutionStatus.SUCCESS);
                if (anyFailed && anySuccess)
                {
                    processLog.Status = ExecutionStatus.PARTIAL;
                }
                else if (anyFailed)
                {
                    processLog.Status = ExecutionStatus.FAILED;
                }
                else
                {
                    processLog.Status = ExecutionStatus.SUCCESS;
                }
            }

            ctx.ProcessExecutionLogs.Update(processLog);
            await ctx.SaveChangesAsync(ct);
        }

        public async Task<long> StartProcessExecLogDetailAsync(long logId, string fileName, CancellationToken ct = default)
        {             
            var logDetail = new ProcessExecutionLogDetail
            {
                LogId = logId,
                FileName = fileName,
                StartTime = DateTime.Now,
                Status = ExecutionStatus.RUNNING,
            };
            await using var ctx = new AppDbContext(_options);
            await ctx.ProcessExecutionLogDetails.AddAsync(logDetail);
            await ctx.SaveChangesAsync();
            return logDetail.DetailId;
        }

        public async Task EndProcessExecLogDetailAsync(long detailId, int? totalRecords = null, int? successCount = null, int? failedCount = null, string status = null, string errorMessage = null, string stackTrace = null, CancellationToken ct = default)
        {
            await using var ctx = new AppDbContext(_options);

            var detail = await ctx.ProcessExecutionLogDetails.FirstOrDefaultAsync(x => x.DetailId == detailId, ct);
            if (detail == null) return;

            detail.TotalRecords = totalRecords.HasValue ? totalRecords : 0;
            detail.SuccessCount = successCount.HasValue ? successCount : 0;
            detail.FailedCount = failedCount.HasValue ? failedCount : 0;

            detail.EndTime = DateTime.Now;
            detail.Status = !string.IsNullOrEmpty(status) ? status : ExecutionStatus.FAILED;
            detail.ErrorMessage = errorMessage;
            detail.ErrorStackTrace = stackTrace;

            ctx.ProcessExecutionLogDetails.Update(detail);
            await ctx.SaveChangesAsync(ct);
        }

        public async Task<long> AddProcessExecLogDetailAsync(ProcessExecutionLogDetail detail, CancellationToken ct = default)
        {
            await using var ctx = new AppDbContext(_options);
            await ctx.ProcessExecutionLogDetails.AddAsync(detail, ct);
            await ctx.SaveChangesAsync(ct);
            return detail.DetailId;
        }
    }
}
