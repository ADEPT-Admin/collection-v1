using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.DataImport.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.DataImport.Validators
{
    public static class ImportRequestValidator
    {
        public static async Task<ImportValidationResult> ValidateAsync(
        IConfiguration configuration,
        MappingImportService mappingImportService,
        string[] args)
        {
            // 1. args
            if (args == null || args.Length < 2)
            {
                return new ImportValidationResult
                {
                    ErrorMessage = "Usage: ApplicationName <TableName> <DirectoryPath>"
                };
            }

            var tableName = args[0];
            var directoryPath = args[1];

            // 2. mapping exists and whitelist
            var mappings = await mappingImportService
                .GetListAsync(m => m.TableName == tableName && m.AllowImport == true);

            if (!mappings.Any())
            {
                return new ImportValidationResult
                {
                    ErrorMessage = $"No Allowance and MappingImport records were found for TableName = {tableName}"
                };
            }

            // 3. directory
            if (!Directory.Exists(directoryPath))
            {
                return new ImportValidationResult
                {
                    ErrorMessage = $"Directory not found: {directoryPath}"
                };
            }

            // 4. csv files by prefix
            var filePrefix = tableName + "_";

            var csvFiles = Directory
                .EnumerateFiles(directoryPath, "*.csv", SearchOption.TopDirectoryOnly)
                .Where(f =>
                    Path.GetFileName(f)
                        .StartsWith(filePrefix, StringComparison.OrdinalIgnoreCase))
                .OrderBy(f => f)
                .ToList();

            if (!csvFiles.Any())
            {
                return new ImportValidationResult
                {
                    ErrorMessage =
                        $"No CSV files found with prefix '{filePrefix}' in directory: {directoryPath}"
                };
            }

            return new ImportValidationResult
            {
                CsvFiles = csvFiles
            };
        }
    }
}
