using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.DataImport;
using ACTCore.CollectionService.DataImport.Extensions;
using ACTCore.CollectionService.DataImport.Helpers;
using ACTCore.CollectionService.DataImport.Validators;
using ACTCore.CollectionService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.CommonConstants;
using System.Diagnostics;

class Program
{
    static async Task<int> Main(string[] args)
    {
        static void WaitForExitIfDebug()
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("\nPress Enter to close...");
                while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter) { }
            }
        }

        // Inject test args only when debugging and none provided
        if (Debugger.IsAttached && args is { Length: 0 })
        {
            // Example: TableName and DirectoryPath
            args = new[] { "Contract", @"D:\DATA_TEST\COLLECTION_CONTRACT\Contract" };
            //args = new[] { "ContractAddress", @"D:\DATA_TEST\COLLECTION_CONTRACT\ContractAddress" };
            //args = new[] { "ContractAsset", @"D:\DATA_TEST\COLLECTION_CONTRACT\ContractAsset" };
            //args = new[] { "ContractAssetVehicle", @"D:\DATA_TEST\COLLECTION_CONTRACT\ContractAssetVehicle" };
            //args = new[] { "ContractOverdue", @"D:\DATA_TEST\COLLECTION_CONTRACT\ContractOverdue" };
            //args = new[] { "ContractPayment", @"D:\DATA_TEST\COLLECTION_CONTRACT\ContractPayment" };
            //args = new[] { "ContractPerson", @"D:\DATA_TEST\COLLECTION_CONTRACT\ContractPerson" };
            //args = new[] { "ContractPhone", @"D:\DATA_TEST\COLLECTION_CONTRACT\ContractPhone" };
            //args = new[] { "EmployeeProfile", @"D:\DATA_TEST\COLLECTION_EMPLOYEE\EmployeeProfile" };

            Console.WriteLine("Debug Mode: using test args for import.");
        }
        //else if (args is { Length: 0 })
        //{
        //    Console.WriteLine("No arguments provided. Usage: <TableName> <DirectoryPath>");
        //    WaitForExitIfDebug();
        //    return 1;
        //}

        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        builder.Services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAutoMapper(typeof(MappingConfig));
        builder.Services.AddCustomServices();

        using var host = builder.Build();

        /* ------------------------------- START --------------------------------------- */
        var startTime = DateTime.Now;
        var _logService = host.Services.GetRequiredService<IProcessExecutionLogService>();
        long logId = await _logService.StartProcessExecLogAsync(
            jobName: "MappingImport",
            jobType: ExecutionJobType.IMPORT,
            sourceSystem: ExecutionSourceSystem.CSV,
            targetTable: args != null && args.Length == 2 ? args[0] : "None",
            directoryPath: args != null && args.Length == 2 ? args[1] : "None",
            startTime: startTime
        );

        var mappingImportService = host.Services.GetRequiredService<MappingImportService>();
        var validation = await ImportRequestValidator.ValidateAsync(
                            builder.Configuration,
                            mappingImportService,
                            args);

        if (!validation.IsValid)
        {
            Console.WriteLine(validation.ErrorMessage);

            await _logService.EndProcessExecLogAsync(
                id: logId,
                description: validation.ErrorMessage
            );

            WaitForExitIfDebug();
            return 1;
        }

        string tableName = args[0];
        string directoryPath = args[1];

        Console.WriteLine($"Found {validation.CsvFiles.Count} CSV files for table '{tableName}'");
        try
        {
            string systemUser = builder.Configuration.GetSection("SystemUser").Get<string>();
            int patialFailedCount = 0;
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Cancellation requested...");
                cts.Cancel();
                e.Cancel = true;
            };

            foreach (var file in validation.CsvFiles)
            {
                cts.Token.ThrowIfCancellationRequested();
                var detailId = await _logService.StartProcessExecLogDetailAsync(logId, Path.GetFileName(file));

                try
                {
                    Console.WriteLine($"\nProcessing: {file}");

                    var status = ExecutionStatus.SUCCESS;
                    var result = await BatchRunnerHelper.RunAsync(host, tableName, systemUser, file, startTime, cts.Token);

                    if (!string.IsNullOrEmpty(result.ErrorMessage) || result.Failed > 0)
                    {
                        patialFailedCount++;
                        status = ExecutionStatus.FAILED;
                    }

                    await _logService.EndProcessExecLogDetailAsync(
                        detailId: detailId,
                        successCount: result.Success,
                        failedCount: result.Failed,
                        totalRecords: result.Total,
                        status: status,
                        errorMessage: result.ErrorMessage,
                        stackTrace: result.StackTrace,
                        ct: cts.Token
                    );
                }
                catch (Exception ex)
                {
                    patialFailedCount++;
                    Console.WriteLine($"Error processing file {file}: {ex.Message}");
                    await _logService.EndProcessExecLogDetailAsync(
                        detailId: detailId,
                        status: ExecutionStatus.FAILED,
                        errorMessage: ex.Message,
                        stackTrace: ex.ToString()
                    );
                }
            }

            // -------- END EXECUTION LOG ----------
            string endMessage = patialFailedCount > 0 ? "Some files failed during import." : null;
            await _logService.EndProcessExecLogAsync(
                id: logId,
                totalFiles: validation.CsvFiles.Count,
                description: endMessage
            );

            Console.WriteLine($"\n{endMessage ?? "Complete Task"}");

            WaitForExitIfDebug();
            return 0;
        }
        catch (OperationCanceledException)
        {
            string message = "Operation was cancelled by user.";
            Console.WriteLine(message);
            await _logService.EndProcessExecLogAsync(id: logId, totalFiles: validation.CsvFiles.Count, description: message);
            WaitForExitIfDebug();
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
            await _logService.EndProcessExecLogAsync(id: logId, totalFiles: validation.CsvFiles.Count, description: ex.Message);
            WaitForExitIfDebug();
            return 1;
        }
    }
}
