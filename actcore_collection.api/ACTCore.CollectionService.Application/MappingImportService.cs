using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Domain.Attributes;
using ACTCore.CollectionService.Domain.Entities.Imports;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using CsvHelper;
using EFCore.BulkExtensions;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application
{
    public class MappingImportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MappingImportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<MappingImport> GetAsync(Expression<Func<MappingImport, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<MappingImport>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<IEnumerable<MappingImport>> GetListAsync(Expression<Func<MappingImport, bool>> filter = null, bool asNoTracking = true, string includeProperties = "")
        {
            // Include all navigation properties
            return await _unitOfWork.Repository<MappingImport>().GetAllAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking
            );
        }
        public async Task<IEnumerable<MappingImportDetail>> GetListDetailAsync(Expression<Func<MappingImportDetail, bool>> filter = null, bool asNoTracking = true)
        {
            // Include all navigation properties
            return await _unitOfWork.Repository<MappingImportDetail>().GetAllAsync(
                filter,
                includeProperties: "",
                asNoTracking: asNoTracking
            );
        }

        public async Task<ProcessExecutionReasponseDto> ImportAsync<T>(string csvPath, string systemUser, DateTime batchNoDateTime, CancellationToken token) where T : VersionBaseModel, new()
        {
            try
            {
                var tableName = typeof(T).Name;
                List<T> items;

                var mapping = await GetAsync(m => m.TableName == tableName && m.AllowImport == true);

                if (mapping == null)
                {
                    string msg = $"Error: No allwance or MappingImport records were found for TableName = {tableName}";
                    Console.WriteLine(msg);
                    return new ProcessExecutionReasponseDto() { ErrorMessage = msg };
                }

                var mappingDetails = await GetListDetailAsync(d => d.MappingId == mapping.Id && d.IsActive == true);

                using (var reader = new StreamReader(csvPath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // Dynamic ClassMap จาก DB
                    csv.Context.RegisterClassMap(new DynamicMappingHelper<T>(mappingDetails));

                    items = csv.GetRecords<T>().ToList();
                }

                await BulkMergeAsync(items, systemUser, token);

                token.ThrowIfCancellationRequested();

                var archivedPath = MoveToArchive(csvPath, batchNoDateTime);

                Console.WriteLine($"Imported {items.Count} records into {tableName}.");
                Console.WriteLine($"Archived file to: {archivedPath}");
                
                return new ProcessExecutionReasponseDto() { Total = items.Count, Success = items.Count };
            }
            catch (Exception ex)
            {
                string msg = $"Error importing CSV for {typeof(T).Name}: {ex.Message}";
                Console.WriteLine(msg);
                return new ProcessExecutionReasponseDto() { ErrorMessage = msg, StackTrace = ex.ToString() };
            }
        }

        private async Task BulkMergeAsync<T>(IEnumerable<T> entities, string systemUser, CancellationToken token) where T : VersionBaseModel
        {
            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync(token);

            try
            {
                var bulkConfig = new BulkConfig
                {
                    PreserveInsertOrder = true,
                    UseTempDB = true,
                    SetOutputIdentity = true,
                    TrackingEntities = false,
                    PropertiesToExcludeOnUpdate = new List<string> { nameof(VersionBaseModel.CreatedBy), nameof(VersionBaseModel.CreatedDate) },
                    UpdateByProperties = GetUpdateByProperties<T>()

                };

                foreach (var e in entities)
                {
                    e.CreatedBy ??= systemUser;
                    e.UpdatedBy = systemUser;
                }

                await _unitOfWork.DbContext.BulkInsertOrUpdateAsync(entities, bulkConfig, cancellationToken: token);

                await tran.CommitAsync(token);
            }
            catch
            {
                await tran.RollbackAsync(token);
                throw;
            }
            finally
            {
                _unitOfWork.DbContext.ChangeTracker.Clear();
            }
        }

        /// <summary>
        /// ดึง Business Key จาก [UpdateKey] Attribute หรือ fallback เป็น Primary Key
        /// </summary>
        private List<string> GetUpdateByProperties<T>() where T : VersionBaseModel
        {
            var entityType = typeof(T);
            var properties = entityType.GetProperties();

            // หา Properties ที่มี [UpdateKey] Attribute
            var updateKeys = properties
                .Where(p => p.GetCustomAttributes(typeof(UpdateKeyAttribute), false).Any())
                .OrderBy(p => ((UpdateKeyAttribute)p.GetCustomAttributes(typeof(UpdateKeyAttribute), false).First()).Order)
                .Select(p => p.Name)
                .ToList();

            // ถ้าเจอ UpdateKey ให้ใช้เลย
            if (updateKeys.Any())
                return updateKeys;

            // Fallback: ใช้ Primary Key
            var primaryKey = properties
                .FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any());

            return primaryKey != null
                ? new List<string> { primaryKey.Name }
                : throw new InvalidOperationException($"No key found for entity {entityType.Name}");
        }

        private string MoveToArchive(string sourceFilePath, DateTime batchNoTime)
        {
            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("Source file not found.", sourceFilePath);

            var sourceDir = Path.GetDirectoryName(sourceFilePath)!;
            var fileName = Path.GetFileName(sourceFilePath);

            var archiveRoot = Path.Combine(sourceDir, "Archive");
            var dateFolder = batchNoTime.ToString("yyyyMMdd");
            var archiveDir = Path.Combine(archiveRoot, dateFolder);

            Directory.CreateDirectory(archiveDir);

            var destinationPath = Path.Combine(archiveDir, fileName);

            // Change file name to include batch date-time stamp
            var name = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);

            destinationPath = Path.Combine(
                archiveDir,
                $"{name}_{batchNoTime:yyyyMMddHHmmss}{ext}"
            );
            

            File.Move(sourceFilePath, destinationPath);

            return destinationPath;
        }

    }
}
