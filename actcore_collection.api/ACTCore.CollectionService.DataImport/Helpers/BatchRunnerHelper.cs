using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Domain.Entities.Contracts;
using ACTCore.CollectionService.Domain.Entities.Profiles;
using ACTCore.CollectionService.Domain.Entities.Securities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ACTCore.CollectionService.DataImport.Helpers
{
    public class BatchRunnerHelper
    {
        public static async Task<ProcessExecutionReasponseDto> RunAsync(IHost host, string tableName, string systemUser, string csvPath, DateTime batchNoDateTime, CancellationToken ct = default)
        {
            using var scope = host.Services.CreateScope();
            var _importService = scope.ServiceProvider.GetRequiredService<MappingImportService>();

            try
            {
                var modelType = ResolveModel(tableName);

                if(modelType == null)
                {
                    string msg = $"Error: Unable to resolve model for TableName = {tableName}";
                    Console.WriteLine(msg);
                    return new ProcessExecutionReasponseDto() { ErrorMessage = msg };
                }

                var method = typeof(MappingImportService)
                    .GetMethod(nameof(MappingImportService.ImportAsync))!
                    .MakeGenericMethod(modelType);

                var task = (Task<ProcessExecutionReasponseDto>)method
                    .Invoke(_importService, new object[] { csvPath, systemUser, batchNoDateTime, ct })!;

                var result = await task;

                Console.WriteLine("Done");
                return result;
            }
            catch (Exception ex)
            {
                string message = $"Fatal error: {ex}";
                Console.WriteLine(message);
                return new ProcessExecutionReasponseDto() { ErrorMessage = message };
            }
        }

        public static Type ResolveModel(string tableName)
        {
            Dictionary<string, Type> _cache = new(StringComparer.OrdinalIgnoreCase);

            if (_cache.TryGetValue(tableName, out var cached))
                return cached;

            var modelType = typeof(Contract).Assembly // <-- assembly, storing the import models
                .GetTypes()
                .FirstOrDefault(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    string.Equals(t.Name, tableName, StringComparison.OrdinalIgnoreCase));

            if (modelType != null)
                _cache[tableName] = modelType;

            return modelType;
        }
    }
}
