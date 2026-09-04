using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using CsvHelper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedKernel.CommonConstants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO.Enumeration;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Runtime.Intrinsics.X86.Avx10v1;

namespace ACTCore.CollectionService.Application
{
    //public interface IAssignmentService
    //{
    //    Task RunAssignmentJobAsync();
    //}
    public class AssignmentService  //:IAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private static bool _isWriteLog { get; set; }
        private static string LogFilePath { get; set; }
        private static int TotalCSVData { get; set; }
        private static int TotalInsertData { get; set; }
        private static int TotalUpdateData { get; set; }
        private static string isError { get; set; }

        public class DataItem
        {
            public int Index { get; set; }
            public int Value { get; set; }
        }

        public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task RunAllocateJobAsync(string projectName, string ADEPTURL, string csvInputFileName, string csvInputFolder, string csvOutputFileName, string csvOutputFolder, string logFile)
        {
            _isWriteLog = true;
            LogFilePath = logFile;

            /*try
            {
                //Read and Import CSV data to Database ---
                string importCSVResult = await ImportCSVToDB_BulkCopy(csvInputFolder, csvInputFileName, "");
                LogMessage(LogFilePath, importCSVResult);

                if (importCSVResult == "")
                {
                    string assignInAllowcateWorklistResult = await AssignCollectorInAssignmentWorklist_BulkCopy();

                    if (assignInAllowcateWorklistResult == "")
                    {
                        await UpdateWorkList_BulkCopy();
                    }
                    else
                    {
                        LogMessage(LogFilePath, importCSVResult);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(LogFilePath, ex.Message);
            }*/

        }

        public async Task RunAssignmentJobAsync_bak20251222(string csvImportFolder, string csvImportFileName, string logFile, string archiveFolder)
        {
            _isWriteLog = true;
            LogFilePath = logFile;            
            TotalInsertData = 0;
            TotalUpdateData = 0;
            isError = "N";       
            
            /*try
            {
                //Read and Import CSV data to Database ---
                string importCSVResult = await ImportCSVToDB_BulkCopy(csvImportFolder, csvImportFileName, archiveFolder);
                
                if (importCSVResult == "")
                {
                    //validate data before do assignment process
                    if (await ValidateData())
                    {
                        string assignInAllowcateWorklistResult = await AssignCollectorInAssignmentWorklist_BulkCopy();

                        if (assignInAllowcateWorklistResult == "")
                        {
                            string updateWorklistResult = await UpdateWorkList_BulkCopy();
                            if (updateWorklistResult != "")
                            {
                                isError = "Y";
                            }
                        }
                        else
                        {
                            LogMessage(LogFilePath, assignInAllowcateWorklistResult);
                            isError = "Y";
                        }
                    }
                    else
                    {
                        isError = "Y";
                    }

                    //Write summary result---
                    await WriteSummaryAssignmentResultLog();
                    
                }
                else
                {
                    LogMessage(LogFilePath, importCSVResult);
                    isError = "Y";
                }
            }
            catch (Exception ex)
            {
                LogMessage(LogFilePath, ex.Message);
                isError = "Y";
            }

            if(isError == "N")
            {
                LogMessage(LogFilePath, "Process complete (Success). Check the log file for details.");
            }
            else
            {
                LogMessage(LogFilePath, "Process complete (Failed). Check the log file for details.");
            }*/
        }

        public async Task RunAssignmentJobAsync(string csvImportFolder, string csvImportFileName, string logFile, string archiveFolder)
        {
            _isWriteLog = true;
            LogFilePath = logFile;            
            TotalInsertData = 0;
            TotalUpdateData = 0;            

            try
            {
                LogMessage(LogFilePath, $"Program Version V11_20260225");
                string[] csvFiles = Directory.GetFiles(csvImportFolder, csvImportFileName + "*.csv");
                if (csvFiles.Length == 0)
                {
                    LogMessage(LogFilePath, $"No CSV files found in the specified folder: {csvImportFolder}.");
                    LogMessage(LogFilePath, "Process complete (Failed). ");
                }
                else
                {                    
                    LogMessage(LogFilePath, $"Found {csvFiles.Length} CSV file(s) to process.");
                    foreach (var filePath in csvFiles)
                    {
                        isError = "N";
                        //Read and Import CSV data to Database ---
                        string importCSVResult = await ImportCSVToDB_BulkCopy(filePath);

                        if (importCSVResult == "")
                        {
                            //validate data before do assignment process
                            if (await ValidateData())
                            {
                                string assignInAllowcateWorklistResult = await AssignCollectorInAssignmentWorklist_BulkCopy();

                                if (assignInAllowcateWorklistResult == "")
                                {
                                    string updateWorklistResult = await UpdateWorkList_BulkCopy();
                                    if (updateWorklistResult != "")
                                    {
                                        isError = "Y";
                                    }
                                }
                                else
                                {
                                    LogMessage(LogFilePath, assignInAllowcateWorklistResult);
                                    isError = "Y";
                                }
                            }
                            else
                            {
                                isError = "Y";
                            }

                            //Write summary result---
                            await WriteSummaryAssignmentResultLog();

                            if (isError != "Y")
                            {
                                //move allocate input file to archive folder (only have no error)---
                                MoveFileToArchive(archiveFolder, csvImportFolder, Path.GetFileName(filePath));
                            }
                        }
                        else
                        {
                            LogMessage(LogFilePath, importCSVResult);
                            isError = "Y";
                        }

                        if (isError == "N")
                        {
                            LogMessage(LogFilePath, "Process complete (Success). ");
                        }
                        else
                        {
                            LogMessage(LogFilePath, "Process complete (Failed). ");
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                LogMessage(LogFilePath, ex.Message);
                LogMessage(LogFilePath, "Process complete (Failed). ");
            }            
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<AssignmentWorklist>().GetByIdAsync(id);
            if (entity == null)
                return false;

            await _unitOfWork.Repository<AssignmentWorklist>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /*public async Task<int> ExecuteSqlAsync(string sql, params object[] parameters)
        {
            return await _unitOfWork.ExecuteSqlRawAsync(sql, parameters);
        }*/

        /*public async Task<bool> DeleteUserWithTransactionAsync(Guid userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // DELETE
                await _unitOfWork.ExecuteSqlRawAsync("DELETE FROM SysUser WHERE Id = {0}", userId);

                // อาจมีคำสั่งอื่นต่อ...

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }*/

        public static void LogMessage(string logFile, string message)
        {
            if (string.IsNullOrEmpty(logFile)) return;

            try
            {
                if (_isWriteLog)
                {
                    // Get the directory of the log file
                    string? logDirectory = Path.GetDirectoryName(logFile);

                    // Check if the directory exists and create it if not
                    if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    string logEntry = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)} - {message}";
                    File.AppendAllText(logFile, logEntry + Environment.NewLine);
                    Console.WriteLine(logEntry); // Also print to console for real-time feedback
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }

        private async Task<string> ImportCSVToDB(string folderPath, string csvFile)
        {
            string resultMessage = "";

            // 1. Get all CSV files than name match with output file name config from the configured folder---            
            string[] csvFiles = Directory.GetFiles(folderPath, csvFile + "*.csv");

            // 2. Check if any files were found
            if (csvFiles.Length == 0)
            {
                LogMessage(LogFilePath, $"No CSV files found in the specified folder: {folderPath}.");
                Console.WriteLine("No CSV files found. Check the log file for details.");
                return "No CSV files found. Check the log file for details.";
            }

            LogMessage(LogFilePath, $"Found {csvFiles.Length} CSV file(s) to process.");
            Console.WriteLine($"Found {csvFiles.Length} CSV file(s) to process.");

            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            var conn = _unitOfWork.DbContext.Database.GetDbConnection();
            await _unitOfWork.DbContext.Database.OpenConnectionAsync();

            // delete data in allocateWorklist---
            await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync("DELETE FROM AssignmentWorklist");

            // 3. Process each file
            foreach (var filePath in csvFiles)
            {
                //await ProcessCsvFile(filePath);
                LogMessage(LogFilePath, $"Processing file: {Path.GetFileName(filePath)}");

                try
                {
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        //Read header row to get column names
                        await csv.ReadAsync();
                        csv.ReadHeader();

                        int rowsInserted = 0;
                        string sql = "";
                        object var00;//ADEPT__ProcessingDate
                        object var01;//ADEPT__ProcessingTime
                        object var02;//ADEPT__ProcessingVersion
                        object var03;//ContractNo
                        object var04;//AllocateMode
                        object var05;//AssignType
                        object var06;//DoNotCallFlag
                        object var07;//AssignFlag
                        object var08;//AssignMethod
                        object var09;//AssignMethodSortField
                        object var10;//AssignMethodSortOrder
                        object var11;//AssignTool
                        object var12;//AssignTeam
                        object var13;//AssignAreaCode
                        object var14;//AssignOverCapacity
                        object var15;//JobType
                        object var16;//RiskLevel
                        object var17;//AssignCollectorId
                        object var18;//ProvinceCode 
                        object var19;//DistrictCode 
                        object var20;//SubDistrictCode 
                        DateTime now = DateTime.Now;

                        // ตรวจสอบ column ที่มีอยู่ในไฟล์ก่อน ถ้าไม่มีจะ return null
                        var hasColumn = csv.HeaderRecord.ToHashSet(StringComparer.OrdinalIgnoreCase); // ใช้ case-insensitive
                        string GetSafeField(string columnName)
                        {
                            return hasColumn.Contains(columnName) && !string.IsNullOrWhiteSpace(csv.GetField(columnName))
                                   ? csv.GetField(columnName)
                                   : null;
                        }


                        while (await csv.ReadAsync())
                        {
                            rowsInserted++;

                            var00 = GetSafeField("ADEPT__ProcessingDate");
                            var01 = GetSafeField("ADEPT__ProcessingTime");
                            var02 = GetSafeField("ADEPT__ProcessingVersion");
                            var03 = GetSafeField("ContractNo");
                            var04 = GetSafeField("AllocateMode");
                            var05 = GetSafeField("AssignType");
                            var06 = GetSafeField("DoNotCallFlag");
                            var07 = GetSafeField("AssignFlag");
                            var08 = GetSafeField("AssignMethod");
                            var09 = GetSafeField("AssignMethodSortField");
                            var10 = GetSafeField("AssignMethodSortOrder");
                            var11 = GetSafeField("AssignTool");
                            var12 = GetSafeField("AssignTeam");
                            var13 = GetSafeField("AssignAreaCode");
                            var14 = GetSafeField("AssignOverCapacity");
                            var15 = GetSafeField("JobType");
                            var16 = GetSafeField("RiskLevel");
                            var17 = GetSafeField("AssignCollectorId");
                            var18 = GetSafeField("ProvinceCode");
                            var19 = GetSafeField("DistrictCode");
                            var20 = GetSafeField("SubDistrictCode");

                            sql = @"INSERT INTO AssignmentWorklist (
                                AllocateWorklistId, ADEPTProcessingDate, ADEPTProcessingTime, ADEPTProcessingVersion,
                                ContractNo, AllocateMode, AssignType, DoNotCallFlag, AssignFlag, AssignMethod,
                                AssignMethodSortField, AssignMethodSortOrder, AssignTool, AssignTeam, AssignAreaCode,
                                AssignOverCapacity, JobType, RiskLevel, AssignCollectorId,ProvinceCode,DistrictCode,SubDistrictCode,
                                CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
                            ) VALUES (
                                @AllocateWorklistId, @ADEPTProcessingDate, @ADEPTProcessingTime, @ADEPTProcessingVersion,
                                @ContractNo, @AllocateMode, @AssignType, @DoNotCallFlag, @AssignFlag, @AssignMethod,
                                @AssignMethodSortField, @AssignMethodSortOrder, @AssignTool, @AssignTeam, @AssignAreaCode,
                                @AssignOverCapacity, @JobType, @RiskLevel, @AssignCollectorId,@ProvinceCode,@DistrictCode,@SubDistrictCode,
                                @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate
                            )";

                            using var cmd = _unitOfWork.DbContext.Database.GetDbConnection().CreateCommand();
                            cmd.CommandText = sql;
                            cmd.Transaction = _unitOfWork.DbContext.Database.CurrentTransaction?.GetDbTransaction();

                            cmd.Parameters.Add(new SqlParameter("@AllocateWorklistId", Guid.NewGuid()));
                            cmd.Parameters.Add(new SqlParameter("@ADEPTProcessingDate", (object?)var00 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@ADEPTProcessingTime", (object?)var01 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@ADEPTProcessingVersion", (object?)var02 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@ContractNo", (object?)var03 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AllocateMode", (object?)var04 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignType", (object?)var05 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@DoNotCallFlag", (object?)var06 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignFlag", (object?)var07 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignMethod", (object?)var08 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignMethodSortField", (object?)var09 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignMethodSortOrder", (object?)var10 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignTool", (object?)var11 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignTeam", (object?)var12 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignAreaCode", (object?)var13 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignOverCapacity", (object?)var14 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@JobType", (object?)var15 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@RiskLevel", (object?)var16 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@AssignCollectorId", (object?)var17 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@ProvinceCode", (object?)var18 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@DistrictCode", (object?)var19 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@SubDistrictCode", (object?)var20 ?? DBNull.Value));
                            cmd.Parameters.Add(new SqlParameter("@CreatedBy", "")); // หรือชื่อ SYSTEM
                            cmd.Parameters.Add(new SqlParameter("@CreatedDate", now));
                            cmd.Parameters.Add(new SqlParameter("@UpdatedBy", "")); // หรือชื่อ SYSTEM
                            cmd.Parameters.Add(new SqlParameter("@UpdatedDate", now));

                            await cmd.ExecuteNonQueryAsync();

                            //LogMessage(LogFilePath, $"Successfully inserted {rowsInserted + "." + sql} rows from {Path.GetFileName(filePath)}.");
                            LogMessage(LogFilePath, "row : " + rowsInserted);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage(LogFilePath, $"Error processing file {Path.GetFileName(filePath)}: {ex.Message}");
                    await tran.RollbackAsync();
                    return ex.Message;
                }
            }
            await _unitOfWork.SaveChangesAsync();
            await tran.CommitAsync();

            return resultMessage;
        }

        private async Task<string> ImportCSVToDB_Json(string folderPath, string csvFile)
        {
            string resultMessage = "";

            // 1. Get all CSV files than name match with output file name config from the configured folder---            
            string[] csvFiles = Directory.GetFiles(folderPath, csvFile + "*.csv");

            // 2. Check if any files were found
            if (csvFiles.Length == 0)
            {
                LogMessage(LogFilePath, $"No CSV files found in the specified folder: {folderPath}.");
                Console.WriteLine("No CSV files found. Check the log file for details.");
                return "No CSV files found. Check the log file for details.";
            }

            LogMessage(LogFilePath, $"Found {csvFiles.Length} CSV file(s) to process.");
            Console.WriteLine($"Found {csvFiles.Length} CSV file(s) to process.");

            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            var conn = _unitOfWork.DbContext.Database.GetDbConnection();
            await _unitOfWork.DbContext.Database.OpenConnectionAsync();

            // delete data in AssignmentWorklist---
            await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync("DELETE FROM AssignmentWorklist2");

            // 3. Process each file
            foreach (var filePath in csvFiles)
            {
                //await ProcessCsvFile(filePath);
                LogMessage(LogFilePath, $"Processing file: {Path.GetFileName(filePath)}");

                try
                {
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        //Read header row to get column names
                        await csv.ReadAsync();
                        csv.ReadHeader();

                        int rowsInserted = 0;
                        DateTime now = DateTime.Now;

                        object oContractNo;//ContractNo                       
                        object oAssignFlag;//AssignFlag
                        object oAssignMethod;//AssignMethod
                        object oAssignMethodSortField;//AssignMethodSortField
                        object oAssignMethodSortOrder;//AssignMethodSortOrder
                        object oAssignTool;//AssignTool
                        object oAssignTeam;//AssignTeam
                        object oAssignAreaCode;//AssignAreaCode
                        object oAssignOverCapacity;//AssignOverCapacity                       
                        object oAssignCollectorId;//AssignCollectorId                        

                        while (await csv.ReadAsync())
                        {
                            rowsInserted++;


                            oContractNo = string.IsNullOrWhiteSpace(csv.GetField("ContractNo")) ? null : csv.GetField("ContractNo");
                            oAssignFlag = string.IsNullOrWhiteSpace(csv.GetField("AssignFlag")) ? null : csv.GetField("AssignFlag");
                            oAssignMethod = string.IsNullOrWhiteSpace(csv.GetField("AssignMethod")) ? null : csv.GetField("AssignMethod");
                            oAssignMethodSortField = string.IsNullOrWhiteSpace(csv.GetField("AssignMethodSortField")) ? null : csv.GetField("AssignMethodSortField");
                            oAssignMethodSortOrder = string.IsNullOrWhiteSpace(csv.GetField("AssignMethodSortOrder")) ? null : csv.GetField("AssignMethodSortOrder");
                            oAssignTool = string.IsNullOrWhiteSpace(csv.GetField("AssignTool")) ? null : csv.GetField("AssignTool");
                            oAssignTeam = string.IsNullOrWhiteSpace(csv.GetField("AssignTeam")) ? null : csv.GetField("AssignTeam");
                            oAssignAreaCode = string.IsNullOrWhiteSpace(csv.GetField("AssignAreaCode")) ? null : csv.GetField("AssignAreaCode");
                            oAssignOverCapacity = string.IsNullOrWhiteSpace(csv.GetField("AssignOverCapacity")) ? null : csv.GetField("AssignOverCapacity");
                            oAssignCollectorId = string.IsNullOrWhiteSpace(csv.GetField("AssignCollectorId")) ? null : csv.GetField("AssignCollectorId");

                            // 1. สร้าง Dictionary เก็บข้อมูลแต่ละคอลัมน์ของแถวนั้น
                            var rowDict = new Dictionary<string, object>();
                            foreach (var header in csv.HeaderRecord)
                            {
                                var rawValue = csv.GetField(header);
                                rowDict[header] = string.IsNullOrWhiteSpace(rawValue) ? null : rawValue;
                            }

                            // 2. แปลง Dictionary เป็น JSON string
                            string jsonData = JsonConvert.SerializeObject(rowDict, Newtonsoft.Json.Formatting.None);

                            // 3. เตรียม SQL และ execute (ใช้ parameter เพื่อป้องกัน injection)
                            string sql = @"INSERT INTO AssignmentWorklist (AssignmentWorklistId, OutputJsonData, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
                                        VALUES (@Id, @JsonData, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate)";

                            using var cmd = _unitOfWork.DbContext.Database.GetDbConnection().CreateCommand();
                            cmd.CommandText = sql;
                            cmd.Transaction = _unitOfWork.DbContext.Database.CurrentTransaction?.GetDbTransaction();

                            var idParam = cmd.CreateParameter();
                            idParam.ParameterName = "@Id";
                            idParam.Value = Guid.NewGuid();
                            cmd.Parameters.Add(idParam);

                            var jsonParam = cmd.CreateParameter();
                            jsonParam.ParameterName = "@JsonData";
                            jsonParam.Value = jsonData ?? (object)DBNull.Value;
                            cmd.Parameters.Add(jsonParam);

                            var createdByParam = cmd.CreateParameter();
                            createdByParam.ParameterName = "@CreatedBy";
                            createdByParam.Value = ""; // หรือใส่ชื่อผู้ใช้งาน
                            cmd.Parameters.Add(createdByParam);

                            var createdDateParam = cmd.CreateParameter();
                            createdDateParam.ParameterName = "@CreatedDate";
                            createdDateParam.Value = now;
                            cmd.Parameters.Add(createdDateParam);

                            var updatedByParam = cmd.CreateParameter();
                            updatedByParam.ParameterName = "@UpdatedBy";
                            updatedByParam.Value = "";
                            cmd.Parameters.Add(updatedByParam);

                            var updatedDateParam = cmd.CreateParameter();
                            updatedDateParam.ParameterName = "@UpdatedDate";
                            updatedDateParam.Value = now;
                            cmd.Parameters.Add(updatedDateParam);

                            await cmd.ExecuteNonQueryAsync();
                        }

                        await _unitOfWork.SaveChangesAsync();
                        await tran.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    LogMessage(LogFilePath, $"Error processing file {Path.GetFileName(filePath)}: {ex.Message}");
                    await tran.RollbackAsync();
                    return ex.Message;
                }
            }

            return resultMessage;
        }

        private async Task<string> ImportCSVToDB_BulkCopy_bak20251222(string folderPath, string csvFile, string archiveFolderPath)
        {
            LogMessage(LogFilePath, $"-----Import CSV data to DB-----");
            string resultMessage = "";            

            string[] csvFiles = Directory.GetFiles(folderPath, csvFile + "*.csv");
            if (csvFiles.Length == 0)
            {
                LogMessage(LogFilePath, $"No CSV files found in the specified folder: {folderPath}.");
                //Console.WriteLine("No CSV files found. Check the log file for details.");
                return "No CSV files found. Check the log file for details.";
            }

            LogMessage(LogFilePath, $"Found {csvFiles.Length} CSV file(s) to process.");
            //Console.WriteLine($"Found {csvFiles.Length} CSV file(s) to process.");

            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            var conn = (SqlConnection)_unitOfWork.DbContext.Database.GetDbConnection();
            await _unitOfWork.DbContext.Database.OpenConnectionAsync();

            // Clear target table
            await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync("DELETE FROM AssignmentWorklist");

            foreach (var filePath in csvFiles)
            {
                LogMessage(LogFilePath, $"Processing file: {Path.GetFileName(filePath)}");

                try
                {
                    using var reader = new StreamReader(filePath);
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        await csv.ReadAsync();
                        csv.ReadHeader();

                        var header = csv.HeaderRecord.ToHashSet(StringComparer.OrdinalIgnoreCase);

                        // Step 1: Prepare DataTable with matching schema
                        DataTable table = new DataTable();
                        table.Columns.Add("AssignmentWorklistId", typeof(Guid)); //1
                        table.Columns.Add("ADEPTProcessingDate", typeof(string)); //2
                        table.Columns.Add("ADEPTProcessingTime", typeof(string)); //3
                        table.Columns.Add("ADEPTProcessingVersion", typeof(string)); //4
                        table.Columns.Add("ContractNo", typeof(string)); //5
                        table.Columns.Add("AllocateMode", typeof(string)); //6
                        table.Columns.Add("AssignType", typeof(string)); //7
                        table.Columns.Add("DoNotCallFlag", typeof(string)); //8
                        table.Columns.Add("AssignFlag", typeof(string)); //9
                        table.Columns.Add("AssignReason", typeof(string)); //10
                        table.Columns.Add("AssignMethod", typeof(string)); //11
                        table.Columns.Add("AssignMethodSortField", typeof(string)); //12
                        table.Columns.Add("AssignMethodSortOrder", typeof(string));    //13
                        table.Columns.Add("AssignTool", typeof(string)); //14
                        table.Columns.Add("AssignTeam", typeof(string)); //15
                        table.Columns.Add("AssignAreaCode", typeof(string)); //16
                        table.Columns.Add("ProvinceCode", typeof(string)); //17
                        table.Columns.Add("DistrictCode", typeof(string)); //18
                        table.Columns.Add("SubDistrictCode", typeof(string)); //19
                        table.Columns.Add("AssignOverCapacity", typeof(string)); //20
                        table.Columns.Add("JobType", typeof(string)); //21                    
                        table.Columns.Add("AssignCollectorId", typeof(string)); //22
                        //table.Columns.Add("SupervisorId", typeof(string));  
                        table.Columns.Add("CreatedBy", typeof(string)); //23
                        table.Columns.Add("CreatedDate", typeof(DateTime)); //24
                        table.Columns.Add("UpdatedBy", typeof(string)); //25
                        table.Columns.Add("UpdatedDate", typeof(DateTime));   //26

                        string GetSafeField(string columnName)
                            => header.Contains(columnName) && !string.IsNullOrWhiteSpace(csv.GetField(columnName))
                            ? csv.GetField(columnName)
                            : null;

                        DateTime now = DateTime.Now;

                        while (await csv.ReadAsync())
                        {
                            DataRow row = table.NewRow();

                            row["AssignmentWorklistId"] = Guid.NewGuid();
                            row["ADEPTProcessingDate"] = (object?)GetSafeField("ADEPT__ProcessingDate") ?? DBNull.Value;
                            row["ADEPTProcessingTime"] = (object?)GetSafeField("ADEPT__ProcessingTime") ?? DBNull.Value;
                            row["ADEPTProcessingVersion"] = (object?)GetSafeField("ADEPT__ProcessingVersion") ?? DBNull.Value;
                            row["ContractNo"] = (object?)GetSafeField("ContractNo") ?? DBNull.Value;
                            row["AllocateMode"] = (object?)GetSafeField("AllocateMode") ?? DBNull.Value;
                            row["AssignType"] = (object?)GetSafeField("AssignType") ?? DBNull.Value;
                            row["DoNotCallFlag"] = (object?)GetSafeField("DoNotCallFlag") ?? DBNull.Value;
                            row["AssignFlag"] = (object?)GetSafeField("AssignFlag") ?? DBNull.Value;
                            row["AssignReason"] = (object?)GetSafeField("AssignReason") ?? DBNull.Value;
                            row["AssignMethod"] = (object?)GetSafeField("AssignMethod") ?? DBNull.Value;
                            row["AssignMethodSortField"] = (object?)GetSafeField("AssignMethodSortField") ?? DBNull.Value;
                            row["AssignMethodSortOrder"] = (object?)GetSafeField("AssignMethodSortOrder") ?? DBNull.Value;
                            row["AssignTool"] = (object?)GetSafeField("AssignTool") ?? DBNull.Value;
                            row["AssignTeam"] = (object?)GetSafeField("AssignTeam") ?? DBNull.Value;
                            row["AssignAreaCode"] = (object?)GetSafeField("AssignAreaCode") ?? DBNull.Value;
                            row["ProvinceCode"] = (object?)GetSafeField("ProvinceCode") ?? DBNull.Value;
                            row["DistrictCode"] = (object?)GetSafeField("DistrictCode") ?? DBNull.Value;
                            row["SubDistrictCode"] = (object?)GetSafeField("SubDistrictCode") ?? DBNull.Value;
                            row["AssignOverCapacity"] = (object?)GetSafeField("AssignOverCapacity") ?? DBNull.Value;
                            row["JobType"] = (object?)GetSafeField("JobType") ?? DBNull.Value;
                            row["AssignCollectorId"] = (object?)GetSafeField("AssignCollectorId") ?? DBNull.Value;
                            //row["SupervisorId"] = (object?)GetSafeField("SupervisorId") ?? DBNull.Value;
                            row["CreatedBy"] = ""; // SYSTEM user
                            row["CreatedDate"] = now;
                            row["UpdatedBy"] = ""; // SYSTEM user
                            row["UpdatedDate"] = now;
                            table.Rows.Add(row);
                        }

                        // Step 2: Use SqlBulkCopy to insert into DB
                        using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, (SqlTransaction)tran.GetDbTransaction())
                        {
                            DestinationTableName = "AssignmentWorklist",
                            BulkCopyTimeout = 0 // No timeout (or you can set it)
                        };

                        await bulkCopy.WriteToServerAsync(table);
                        TotalCSVData = TotalCSVData + table.Rows.Count;
                        LogMessage(LogFilePath, $"Inserted {table.Rows.Count} rows from {Path.GetFileName(filePath)}.");
                    }

                    //move allocate input file to archive folder---
                    MoveFileToArchive(archiveFolderPath, folderPath, Path.GetFileName(filePath));
                }
                catch (Exception ex)
                {
                    LogMessage(LogFilePath, $"Error Import CSV to DB {Path.GetFileName(filePath)}: {ex.Message}");
                    await tran.RollbackAsync();
                    return ex.Message;
                }
            }

            await _unitOfWork.SaveChangesAsync();
            await tran.CommitAsync();
            return resultMessage;
        }

        private async Task<string> ImportCSVToDB_BulkCopy(string filePath)
        {
            TotalCSVData = 0;
            LogMessage(LogFilePath, $"-----Import CSV data to DB-----");
            string resultMessage = "";

            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            var conn = (SqlConnection)_unitOfWork.DbContext.Database.GetDbConnection();
            await _unitOfWork.DbContext.Database.OpenConnectionAsync();

            // Clear target table
            await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync("DELETE FROM AssignmentWorklist");            
            LogMessage(LogFilePath, $"Processing file: {Path.GetFileName(filePath)}");

            try
            {
                using var reader = new StreamReader(filePath);
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    await csv.ReadAsync();
                    csv.ReadHeader();

                    var header = csv.HeaderRecord.ToHashSet(StringComparer.OrdinalIgnoreCase);

                    // Step 1: Prepare DataTable with matching schema
                    DataTable table = new DataTable();
                    table.Columns.Add("AssignmentWorklistId", typeof(Guid)); //1
                    table.Columns.Add("ADEPTProcessingDate", typeof(string)); //2
                    table.Columns.Add("ADEPTProcessingTime", typeof(string)); //3
                    table.Columns.Add("ADEPTProcessingVersion", typeof(string)); //4
                    table.Columns.Add("ContractNo", typeof(string)); //5
                    table.Columns.Add("AllocateMode", typeof(string)); //6
                    table.Columns.Add("AssignType", typeof(string)); //7
                    table.Columns.Add("DoNotCallFlag", typeof(string)); //8
                    table.Columns.Add("AssignFlag", typeof(string)); //9
                    table.Columns.Add("AssignReason", typeof(string)); //10
                    table.Columns.Add("AssignMethod", typeof(string)); //11
                    table.Columns.Add("AssignMethodSortField", typeof(string)); //12
                    table.Columns.Add("AssignMethodSortOrder", typeof(string));    //13
                    table.Columns.Add("AssignTool", typeof(string)); //14
                    table.Columns.Add("AssignTeam", typeof(string)); //15
                    table.Columns.Add("AssignAreaCode", typeof(string)); //16
                    table.Columns.Add("ProvinceCode", typeof(string)); //17
                    table.Columns.Add("DistrictCode", typeof(string)); //18
                    table.Columns.Add("SubDistrictCode", typeof(string)); //19
                    table.Columns.Add("AssignOverCapacity", typeof(string)); //20
                    table.Columns.Add("JobType", typeof(string)); //21                    
                    table.Columns.Add("AssignCollectorId", typeof(string)); //22
                    //table.Columns.Add("SupervisorId", typeof(string));  
                    table.Columns.Add("IsAssigned", typeof(bool)); 
                    table.Columns.Add("CreatedBy", typeof(string)); //23
                    table.Columns.Add("CreatedDate", typeof(DateTime)); //24
                    table.Columns.Add("UpdatedBy", typeof(string)); //25
                    table.Columns.Add("UpdatedDate", typeof(DateTime));   //26

                    string GetSafeField(string columnName)
                        => header.Contains(columnName) && !string.IsNullOrWhiteSpace(csv.GetField(columnName))
                        ? csv.GetField(columnName)
                        : null;

                    DateTime now = DateTime.Now;

                    while (await csv.ReadAsync())
                    {
                        DataRow row = table.NewRow();

                        row["AssignmentWorklistId"] = Guid.NewGuid();
                        row["ADEPTProcessingDate"] = (object?)GetSafeField("ADEPT__ProcessingDate") ?? DBNull.Value;
                        row["ADEPTProcessingTime"] = (object?)GetSafeField("ADEPT__ProcessingTime") ?? DBNull.Value;
                        row["ADEPTProcessingVersion"] = (object?)GetSafeField("ADEPT__ProcessingVersion") ?? DBNull.Value;
                        row["ContractNo"] = (object?)GetSafeField("ContractNo") ?? DBNull.Value;
                        row["AllocateMode"] = (object?)GetSafeField("AllocateMode") ?? DBNull.Value;
                        row["AssignType"] = (object?)GetSafeField("AssignType") ?? DBNull.Value;
                        row["DoNotCallFlag"] = (object?)GetSafeField("DoNotCallFlag") ?? DBNull.Value;
                        row["AssignFlag"] = (object?)GetSafeField("AssignFlag") ?? DBNull.Value;
                        row["AssignReason"] = (object?)GetSafeField("AssignReason") ?? DBNull.Value;
                        row["AssignMethod"] = (object?)GetSafeField("AssignMethod") ?? DBNull.Value;
                        row["AssignMethodSortField"] = (object?)GetSafeField("AssignMethodSortField") ?? DBNull.Value;
                        row["AssignMethodSortOrder"] = (object?)GetSafeField("AssignMethodSortOrder") ?? DBNull.Value;
                        row["AssignTool"] = (object?)GetSafeField("AssignTool") ?? DBNull.Value;
                        row["AssignTeam"] = (object?)GetSafeField("AssignTeam") ?? DBNull.Value;
                        row["AssignAreaCode"] = (object?)GetSafeField("AssignAreaCode") ?? DBNull.Value;
                        row["ProvinceCode"] = (object?)GetSafeField("ProvinceCode") ?? DBNull.Value;
                        row["DistrictCode"] = (object?)GetSafeField("DistrictCode") ?? DBNull.Value;
                        row["SubDistrictCode"] = (object?)GetSafeField("SubDistrictCode") ?? DBNull.Value;
                        row["AssignOverCapacity"] = (object?)GetSafeField("AssignOverCapacity") ?? DBNull.Value;
                        row["JobType"] = (object?)GetSafeField("JobType") ?? DBNull.Value;
                        row["AssignCollectorId"] = (object?)GetSafeField("AssignCollectorId") ?? DBNull.Value;
                        //row["SupervisorId"] = (object?)GetSafeField("SupervisorId") ?? DBNull.Value;
                        row["IsAssigned"] = false;
                        row["CreatedBy"] = ""; // SYSTEM user
                        row["CreatedDate"] = now;
                        row["UpdatedBy"] = ""; // SYSTEM user
                        row["UpdatedDate"] = now;
                        table.Rows.Add(row);
                    }

                    // Step 2: Use SqlBulkCopy to insert into DB
                    using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, (SqlTransaction)tran.GetDbTransaction())
                    {
                        DestinationTableName = "AssignmentWorklist",
                        BulkCopyTimeout = 0 // No timeout (or you can set it)
                    };

                    await bulkCopy.WriteToServerAsync(table);
                    TotalCSVData = TotalCSVData + table.Rows.Count;
                    LogMessage(LogFilePath, $"Inserted {table.Rows.Count} rows from {Path.GetFileName(filePath)}.");
                }                
            }
            catch (Exception ex)
            {
                LogMessage(LogFilePath, $"Error Import CSV to DB {Path.GetFileName(filePath)}: {ex.Message}");
                await tran.RollbackAsync();
                return ex.Message;
            }            

            await _unitOfWork.SaveChangesAsync();
            await tran.CommitAsync();
            return resultMessage;
        }

        private async Task<string> AssignCollectorInAllowcateWorklist()
        {
            string resultMessage = "";
            LogMessage(LogFilePath, "Process assign collection to contact is running------------");
            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            var conn = _unitOfWork.DbContext.Database.GetDbConnection();
            await _unitOfWork.DbContext.Database.OpenConnectionAsync();

            try
            {
                string baseSql = "";
                string strTeamName = "";
                string tmpTeamID = "";
                string strSupervisorID = "";
                string strSortField = "";
                string strSortOrder = "";
                string strAssignMethod = "";
                string strAssignOverCapacity = "";
                string SupervisorOverCap = "";

                DateTime now = DateTime.Now;

                // นำ contract ทั้งหมดใน AssignmentWorklist มาใช้เป็นข้อมูลเพื่อ update ข้อมูลบางตัวใน WorkList(เป็นการล้างงาน)---                
                baseSql = "update WorkList set AssignDate=GETDATE(), AssignTool=NULL,AssignAreaId=NULL,AssignAreaCode=NULL,AssignTeamId=NULL,AssignTeamCode=NULL,AssignCollectorId=NULL,SupervisorId=NULL,ReassignStatus=NULL,UpdatedBy=NULL,UpdatedDate=GETDATE()  " +
                    " where ContractNo in ( select ContractNo from AssignmentWorklist) ";
                LogMessage(LogFilePath, "SQL Clear Worklist before assing collector=" + baseSql);
                await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);

                //query data สำหรับ assignFlag = Y เลือกเฉพาะที่มี Team phone ---
                baseSql = "select a.AssignTeam ,count(a.ContractNo) as AmountOfContact,m.ColTeamId,c.Capacity,c.SupervisorID ,dd.AmountOfCollector,a.AssignMethodSortField,a.AssignMethodSortOrder,a.AssignMethod,a.AssignOverCapacity " +
                        "from AssignmentWorklist a " +
                        "full outer join  ColTeam m on a.AssignTeam = m.ColTeamCode " +
                        "full outer join ColTeam c on m.ColTeamId = c.ColTeamId and c.IsActive = 1 " +
                        "full outer join(select co.ColTeamId, count(co.CollectorID) AmountOfCollector from Collector co group by co.ColTeamId) dd on m.ColTeamId = dd.ColTeamId " +
                        "where a.AssignFlag = 'Y' and a.AssignTool = 'Phone' and a.AssignTeam is not NUll " +
                        "group by a.AssignTeam,m.ColTeamId,c.Capacity,c.SupervisorID,dd.AmountOfCollector,a.AssignMethodSortField,a.AssignMethodSortOrder,a.AssignMethod,a.AssignOverCapacity order by a.AssignTeam; ";

                var resultListDataDic = new List<Dictionary<string, object>>();
                var repository = _unitOfWork.Repository<AssignmentWorklistListTeamPhoneDto>();

                using var command = conn.CreateCommand();
                command.CommandText = baseSql;
                command.Transaction = _unitOfWork.DbContext.Database.CurrentTransaction?.GetDbTransaction();
                LogMessage(LogFilePath, "query data สำหรับ assignFlag = Y เลือกเฉพาะที่มี Team phone=" + baseSql);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        var value = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                        row[columnName] = value;
                    }

                    resultListDataDic.Add(row);
                }
                var teamList = resultListDataDic.Select(row => new AssignmentWorklistListTeamPhoneDto
                {
                    AssignTeam = row.ContainsKey("AssignTeam") ? row["AssignTeam"]?.ToString() : null,
                    AmountOfContact = row.ContainsKey("AmountOfContact") && row["AmountOfContact"] != null ? (int?)row["AmountOfContact"] : null,
                    AssignMethodSortField = row.ContainsKey("AssignMethodSortField") ? row["AssignMethodSortField"]?.ToString() : null,
                    AssignMethodSortOrder = row.ContainsKey("AssignMethodSortOrder") ? row["AssignMethodSortOrder"]?.ToString() : "desc",
                    TeamID = row.ContainsKey("ColTeamId") ? row["ColTeamId"]?.ToString() : null,
                    Capacity = row.ContainsKey("Capacity") && row["Capacity"] != null ? Convert.ToInt32(row["Capacity"]) : 0,
                    //SupervisorID = row.ContainsKey("SupervisorID") ? row["SupervisorID"]?.ToString() : null,
                    AmountOfCollector = row.ContainsKey("AmountOfCollector") && row["AmountOfCollector"] != null ? (int?)row["AmountOfCollector"] : null,
                    AssignMethod = row.ContainsKey("AssignMethod") ? row["AssignMethod"]?.ToString() : null,
                    AssignOverCapacity = row.ContainsKey("AssignOverCapacity") ? row["AssignOverCapacity"]?.ToString() : null
                }).ToList();


                //loop for each phone team---       
                for (int i = 0; i < teamList.Count; i++) //teamList.Count
                {
                    tmpTeamID = teamList[i].TeamID;
                    strTeamName = teamList[i].AssignTeam;
                    //strSupervisorID = teamList[i].SupervisorID;
                    strSortField = teamList[i].AssignMethodSortField;
                    strSortOrder = teamList[i].AssignMethodSortOrder;
                    strAssignMethod = teamList[i].AssignMethod;
                    strAssignOverCapacity = teamList[i].AssignOverCapacity;

                    //ถ้า AssignOverCapacity == "AssignPool", set suppervisor = NULL---
                    SupervisorOverCap = (strAssignOverCapacity == "AssignPool") ? "NULL" : "NULL";


                    LogMessage(LogFilePath, "Loop Team " + strTeamName + " ----------------------");
                    //query Team member details---
                    baseSql = @"SELECT c.CollectorID,  c.Capacity AS CollectorCapacity, ISNULL(COUNT(w.ContractNo), 0) AS CountCollectorWorkAssignment  " +
                            "FROM Collector c LEFT JOIN WorkList w ON w.AssignCollectorId = c.CollectorId  " +
                            "WHERE c.ColTeamId = @TeamId  AND c.IsActive = '1'  " +
                            "GROUP BY c.CollectorId,c.CollectorID, c.Capacity ORDER BY  CountCollectorWorkAssignment;";

                    resultListDataDic = new List<Dictionary<string, object>>();
                    using var command2 = conn.CreateCommand();
                    command2.CommandText = baseSql;
                    command2.Transaction = _unitOfWork.DbContext.Database.CurrentTransaction?.GetDbTransaction();

                    var paramTeamId = command2.CreateParameter();
                    paramTeamId.ParameterName = "@TeamId";
                    paramTeamId.Value = tmpTeamID ?? (object)DBNull.Value;
                    command2.Parameters.Add(paramTeamId);

                    LogMessage(LogFilePath, "SQL query Team member details=" + baseSql + ": " + tmpTeamID);
                    using var reader2 = await command2.ExecuteReaderAsync();
                    while (await reader2.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();

                        for (int j = 0; j < reader2.FieldCount; j++)
                        {
                            var columnName = reader2.GetName(j);
                            var value = await reader2.IsDBNullAsync(j) ? null : reader2.GetValue(j);
                            row[columnName] = value;
                        }

                        resultListDataDic.Add(row);
                    }
                    var itemsTeamMemberDetails = resultListDataDic.Select(row => new TeamMemberDetailsDto
                    {
                        CollectorID = row.ContainsKey("CollectorID") ? row["CollectorID"]?.ToString() : null,
                        CollectorCapacity = row.ContainsKey("CollectorCapacity") && row["CollectorCapacity"] != null ? (int?)row["CollectorCapacity"] : null,
                        CountCollectorWorkAssignment = row.ContainsKey("CountCollectorWorkAssignment") ? (int?)row["CountCollectorWorkAssignment"] : null
                    }).ToList();

                    var allowedSortOrders = new HashSet<string> { "ASC", "DESC" };
                    bool validSortField = !string.IsNullOrWhiteSpace(strSortField) && strSortField != "NA";
                    bool validSortOrder = allowedSortOrders.Contains(strSortOrder?.ToUpperInvariant() ?? "");
                    string sortFieldClause = validSortField ? $"c.{strSortField}" : "''";
                    string orderByClause = validSortField && validSortOrder ? $"ORDER BY c.{strSortField} {strSortOrder.ToUpperInvariant()}" : "";

                    baseSql = $@"SELECT a.ContractNo, a.RiskLevel, {(validSortField ? $"c.{strSortField}" : "''")} AS SourceField
                    FROM AssignmentWorklist a
                    FULL OUTER JOIN Contract c ON a.ContractNo = c.ContractNo
                    WHERE a.AssignFlag = 'Y' AND a.AssignTeam = @AssignTeam
                    {orderByClause};";

                    var repositoryListOfContractForTeam = _unitOfWork.Repository<ListOfContractForTeamDto>();
                    var resultRepoLOC = new List<Dictionary<string, object>>();

                    using var command3 = conn.CreateCommand();
                    command3.CommandText = baseSql;
                    command3.Transaction = _unitOfWork.DbContext.Database.CurrentTransaction?.GetDbTransaction();

                    var assignTeamParam = command3.CreateParameter();
                    assignTeamParam.ParameterName = "@AssignTeam";
                    assignTeamParam.Value = strTeamName;
                    command3.Parameters.Add(assignTeamParam);
                    LogMessage(LogFilePath, "Query work of each team from allowcateWorklist = " + baseSql + ":" + strTeamName);
                    using var reader3 = await command3.ExecuteReaderAsync();

                    var contractListOfTeam = new List<Dictionary<string, object>>();
                    while (await reader3.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();

                        for (int j = 0; j < reader3.FieldCount; j++)
                        {
                            var columnName = reader3.GetName(j);
                            var value = await reader3.IsDBNullAsync(j) ? null : reader3.GetValue(j);
                            row[columnName] = value;
                        }

                        contractListOfTeam.Add(row);
                    }

                    //initial variable for count capacity of each collector in each team---
                    List<DataItem> ListMemberFullCapacity = initialMemberCheckFullCapacity(resultListDataDic.Count);

                    //LogMessage(LogFilePath, "Get Contact Of Team ");
                    int indexMember = 0;
                    bool isMemberAllFullCapacity = false;
                    for (int j = 0; j < contractListOfTeam.Count; j++)
                    {
                        bool isContactAlreadyAssign = false;
                        string tmpContactNo = contractListOfTeam[j]["ContractNo"].ToString();
                        LogMessage(LogFilePath, "Loop Contact : " + tmpContactNo);

                        //if J > team capacity, that mean the contact is over capacity of team, it will set to no collector assign---
                        if (j >= Convert.ToInt32(teamList[i].Capacity.ToString()))
                        {
                            baseSql = "update AssignmentWorklist set AssignCollectorId = NULL,SupervisorId=NULL,UpdatedBy=NULL,UpdatedDate=GETDATE() where ContractNo = '" + tmpContactNo + "'";
                            LogMessage(LogFilePath, "No assign collector, exceed team capacity. Just assign to supervisorID and SQL=" + baseSql);
                            await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);
                        }
                        else
                        {
                            if (itemsTeamMemberDetails.Count == 0) //if no collector for handdle contract, just update contract with no collector---
                            {

                                baseSql = "update AssignmentWorklist set AssignCollectorId = NULL,SupervisorId=" + SupervisorOverCap + ",UpdatedBy=NULL,UpdatedDate=GETDATE() where ContractNo = '" + tmpContactNo + "'";
                                LogMessage(LogFilePath, "No assign collector, team have no member. Just assign to supervisorID. SQL=" + baseSql);
                                await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);
                                isContactAlreadyAssign = true;
                            }
                            else
                            {
                                int tmpCountCollectionWorkAssignment = 0;
                                while (isMemberAllFullCapacity == false)
                                {
                                    //indexMember = indexMember + indexIncrete;
                                    tmpCountCollectionWorkAssignment = Convert.ToInt32(itemsTeamMemberDetails[indexMember].CountCollectorWorkAssignment) + 1;
                                    if (tmpCountCollectionWorkAssignment > Convert.ToInt32(itemsTeamMemberDetails[indexMember].CollectorCapacity))
                                    {
                                        //เป็นการ flag ว่า collector มีการ assign งานเต็ม capacity แล้ว---
                                        ListMemberFullCapacity[indexMember].Value = 1;

                                        indexMember = indexMember + 1;

                                        if ((indexMember) >= itemsTeamMemberDetails.Count)
                                        {
                                            indexMember = 0;
                                        }

                                        //check is all collection is full capacity ?---
                                        bool allFull = ListMemberFullCapacity.All(item => item.Value == 1);
                                        if (allFull)
                                        {
                                            isMemberAllFullCapacity = true;
                                        }
                                    }
                                    else
                                    {
                                        baseSql = "update AssignmentWorklist set AssignCollectorId = " + itemsTeamMemberDetails[indexMember].CollectorID.ToString() + ",SupervisorId='" + strSupervisorID + "',UpdatedBy=NULL,UpdatedDate=GETDATE() where ContractNo = '" + tmpContactNo + "'";
                                        LogMessage(LogFilePath, "Assign collector:" + itemsTeamMemberDetails[indexMember].CollectorID.ToString() + " ,SupervisorId=" + strSupervisorID);
                                        await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);


                                        itemsTeamMemberDetails[indexMember].CountCollectorWorkAssignment = tmpCountCollectionWorkAssignment;
                                        indexMember = indexMember + 1;

                                        //เป็นการเช็คว่าถ้าเลื่อนคนถัดไปมาแล้ว มีคนหรือเปล่าถ้าไม่มีก็ให้ไปเริ่มที่คนแรกใหม่---
                                        if (indexMember >= itemsTeamMemberDetails.Count)
                                        {
                                            indexMember = 0;
                                        }

                                        isContactAlreadyAssign = true;
                                        break;
                                    }
                                }
                            }
                        }

                        //case นี้เป็นเคสที่ สัญญาของทีมนี้ยังไม่หมดแต่ collector มี capacity เต็มหมดแล้ว---
                        if (isContactAlreadyAssign == false)
                        {
                            //ถ้า AssignOverCapacity == "AssignPool", set suppervisor = NULL---
                            baseSql = "update AssignmentWorklist set AssignCollectorId = NULL,SupervisorId=" + SupervisorOverCap + ",UpdatedBy=NULL,UpdatedDate=GETDATE() where ContractNo = '" + tmpContactNo + "'";
                            LogMessage(LogFilePath, "No assign collector, all member's capacity is full. SQL=" + baseSql);
                            await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);

                        }
                    }
                }

                await tran.CommitAsync();

            }
            catch (Exception ex)
            {
                LogMessage(LogFilePath, "Error Process assign collection to contact: " + ex.Message);
                await tran.RollbackAsync();
                return ex.Message;
            }

            return resultMessage;
        }

        private async Task<string> AssignCollectorInAllowcateWorklist_turnning()
        {
            string resultMessage = "";
            LogMessage(LogFilePath, "Process assign collection to contact is running------------");
            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            var conn = _unitOfWork.DbContext.Database.GetDbConnection();
            await _unitOfWork.DbContext.Database.OpenConnectionAsync();

            try
            {
                string baseSql = "";
                string strTeamName = "";
                string tmpTeamID = "";
                string strSupervisorID = "";
                string strSortField = "";
                string strSortOrder = "";
                string strAssignMethod = "";
                string strAssignOverCapacity = "";
                string SupervisorOverCap = "";

                var updateList = new List<AssignUpdateDto>();
                DateTime now = DateTime.Now;

                // นำ contract ทั้งหมดใน AssignmentWorklist มาใช้เป็นข้อมูลเพื่อ update ข้อมูลบางตัวใน WorkList(เป็นการล้างงาน)---                
                baseSql = "update WorkList set AssignDate=GETDATE(), AssignTool=NULL,AssignAreaId=NULL,AssignAreaCode=NULL,AssignTeamId=NULL,AssignTeamCode=NULL,AssignCollectorId=NULL,SupervisorId=NULL,ReassignStatus=NULL,UpdatedBy=NULL,UpdatedDate=GETDATE()  " +
                    " where ContractNo in ( select ContractNo from AssignmentWorklist) ";
                LogMessage(LogFilePath, "SQL Clear Worklist before assing collector=" + baseSql);
                await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);

                //query data สำหรับ assignFlag = Y เลือกเฉพาะที่มี Team phone ---
                baseSql = "select a.AssignTeam ,count(a.ContractNo) as AmountOfContact,m.ColTeamId,c.Capacity,c.SupervisorID ,dd.AmountOfCollector,a.AssignMethodSortField,a.AssignMethodSortOrder,a.AssignMethod,a.AssignOverCapacity " +
                        "from AssignmentWorklist a " +
                        "full outer join  ColTeam m on a.AssignTeam = m.ColTeamCode " +
                        "full outer join ColTeam c on m.ColTeamId = c.ColTeamId and c.IsActive = 1 " +
                        "full outer join(select co.ColTeamId, count(co.CollectorID) AmountOfCollector from Collector co group by co.ColTeamId) dd on m.ColTeamId = dd.ColTeamId " +
                        "where a.AssignFlag = 'Y' and a.AssignTool = 'Phone' and a.AssignTeam is not NUll " +
                        "group by a.AssignTeam,m.ColTeamId,c.Capacity,c.SupervisorID,dd.AmountOfCollector,a.AssignMethodSortField,a.AssignMethodSortOrder,a.AssignMethod,a.AssignOverCapacity order by a.AssignTeam; ";

                var resultListDataDic = new List<Dictionary<string, object>>();
                var repository = _unitOfWork.Repository<AssignmentWorklistListTeamPhoneDto>();

                using var command = conn.CreateCommand();
                command.CommandText = baseSql;
                command.Transaction = _unitOfWork.DbContext.Database.CurrentTransaction?.GetDbTransaction();
                LogMessage(LogFilePath, "query data สำหรับ assignFlag = Y เลือกเฉพาะที่มี Team phone=" + baseSql);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        var value = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                        row[columnName] = value;
                    }

                    resultListDataDic.Add(row);
                }
                var teamList = resultListDataDic.Select(row => new AssignmentWorklistListTeamPhoneDto
                {
                    AssignTeam = row.ContainsKey("AssignTeam") ? row["AssignTeam"]?.ToString() : null,
                    AmountOfContact = row.ContainsKey("AmountOfContact") && row["AmountOfContact"] != null ? (int?)row["AmountOfContact"] : null,
                    AssignMethodSortField = row.ContainsKey("AssignMethodSortField") ? row["AssignMethodSortField"]?.ToString() : null,
                    AssignMethodSortOrder = row.ContainsKey("AssignMethodSortOrder") ? row["AssignMethodSortOrder"]?.ToString() : "desc",
                    TeamID = row.ContainsKey("ColTeamId") ? row["ColTeamId"]?.ToString() : null,
                    Capacity = row.ContainsKey("Capacity") && row["Capacity"] != null ? Convert.ToInt32(row["Capacity"]) : 0,
                    //SupervisorID = row.ContainsKey("SupervisorID") ? row["SupervisorID"]?.ToString() : null,
                    AmountOfCollector = row.ContainsKey("AmountOfCollector") && row["AmountOfCollector"] != null ? (int?)row["AmountOfCollector"] : null,
                    AssignMethod = row.ContainsKey("AssignMethod") ? row["AssignMethod"]?.ToString() : null,
                    AssignOverCapacity = row.ContainsKey("AssignOverCapacity") ? row["AssignOverCapacity"]?.ToString() : null
                }).ToList();


                //loop for each phone team---       
                for (int i = 0; i < teamList.Count; i++) //teamList.Count
                {
                    tmpTeamID = teamList[i].TeamID;
                    strTeamName = teamList[i].AssignTeam;
                    //strSupervisorID = teamList[i].SupervisorID;
                    strSortField = teamList[i].AssignMethodSortField;
                    strSortOrder = teamList[i].AssignMethodSortOrder;
                    strAssignMethod = teamList[i].AssignMethod;
                    strAssignOverCapacity = teamList[i].AssignOverCapacity;

                    //ถ้า AssignOverCapacity == "AssignPool", set suppervisor = NULL---
                    SupervisorOverCap = (strAssignOverCapacity == "AssignPool") ? "NULL" : "NULL";


                    LogMessage(LogFilePath, "Loop Team " + strTeamName + " ----------------------");
                    //query Team member details---
                    baseSql = @"SELECT c.CollectorID,  c.Capacity AS CollectorCapacity, ISNULL(COUNT(w.ContractNo), 0) AS CountCollectorWorkAssignment  " +
                            "FROM Collector c LEFT JOIN WorkList w ON w.AssignCollectorId = c.CollectorId  " +
                            "WHERE c.ColTeamId = @TeamId  AND c.IsActive = '1'  " +
                            "GROUP BY c.CollectorId,c.CollectorID, c.Capacity ORDER BY  CountCollectorWorkAssignment;";

                    resultListDataDic = new List<Dictionary<string, object>>();
                    using var command2 = conn.CreateCommand();
                    command2.CommandText = baseSql;
                    command2.Transaction = _unitOfWork.DbContext.Database.CurrentTransaction?.GetDbTransaction();

                    var paramTeamId = command2.CreateParameter();
                    paramTeamId.ParameterName = "@TeamId";
                    paramTeamId.Value = tmpTeamID ?? (object)DBNull.Value;
                    command2.Parameters.Add(paramTeamId);

                    LogMessage(LogFilePath, "SQL query Team member details=" + baseSql + ": " + tmpTeamID);
                    using var reader2 = await command2.ExecuteReaderAsync();
                    while (await reader2.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();

                        for (int j = 0; j < reader2.FieldCount; j++)
                        {
                            var columnName = reader2.GetName(j);
                            var value = await reader2.IsDBNullAsync(j) ? null : reader2.GetValue(j);
                            row[columnName] = value;
                        }

                        resultListDataDic.Add(row);
                    }
                    var itemsTeamMemberDetails = resultListDataDic.Select(row => new TeamMemberDetailsDto
                    {
                        CollectorID = row.ContainsKey("CollectorID") ? row["CollectorID"]?.ToString() : null,
                        CollectorCapacity = row.ContainsKey("CollectorCapacity") && row["CollectorCapacity"] != null ? (int?)row["CollectorCapacity"] : null,
                        CountCollectorWorkAssignment = row.ContainsKey("CountCollectorWorkAssignment") ? (int?)row["CountCollectorWorkAssignment"] : null
                    }).ToList();

                    var allowedSortOrders = new HashSet<string> { "ASC", "DESC" };
                    bool validSortField = !string.IsNullOrWhiteSpace(strSortField) && strSortField != "NA";
                    bool validSortOrder = allowedSortOrders.Contains(strSortOrder?.ToUpperInvariant() ?? "");
                    string sortFieldClause = validSortField ? $"c.{strSortField}" : "''";
                    string orderByClause = validSortField && validSortOrder ? $"ORDER BY c.{strSortField} {strSortOrder.ToUpperInvariant()}" : "";

                    baseSql = $@"SELECT a.ContractNo, a.RiskLevel, {(validSortField ? $"c.{strSortField}" : "''")} AS SourceField
                    FROM AssignmentWorklist a
                    FULL OUTER JOIN Contract c ON a.ContractNo = c.ContractNo
                    WHERE a.AssignFlag = 'Y' AND a.AssignTeam = @AssignTeam
                    {orderByClause};";

                    var repositoryListOfContractForTeam = _unitOfWork.Repository<ListOfContractForTeamDto>();
                    var resultRepoLOC = new List<Dictionary<string, object>>();

                    using var command3 = conn.CreateCommand();
                    command3.CommandText = baseSql;
                    command3.Transaction = _unitOfWork.DbContext.Database.CurrentTransaction?.GetDbTransaction();

                    var assignTeamParam = command3.CreateParameter();
                    assignTeamParam.ParameterName = "@AssignTeam";
                    assignTeamParam.Value = strTeamName;
                    command3.Parameters.Add(assignTeamParam);
                    LogMessage(LogFilePath, "Query work of each team from allowcateWorklist = " + baseSql + ":" + strTeamName);
                    using var reader3 = await command3.ExecuteReaderAsync();

                    var contractListOfTeam = new List<Dictionary<string, object>>();
                    while (await reader3.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();

                        for (int j = 0; j < reader3.FieldCount; j++)
                        {
                            var columnName = reader3.GetName(j);
                            var value = await reader3.IsDBNullAsync(j) ? null : reader3.GetValue(j);
                            row[columnName] = value;
                        }

                        contractListOfTeam.Add(row);
                    }

                    //initial variable for count capacity of each collector in each team---
                    List<DataItem> ListMemberFullCapacity = initialMemberCheckFullCapacity(resultListDataDic.Count);

                    //LogMessage(LogFilePath, "Get Contact Of Team ");
                    int indexMember = 0;
                    bool isMemberAllFullCapacity = false;
                    for (int j = 0; j < contractListOfTeam.Count; j++)
                    {
                        bool isContactAlreadyAssign = false;
                        string tmpContactNo = contractListOfTeam[j]["ContractNo"].ToString();
                        LogMessage(LogFilePath, "Loop Contact : " + tmpContactNo);

                        //if J > team capacity, that mean the contact is over capacity of team, it will set to no collector assign---
                        if (j >= Convert.ToInt32(teamList[i].Capacity.ToString()))
                        {
                            baseSql = "update AssignmentWorklist set AssignCollectorId = NULL,SupervisorId=NULL,UpdatedBy=NULL,UpdatedDate=GETDATE() where ContractNo = '" + tmpContactNo + "'";
                            LogMessage(LogFilePath, "No assign collector, exceed team capacity. SQL=" + baseSql);
                            await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);
                        }
                        else
                        {
                            if (itemsTeamMemberDetails.Count == 0) //if no collector for handdle contract, just update contract with no collector---
                            {

                                baseSql = "update AssignmentWorklist set AssignCollectorId = NULL,SupervisorId=NULL,UpdatedBy=NULL,UpdatedDate=GETDATE() where ContractNo = '" + tmpContactNo + "'";
                                LogMessage(LogFilePath, "No assign collector, team have no member. SQL=" + baseSql);
                                await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);
                                isContactAlreadyAssign = true;
                            }
                            else
                            {
                                int tmpCountCollectionWorkAssignment = 0;
                                while (isMemberAllFullCapacity == false)
                                {
                                    //indexMember = indexMember + indexIncrete;
                                    tmpCountCollectionWorkAssignment = Convert.ToInt32(itemsTeamMemberDetails[indexMember].CountCollectorWorkAssignment) + 1;
                                    if (tmpCountCollectionWorkAssignment > Convert.ToInt32(itemsTeamMemberDetails[indexMember].CollectorCapacity))
                                    {
                                        //เป็นการ flag ว่า collector มีการ assign งานเต็ม capacity แล้ว---
                                        ListMemberFullCapacity[indexMember].Value = 1;

                                        indexMember = indexMember + 1;

                                        if ((indexMember) >= itemsTeamMemberDetails.Count)
                                        {
                                            indexMember = 0;
                                        }

                                        //check is all collection is full capacity ?---
                                        bool allFull = ListMemberFullCapacity.All(item => item.Value == 1);
                                        if (allFull)
                                        {
                                            isMemberAllFullCapacity = true;
                                        }
                                    }
                                    else
                                    {
                                        baseSql = "update AssignmentWorklist set AssignCollectorId = " + itemsTeamMemberDetails[indexMember].CollectorID.ToString() + ",SupervisorId='" + strSupervisorID + "',UpdatedBy=NULL,UpdatedDate=GETDATE() where ContractNo = '" + tmpContactNo + "'";
                                        LogMessage(LogFilePath, "Assign collector:" + itemsTeamMemberDetails[indexMember].CollectorID.ToString() + " ,SupervisorId=" + strSupervisorID);
                                        await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);


                                        itemsTeamMemberDetails[indexMember].CountCollectorWorkAssignment = tmpCountCollectionWorkAssignment;
                                        indexMember = indexMember + 1;

                                        //เป็นการเช็คว่าถ้าเลื่อนคนถัดไปมาแล้ว มีคนหรือเปล่าถ้าไม่มีก็ให้ไปเริ่มที่คนแรกใหม่---
                                        if (indexMember >= itemsTeamMemberDetails.Count)
                                        {
                                            indexMember = 0;
                                        }

                                        isContactAlreadyAssign = true;
                                        break;
                                    }
                                }
                            }
                        }

                        //case นี้เป็นเคสที่ สัญญาของทีมนี้ยังไม่หมดแต่ collector มี capacity เต็มหมดแล้ว---
                        if (isContactAlreadyAssign == false)
                        {
                            //ถ้า AssignOverCapacity == "AssignPool", set suppervisor = NULL---
                            baseSql = "update AssignmentWorklist set AssignCollectorId = NULL,SupervisorId=" + SupervisorOverCap + ",UpdatedBy=NULL,UpdatedDate=GETDATE() where ContractNo = '" + tmpContactNo + "'";
                            LogMessage(LogFilePath, "No assign collector, all member's capacity is full. SQL=" + baseSql);
                            await _unitOfWork.DbContext.Database.ExecuteSqlRawAsync(baseSql);

                        }
                    }
                }

                await tran.CommitAsync();

            }
            catch (Exception ex)
            {
                LogMessage(LogFilePath, "Error Process assign collection to contact: " + ex.Message);
                await tran.RollbackAsync();
                return ex.Message;
            }

            return resultMessage;
        }

        private async Task<string> AssignCollectorInAssignmentWorklist_BulkCopy()
        {
            LogMessage(LogFilePath, "-----Process assign collection in table AssignmentWorklist-----");

            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            var conn = _unitOfWork.DbContext.Database.GetDbConnection();
            await _unitOfWork.DbContext.Database.OpenConnectionAsync();

            try
            {
                DateTime now = DateTime.Now;

                string fetchFollowupStatusSql = @"SELECT EnumCode,EnumDescription FROM SysEnum WHERE EnumName = 'FollowupStatus'";
                var followupStatusData = await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, fetchFollowupStatusSql);

                var newStatus = followupStatusData.FirstOrDefault(x => x["EnumCode"].ToString() == "NEW");
                var carryOverStatus = followupStatusData.FirstOrDefault(x => x["EnumCode"].ToString() == "COV");

                // 1. เคลียร์ข้อมูลใน WorkList ที่ไม่ต้องติดตาม --> AssignFlag='N'---
                string clearWorklistSql = @"
                UPDATE w
                SET w.AssignDate = GETDATE(),                     
                    w.AssignAreaId = NULL,
                    w.AssignAreaCode = NULL,
                    w.AssignTeamId = NULL,
                    w.AssignTeamCode = NULL,
                    w.AssignTeamName = NULL,
                    w.AssignCollectorId = NULL, 
                    w.AssignCollectorEmpId = NULL,
                    w.AssignCollectorName = NULL,
                    w.AssignTypeCode = NULL,
                    w.AssignTypeDesc = NULL,                    
                    w.ReassignById = NULL,
                    w.ReassignByEmpId = NULL,
                    w.ReassignByName = NULL,                    
                    w.ReassignRequestDate = NULL,
                    w.ReassignFromId = NULL,
                    w.ReassignFromEmpId = NULL,
                    w.ReassignFromName = NULL,
                    w.ReassignToId = NULL,
                    w.ReassignToEmpId = NULL,
                    w.ReassignToName = NULL,
                    w.ReassignReason = NULL,
                    w.ReassignStatusCode = NULL,
                    w.ReassignStatusDesc = NULL,
                    w.ApprovedById = NULL,
                    w.ApprovedEmpId = NULL,
                    w.ApprovedName = NULL,
                    w.ApproveReason = NULL,
                    w.ApproveDate = NULL,
                    w.FollowupStatusCode = NULL,
                    w.FollowupStatusDesc = NULL,
                    w.FollowupDate = NULL,                   
                    w.DoNotCallFlag = NULL,
                    w.UpdatedBy = NULL,
                    w.UpdatedDate = GETDATE(),
                    w.JobTypeCode = a.JobType,
	                w.JobTypeDesc = s.EnumDescription
                FROM WorkList w
                INNER JOIN AssignmentWorklist a  ON w.ContractNo = a.ContractNo
                LEFT OUTER JOIN SysEnum s ON s.EnumCode = a.JobType AND s.EnumName = 'JobType'
                WHERE a.AssignFlag = 'N';";

                var updateClearContract = await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, clearWorklistSql);
                //LogMessage(LogFilePath, "Clear contract that no need to follow up on WorkList SQL: " + clearWorklistSql);
                                
                // 2. อัพเดท status ข้อมูลใน WorkList ของสัญญาที่ต้องติดตามต่อ --> AssignFlag='X'---
                string updateWorklistSql = @"
                UPDATE w
                SET 
                    w.AssignDate = GETDATE(),
                    w.FollowupStatusCode = @FollowupStatusCode,
                    w.FollowupStatusDesc = @FollowupStatusDesc,
                    w.FollowupDate = GETDATE(),
                    w.UpdatedBy = NULL,
                    w.UpdatedDate = GETDATE(),
                    w.JobTypeCode = a.JobType,
	                w.JobTypeDesc = s.EnumDescription
                FROM WorkList w
                INNER JOIN AssignmentWorklist a  ON w.ContractNo = a.ContractNo
                LEFT OUTER JOIN SysEnum s ON s.EnumCode = a.JobType AND s.EnumName = 'JobType'
                WHERE a.AssignFlag = 'X';";

                await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, updateWorklistSql, new Dictionary<string, object>
                {
                    ["@FollowupStatusCode"] = carryOverStatus?["EnumCode"]?.ToString(),
                    ["@FollowupStatusDesc"] = carryOverStatus?["EnumDescription"]?.ToString()
                });
                //LogMessage(LogFilePath, "Update WorkList for Carry Over SQL: " + updateWorklistSql);

                // 3. เตรียม DataTable สำหรับการ bulk update---
                DataTable assignCollectorTable = new DataTable();
                assignCollectorTable.Columns.Add("ContractNo", typeof(string));
                assignCollectorTable.Columns.Add("AssignTeam", typeof(string));
                assignCollectorTable.Columns.Add("AssignCollectorId", typeof(string));
                //assignCollectorTable.Columns.Add("SupervisorId", typeof(string));
                assignCollectorTable.Columns.Add("UpdatedDate", typeof(DateTime));

                // 4. ดึงข้อมูลทีมที่ต้อง assign ใน table AssignmentWorklist---
                string fetchTeamSql = @"
                SELECT a.AssignTeam, COUNT(a.ContractNo) AS AmountOfContact, a.AssignMethodSortField, a.AssignMethodSortOrder, a.AssignMethod, a.AssignOverCapacity,
                m.ColTeamId,m.Capacity,m.ColTeamCode,m.ColTeamName,ISNULL(col1.AmountOfCollector,0) AS AmountOfCollector
                ,ISNULL(col3.TeamTotalCurrentWork,0) AS TeamTotalCurrentWork
                FROM AssignmentWorklist a
                FULL OUTER JOIN ColTeam m ON LOWER(a.AssignTeam) = LOWER(m.ColTeamCode) and m.IsActive = 1
                FULL OUTER JOIN (  
	                SELECT co.ColTeamId, COUNT(co.CollectorID) AS AmountOfCollector
	                FROM vw_CollectorAndTeamStatus co GROUP BY co.ColTeamId
                ) col1 ON col1.ColTeamId = m.ColTeamId
                FULL OUTER JOIN (
	                 select count(*) as TeamTotalCurrentWork ,c.ColTeamId 
	                from WorkList w 
	                FULL OUTER JOIN vw_CollectorAndTeamStatus c ON  w.AssignCollectorId = c.CollectorId
	                where w.AssignTeamId IS NOT NULL 
	                and w.ContractNo not in (select ContractNo from AssignmentWorklist where AssignFlag = 'Y' ) --and AssignTeam = w.AssignTeamName
	                group by c.ColTeamId
	                ) col3 on m.ColTeamId = col3.ColTeamId
                WHERE a.AssignFlag = 'Y' AND LOWER(a.AssignTool) = LOWER('Phone') AND a.AssignTeam IS NOT NULL
                AND m.ColTeamId IS NOT NULL
                GROUP BY a.AssignTeam, a.AssignMethodSortField, a.AssignMethodSortOrder, a.AssignMethod, a.AssignOverCapacity,m.ColTeamId,m.Capacity,m.ColTeamCode
                ,m.ColTeamName,col1.AmountOfCollector,col3.TeamTotalCurrentWork
                ORDER BY a.AssignTeam";
                //LogMessage(LogFilePath, "ดึงข้อมูลทีมที่ต้อง assign: " + fetchTeamSql);
                var teamData = await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, fetchTeamSql);
                var teamList = teamData.Select(row => new AssignmentWorklistListTeamPhoneDto
                {
                    AssignTeam = row["AssignTeam"]?.ToString(),
                    TeamID = row["ColTeamId"]?.ToString(),
                    Capacity = row["Capacity"] != null ? Convert.ToInt32(row["Capacity"]) : 0,
                    //SupervisorID = row["SupervisorID"]?.ToString(),
                    AssignMethodSortField = row["AssignMethodSortField"] != null ? row["AssignMethodSortField"]?.ToString() : "",
                    AssignMethodSortOrder = row["AssignMethodSortOrder"] != null ? row["AssignMethodSortOrder"]?.ToString() : "",
                    AssignMethod = row["AssignMethod"]?.ToString(),
                    AssignOverCapacity = row["AssignOverCapacity"]?.ToString(),
                    TeamTotalCurrentWork = row["TeamTotalCurrentWork"] != null ? Convert.ToInt32(row["TeamTotalCurrentWork"]) : 0,
                }).ToList();

                // 5. วนลูปทุกทีมเพื่อ assign collector โดยจะ process ทีละทีม---
                foreach (var team in teamList)
                {
                    var teamId = team.TeamID;
                    var teamName = team.AssignTeam;
                    var overCapMode = team.AssignOverCapacity;
                    var assignMethodSortField = team.AssignMethodSortField;
                    var assignMethodSortOrder = team.AssignMethodSortOrder;
                    //var supervisorId = team.SupervisorID=="NULL"? null : team.SupervisorID;
                    var assignTeamFinal = (overCapMode == "AssignTeam") ? teamName : null;

                    // ดึงรายละเอียดของสมาชิกในทีม---
                    /*string fetchMembersSql = @"
                    SELECT c.CollectorID, c.Capacity AS CollectorCapacity
                    , ISNULL(COUNT(w.ContractNo), 0) AS CountCollectorWorkAssignment,ct.ColTeamName as AssignTeam
                    FROM ColTeamAssignment c
                    LEFT JOIN WorkList w ON w.AssignCollectorId = c.CollectorId
                    and w.ContractNo not in (select ContractNo from AssignmentWorklist where AssignFlag = 'Y' ) -- and AssignTeam = w.AssignTeamName
                    INNER JOIN	ColTeam ct ON ct.ColTeamId = c.ColTeamId
                    WHERE c.ColTeamId = @TeamId AND c.IsActive = 1  
                    AND c.EffectiveDate <= GETDATE() AND (c.ExpireDate > GETDATE() or c.ExpireDate IS NULL)
                    GROUP BY c.CollectorId, c.Capacity,ct.ColTeamName
                    ORDER BY CountCollectorWorkAssignment";*/

                    string fetchMembersSql = @"
                    SELECT c.CollectorID, c.CollectorCapacity,c.ColTeamName as AssignTeam
                    ,ISNULL(COUNT(w.ContractNo), 0) AS CountCollectorWorkAssignment                    
                    FROM vw_CollectorAndTeamStatus c
                    LEFT JOIN WorkList w ON w.AssignCollectorId = c.CollectorId
                    and w.ContractNo not in (select ContractNo from AssignmentWorklist where AssignFlag = 'Y' ) 
                    WHERE c.ColTeamId = @TeamId   
                    GROUP BY c.CollectorId, c.CollectorCapacity,c.ColTeamName
                    ORDER BY CountCollectorWorkAssignment";
                    //LogMessage(LogFilePath, "ดึงรายละเอียดของสมาชิกในทีม: " + fetchMembersSql);
                    var members = await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, fetchMembersSql, new Dictionary<string, object>
                    {
                        ["@TeamId"] = teamId
                    });

                    var memberList = members.Select(m => new TeamMemberDetailsDto
                    {
                        AssignTeam = m["AssignTeam"]?.ToString(),
                        CollectorID = m["CollectorID"]?.ToString(),
                        CollectorCapacity = m["CollectorCapacity"] != null ? Convert.ToInt32(m["CollectorCapacity"]) : 0,
                        CountCollectorWorkAssignment = m["CountCollectorWorkAssignment"] != null ? Convert.ToInt32(m["CountCollectorWorkAssignment"]) : 0
                    }).ToList();

                    // ดึงสัญญาทั้งหมดของทีมในรอบการแจกงานนี้ โดยพิจารณา assignMethodSort ด้วย---
                    string fetchContractsSql = "";
                    if (assignMethodSortField != "")
                    {
                        string orderStr = $"c.{assignMethodSortField} {assignMethodSortOrder}";
                        fetchContractsSql = $@"
                        SELECT a.ContractNo 
                        FROM AssignmentWorklist a
                        INNER JOIN Contract c on a.ContractNo = c.ContractNo
                        WHERE a.AssignFlag = 'Y' AND a.AssignTeam = @AssignTeam 
                        ORDER BY {orderStr}";
                    }
                    else
                    {
                        fetchContractsSql = $@"
                        SELECT a.ContractNo 
                        FROM AssignmentWorklist a
                        WHERE a.AssignFlag = 'Y' AND a.AssignTeam = @AssignTeam
                        ORDER BY a.ContractNo";
                    }
                    //LogMessage(LogFilePath, "ดึงสัญญาทั้งหมดของทีม: " + fetchContractsSql);
                    var contracts = await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, fetchContractsSql, new Dictionary<string, object>
                    {
                        ["@AssignTeam"] = teamName
                    });

                    int index = 0;
                    var full = new bool[memberList.Count];

                    //นำสัญญาที่ query ได้มาวนลูปแจกงานในทีม---
                    for (int i = 0; i < contracts.Count; i++)
                    {
                        var contractNo = contracts[i]["ContractNo"]?.ToString();

                        //if new work plus current work of team over team capacity,just assign to pool---
                        if (((i + 1) + team.TeamTotalCurrentWork) > team.Capacity || memberList.Count == 0)
                        {
                            //assignCollectorTable.Rows.Add(contractNo, null, supervisorFinal, now);
                            assignCollectorTable.Rows.Add(contractNo, null, null, now);
                            continue;
                        }

                        bool isExist = false;
                        bool assigned = false;   
                        while (isExist == false)
                        {
                            var member = memberList[index];

                            //ถ้างานที่เพิ่มยังไม่เกิน capacity ของ collector ก็เพิ่มงานเข้าไปได้---
                            if (member.CountCollectorWorkAssignment < member.CollectorCapacity)
                            {
                                member.CountCollectorWorkAssignment++;
                                assignCollectorTable.Rows.Add(contractNo, member.AssignTeam, member.CollectorID, now);
                                assigned = true;

                                //เป็นเทคนิคเพิ่ม index ไปเรื่อยๆ และเมื่อถึง limit ก็จะเริ่ม 0 ใหม่---
                                index = (index + 1) % memberList.Count;

                                isExist = true;
                            }
                            else
                            {
                                full[index] = true;

                                //check ว่าทุกคนเต็ม capacity แล้วใช่หรือไม่---
                                bool allTrue = full.All(x => x == true);
                                if (allTrue)
                                {
                                    isExist = true;
                                }
                                else
                                {
                                    index = (index + 1) % memberList.Count;
                                }                                    
                            }              
                        }

                        //ถ้างานไม่เข้าเงื่อนไขไหนเลยก็จะถูก set ไปที่ pool กลาง หรือทีมตามเงื่อนไข overCapacity---
                        if (!assigned)
                        {
                            assignCollectorTable.Rows.Add(contractNo, assignTeamFinal, null, now);
                        }
                    }
                }

                // 6. สร้าง Temp Table (ต้องใช้คู่กับ Bulkcopy)---
                string createTempTable = @"
                    CREATE TABLE #TempAssignCollector (
                        ContractNo NVARCHAR(50),
                        AssignTeam NVARCHAR(250),
                        AssignCollectorId NVARCHAR(50),
                        --SupervisorId NVARCHAR(50),
                        UpdatedDate DATETIME
                    );";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = createTempTable;
                    cmd.Transaction = tran.GetDbTransaction();
                    await cmd.ExecuteNonQueryAsync();
                }

                // 7. ใช้ SqlBulkCopy ---
                using (var bulk = new SqlBulkCopy((SqlConnection)conn, SqlBulkCopyOptions.Default, (SqlTransaction)tran.GetDbTransaction()))
                {
                    bulk.DestinationTableName = "#TempAssignCollector";
                    await bulk.WriteToServerAsync(assignCollectorTable);
                }

                // 8. Merge Temp Table เข้ากับ AssignmentWorklist
                string mergeSql = @"
                    UPDATE A
                    SET 
                        A.AssignCollectorId = T.AssignCollectorId,
                        A.AssignTeam = T.AssignTeam,
                        --A.SupervisorId = T.SupervisorId,
                        A.UpdatedDate = T.UpdatedDate,
                        A.UpdatedBy = NULL
                    FROM AssignmentWorklist A
                    INNER JOIN #TempAssignCollector T ON A.ContractNo = T.ContractNo";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = mergeSql;
                    cmd.Transaction = tran.GetDbTransaction();
                    await cmd.ExecuteNonQueryAsync();
                }

                // 9. Drop temp table---
                string dropTempTable = @"
                IF OBJECT_ID('tempdb..#TempAssignCollector') IS NOT NULL
                    DROP TABLE #TempAssignCollector;";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = dropTempTable;
                    cmd.Transaction = tran.GetDbTransaction();
                    await cmd.ExecuteNonQueryAsync();
                }

                // 10. Commit---
                await tran.CommitAsync();
                return "";
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                LogMessage(LogFilePath, "Error Process assign collection in table AssignmentWorklist: " + ex.Message);
                return ex.Message;
            }
            
        }

        private DataTable CreateAssignUpdateDataTable(List<AssignUpdateDto> updates)
        {
            var table = new DataTable();
            table.Columns.Add("ContractNo", typeof(string));
            table.Columns.Add("AssignCollectorId", typeof(string));
            table.Columns.Add("SupervisorId", typeof(string));
            table.Columns.Add("UpdatedDate", typeof(DateTime));

            foreach (var item in updates)
            {
                table.Rows.Add(item.ContractNo, item.AssignCollectorId ?? (object)DBNull.Value,
                               item.SupervisorId ?? (object)DBNull.Value, item.UpdatedDate);
            }

            return table;
        }

        private async Task<string> UpdateWorkList_BulkCopy()
        {
            LogMessage(LogFilePath, "-----Process update WorkList-----");

            string processResult = "";
            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();

            try
            {
                var conn = (SqlConnection)_unitOfWork.DbContext.Database.GetDbConnection();
                await _unitOfWork.DbContext.Database.OpenConnectionAsync();

                // 1. ดึงข้อมูลใน AssignmentWorklist ที่ต้องการเพิ่มหรืออัพเดทใน worklist---
                //CONVERT(VARCHAR(19), GETDATE(), 120) AS UpdatedDate,   -- yyyy-MM-dd HH:mm:ss
                var command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT 
                        A.ContractNo,
                        GETDATE() AS UpdatedDate,  
	                    SE.EnumCode AS JobTypeCode,SE.EnumDescription AS JobTypeDesc,
	                    SEType.EnumCode AS AssignTypeCode,SEType.EnumDescription AS AssignTypeDesc,
	                    SEF.EnumCode AS FollowupStatusCode,SEF.EnumDescription AS FollowupStatusDesc,
                        A.AssignTool,
	                    NULL AS AssignAreaId,
                        A.AssignAreaCode,
                        A.AssignTeam AS AssignTeamCode,
                        A.AssignCollectorId,
                        C.ColTeamId AS AssignTeamId,
                        C.ColTeamName AS AssignTeamName,
                        CA.AreaId,
                        empC.EmployeeId AS AssignCollectorEmpId,
                        empC.PrefixName+ ' ' + empC.FirstName + ' ' + empC.LastName AS AssignCollectorName,    
                        CASE WHEN EXISTS (SELECT 1 FROM Worklist B WHERE B.ContractNo = A.ContractNo) THEN 1 ELSE 0 END AS IsFoundInWorkList
                    FROM AssignmentWorklist A
                    LEFT JOIN ColTeam C ON A.AssignTeam = C.ColTeamCode 
                    LEFT JOIN SysEnum SE ON SE.EnumName = 'JobType' AND SE.EnumCode = A.JobType
                    LEFT JOIN SysEnum SEType ON SEType.EnumName = 'AssignType' AND SEType.EnumCode = A.AssignType
                    LEFT JOIN SysEnum SEF ON SEF.EnumName = 'FollowupStatus' AND SEF.EnumCode = 'NEW'
                    LEFT JOIN ColArea CA ON A.AssignAreaCode = CA.AreaCode
                    LEFT JOIN ColTeamAssignment CTA ON CTA.ColTeamId = C.ColTeamId AND IsSupervisor = '1'                  
                    LEFT JOIN (
                            SELECT 
			                    cp.CollectorId,
			                    su.EmployeeId,
			                    PF.PrefixName,
			                    emp.FirstName,
			                    emp.LastName
		                    FROM CollectorProfile cp
		                    LEFT JOIN SysUser su ON su.UserId = cp.UserId
		                    LEFT JOIN EmployeeProfile emp ON emp.EmployeeId = su.EmployeeId
		                    LEFT JOIN Prefix PF on PF.PrefixCode = emp.PrefixId
		                    ) empC ON empC.CollectorId = A.AssignCollectorId
                    WHERE 
                        A.AssignFlag = 'Y'
                        AND A.AssignTool = 'Phone';";
                command.Transaction = (SqlTransaction)tran.GetDbTransaction();
                //LogMessage(LogFilePath, "ดึงข้อมูลใน AssignmentWorklist ที่ต้องการ :" + command.CommandText);

                var resultList = new List<AllocateWorklistListUpdateWorkListDto>();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    resultList.Add(new AllocateWorklistListUpdateWorkListDto
                    {
                        ContractNo = reader["ContractNo"]?.ToString(),
                        AssignDate = reader["UpdatedDate"] != DBNull.Value? (DateTime?)Convert.ToDateTime(reader["UpdatedDate"]): null,
                        JobTypeCode = reader["JobTypeCode"] != DBNull.Value ? reader["JobTypeCode"].ToString() : null,
                        JobTypeDesc = reader["JobTypeDesc"] != DBNull.Value ? reader["JobTypeDesc"].ToString() : null,
                        AssignTool = reader["AssignTool"] != DBNull.Value ? reader["AssignTool"].ToString() : null,
                        AssignAreaId = reader["AssignAreaId"] == DBNull.Value ? (Guid?)null : Guid.TryParse(reader["AssignAreaId"]?.ToString(), out var areaId) ? areaId : (Guid?)null,
                        AssignAreaCode = reader["AssignAreaCode"] != DBNull.Value ? reader["AssignAreaCode"].ToString() : null,
                        AssignTeamId = reader["AssignTeamId"] == DBNull.Value ? (Guid?)null : Guid.TryParse(reader["AssignTeamId"]?.ToString(), out var teamId) ? teamId : (Guid?)null,
                        AssignTeamCode = reader["AssignTeamCode"] != DBNull.Value ? reader["AssignTeamCode"].ToString() : null,
                        AssignTeamName = reader["AssignTeamName"] != DBNull.Value ? reader["AssignTeamName"].ToString() : null,
                        AssignCollectorId = reader["AssignCollectorId"] == DBNull.Value ? (Guid?)null : Guid.TryParse(reader["AssignCollectorId"]?.ToString(), out var collectorId) ? collectorId : (Guid?)null,
                        AssignCollectorEmpId = reader["AssignCollectorEmpId"] != DBNull.Value ? reader["AssignCollectorEmpId"].ToString() : null,
                        AssignCollectorName = reader["AssignCollectorName"] != DBNull.Value ? reader["AssignCollectorName"].ToString() : null,
                        AssignTypeCode = reader["AssignTypeCode"] != DBNull.Value ? reader["AssignTypeCode"].ToString() : null,
                        AssignTypeDesc = reader["AssignTypeDesc"] != DBNull.Value ? reader["AssignTypeDesc"].ToString() : null,
                        FollowupStatusCode = reader["FollowupStatusCode"] != DBNull.Value ? reader["FollowupStatusCode"].ToString() : null,
                        FollowupStatusDesc = reader["FollowupStatusDesc"] != DBNull.Value ? reader["FollowupStatusDesc"].ToString() : null,
                        IsFoundInWorkList = reader["IsFoundInWorkList"] != DBNull.Value ? Convert.ToInt32(reader["IsFoundInWorkList"]) : 0
                    });
                }

                if (resultList.Count == 0)
                {
                    LogMessage(LogFilePath, "No update contract to worklist!");
                    return "";
                }

                // 2. แยกข้อมูล เพิ่มใหม่  อัพเดท และ history---
                var insertList = resultList.Where(r => r.IsFoundInWorkList == 0).ToList();
                var updateList = resultList.Where(r => r.IsFoundInWorkList == 1).ToList();

                //keep amout of insert and update data for show in summary log---
                TotalInsertData = insertList.Count;
                TotalUpdateData = updateList.Count;

                // 3. ใช้ BulkCopy สำหรับ insert---
                //LogMessage(LogFilePath, "ใช้ BulkCopy สำหรับ insert :");
                var insertTable = CreateWorkListDataTable(insertList);                
                if (insertTable.Rows.Count > 0)
                {
                    using var insertCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.FireTriggers, (SqlTransaction)tran.GetDbTransaction());
                    insertCopy.DestinationTableName = "WorkList";
                    await insertCopy.WriteToServerAsync(insertTable);
                }

                // 4. ใช้ BulkCopy + MERGE สำหรับ update ---               
                var tempUpdateTable = CreateUpdateWorkListTempTable(updateList);

                // 4.1 สร้าง Temp Table---
                //LogMessage(LogFilePath, "สร้าง Temp Table :");
                string createTempSql = @"
                CREATE TABLE #TempWorkListUpdate (
                    ContractNo NVARCHAR(50),
                    AssignDate datetime2,
                    JobTypeCode NVARCHAR(50),
                    JobTypeDesc NVARCHAR(250),
                    AssignTool NVARCHAR(50),
                    AssignAreaId uniqueidentifier,
                    AssignAreaCode NVARCHAR(50),
                    AssignTeamId uniqueidentifier,
                    AssignTeamCode NVARCHAR(50),
                    AssignTeamName NVARCHAR(250),
                    AssignCollectorId uniqueidentifier,
                    AssignCollectorEmpId NVARCHAR(50),
                    AssignCollectorName NVARCHAR(250),
                    AssignTypeCode NVARCHAR(50),
                    AssignTypeDesc NVARCHAR(250),
                    FollowupStatusCode NVARCHAR(50),
                    FollowupStatusDesc NVARCHAR(250),
                    FollowupDate datetime2,                                    
                    UpdatedDate datetime2   );";

                using var createCmd = new SqlCommand(createTempSql, conn, (SqlTransaction)tran.GetDbTransaction());
                await createCmd.ExecuteNonQueryAsync();

                //LogMessage(LogFilePath, "update temp table :");
                using var updateCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.FireTriggers, (SqlTransaction)tran.GetDbTransaction());
                updateCopy.DestinationTableName = "#TempWorkListUpdate";
                await updateCopy.WriteToServerAsync(tempUpdateTable);

                // 4.2 Merge update data from temp to Worklist---
                //LogMessage(LogFilePath, "merge temp and worklist table:");
                string mergeSql = @"
                MERGE WorkList AS TARGET
                USING #TempWorkListUpdate AS SOURCE
                ON TARGET.ContractNo = SOURCE.ContractNo
                WHEN MATCHED THEN
                UPDATE SET 
                    AssignDate = SOURCE.AssignDate,
                    JobTypeCode = SOURCE.JobTypeCode,
                    JobTypeDesc = SOURCE.JobTypeDesc,
                    AssignTool = SOURCE.AssignTool,
                    AssignAreaId = SOURCE.AssignAreaId,
                    AssignAreaCode = SOURCE.AssignAreaCode,
                    AssignTeamId = SOURCE.AssignTeamId,
                    AssignTeamCode = SOURCE.AssignTeamCode,
                    AssignTeamName = SOURCE.AssignTeamName,
                    AssignCollectorId = SOURCE.AssignCollectorId,
                    AssignCollectorEmpId = SOURCE.AssignCollectorEmpId,
                    AssignCollectorName = SOURCE.AssignCollectorName,
                    AssignTypeCode = SOURCE.AssignTypeCode,
                    AssignTypeDesc = SOURCE.AssignTypeDesc,
                    ReassignById = NULL,
                    ReassignByEmpId = NULL,
                    ReassignByName =  NULL,
                    ReassignRequestDate =  NULL,
                    ReassignFromId =  NULL,
                    ReassignFromEmpId =  NULL,
                    ReassignFromName =  NULL,
                    ReassignToId =  NULL,
                    ReassignToEmpId =  NULL,
                    ReassignToName =  NULL,
                    ReassignToTeamId =  NULL,
                    ReassignToTeamCode =  NULL,
                    ReassignToTeamName =  NULL,
                    ReassignReason =  NULL,
                    ReassignStatusCode =  NULL,
                    ReassignStatusDesc =  NULL,
                    ApprovedById =  NULL,
                    ApprovedEmpId =  NULL,
                    ApprovedName =  NULL,
                    ApproveReason =  NULL,
                    ApproveDate =  NULL,
                    FollowupStatusCode = SOURCE.FollowupStatusCode,
                    FollowupStatusDesc = SOURCE.FollowupStatusDesc,
                    FollowupDate = SOURCE.AssignDate,
                    DoNotCallFlag =  NULL,                    
                    UpdatedDate = SOURCE.AssignDate;";


                using var mergeCmd = new SqlCommand(mergeSql, conn, (SqlTransaction)tran.GetDbTransaction());
                await mergeCmd.ExecuteNonQueryAsync();

                //// 5. ใช้ BulkCopy สำหรับ insert data to WorkListHistory
                //LogMessage(LogFilePath, "insert worklistHistory table:");
                ////--------------------------------------------------------------------
                //// แปลง historyList เป็น DataTable                
                //DataTable contractTable = new DataTable();
                //contractTable.Columns.Add("ContractNo", typeof(string));
                //foreach (var contract in historyList)
                //{
                //    contractTable.Rows.Add(contract.ContractNo);
                //}

                //LogMessage(LogFilePath, "create temp table:");
                //// ใส่ลง temp table ใน SQL Server ผ่าน SqlBulkCopy
                //string createTempContractSql = @"CREATE TABLE #TempContract (ContractNo NVARCHAR(50))";
                //using var createContractCmd = new SqlCommand(createTempContractSql, conn, (SqlTransaction)tran.GetDbTransaction());
                //await createContractCmd.ExecuteNonQueryAsync();

                //LogMessage(LogFilePath, "insert data to temp table:");
                //// insert data to temp table      
                //using var updateContractCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, (SqlTransaction)tran.GetDbTransaction());
                //updateContractCopy.DestinationTableName = "#TempContract";
                //updateContractCopy.ColumnMappings.Add("ContractNo", "ContractNo");
                //await updateContractCopy.WriteToServerAsync(contractTable);

                //LogMessage(LogFilePath, "insert data worklist to datatable:");
                //// insert data worklist to datatable
                //var result = new List<WorklistHistoryDto>();
                //string selectSql = @"
                //SELECT w.*
                //FROM Worklist w
                //INNER JOIN #TempContract t ON w.ContractNo = t.ContractNo";

                //using (var cmd = new SqlCommand(selectSql, (SqlConnection)conn, (SqlTransaction)tran.GetDbTransaction()))
                //using (var reader2 = await cmd.ExecuteReaderAsync())
                //{
                //    while (await reader2.ReadAsync())
                //    {
                //        // Map ข้อมูลจาก reader มาเป็น Worklist object
                //        var item = new WorklistHistoryDto
                //        {
                //            //Id = Guid.NewGuid().ToString(),
                //            WorklistId = reader2["WorklistId"].ToString(),
                //            AssignDate = reader2["AssignDate"] != DBNull.Value ? (DateTime?)reader2["AssignDate"] : null,
                //            ContractNo = reader2["ContractNo"].ToString(),
                //            JobTypeId = reader2["JobTypeId"] != DBNull.Value ? (int?)Convert.ToInt32(reader2["JobTypeId"]) : null,
                //            JobType = reader2["JobType"].ToString(),
                //            RiskLevel = reader2["RiskLevel"].ToString(),
                //            AssignTool = reader2["AssignTool"].ToString(),
                //            AssignAreaId = reader2["AssignAreaId"] != DBNull.Value ? (int?)Convert.ToInt32(reader2["AssignAreaId"]) : null,
                //            AssignAreaCode = reader2["AssignAreaCode"].ToString(),
                //            AssignTeamId = reader2["AssignTeamId"] != DBNull.Value ? (int?)Convert.ToInt32(reader2["AssignTeamId"]) : null,
                //            AssignTeamCode = reader2["AssignTeamCode"].ToString(),
                //            AssignCollectorId = reader2["AssignCollectorId"] != DBNull.Value ? reader2["AssignCollectorId"].ToString() : null,
                //            SupervisorId = reader2["SupervisorId"] != DBNull.Value ? reader2["SupervisorId"].ToString() : null,
                //            AssignTypeId = reader2["AssignTypeId"] != DBNull.Value ? (int?)Convert.ToInt32(reader2["AssignTypeId"]) : null,
                //            AssignTypeCode = reader2["AssignTypeCode"].ToString(),
                //            ReassignFrom = reader2["ReassignFrom"].ToString(),
                //            ReassignTo = reader2["ReassignTo"].ToString(),
                //            ReassignReason = reader2["ReassignReason"].ToString(),
                //            ReassignStatusId = reader2["ReassignStatusId"] != DBNull.Value ? (int?)Convert.ToInt32(reader2["ReassignStatusId"]) : null,
                //            ReassignStatus = reader2["ReassignStatus"].ToString(),
                //            CreatedBy = reader2["CreatedBy"].ToString(),
                //            CreatedDate = reader2["CreatedDate"] != DBNull.Value ? (DateTime?)reader2["CreatedDate"] : null,
                //            UpdatedBy = reader2["UpdatedBy"].ToString(),
                //            UpdatedDate = reader2["UpdatedDate"] != DBNull.Value? (DateTime?)reader2["UpdatedDate"] : null

                //        };
                //        result.Add(item);
                //    }
                //}

                //LogMessage(LogFilePath, "insert datatable to history:");
                //var historyTable = CreateWorkListHistoryDataTable(result);
                //if (historyTable.Rows.Count > 0)
                //{
                //    using var historyCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, (SqlTransaction)tran.GetDbTransaction());
                //    historyCopy.DestinationTableName = "WorkListHistory";
                //    await historyCopy.WriteToServerAsync(historyTable);
                //}

                //LogMessage(LogFilePath, "ลบ temp table");
                //// ลบ temp table
                //using (var cmd = new SqlCommand("DROP TABLE #TempContract", conn, (SqlTransaction)tran.GetDbTransaction()))
                //{
                //    await cmd.ExecuteNonQueryAsync();
                //}

                // Drop temp table---
                string dropTempTable = @"
                IF OBJECT_ID('tempdb..#TempWorkListUpdate') IS NOT NULL
                    DROP TABLE #TempWorkListUpdate;";
                using var deleteTempTableCmd = new SqlCommand(dropTempTable, conn, (SqlTransaction)tran.GetDbTransaction());
                await deleteTempTableCmd.ExecuteNonQueryAsync();

                // Commit transaction---
                await tran.CommitAsync();
                //LogMessage(LogFilePath, "Update WorkList Success");
            }
            catch (Exception ex)
            {
                LogMessage(LogFilePath, "Error process update WorkList: " + ex.Message);
                await tran.RollbackAsync();
                return "Error process update WorkList: " + ex.Message;
            }

            return processResult;
        }

        private DataTable CreateUpdateWorkListTempTable(List<AllocateWorklistListUpdateWorkListDto> items)
        {
            var table = new DataTable();
            table.Columns.Add("ContractNo", typeof(string));
            table.Columns.Add("AssignDate", typeof(DateTime));
            table.Columns.Add("JobTypeCode", typeof(string));
            table.Columns.Add("JobTypeDesc", typeof(string));
            table.Columns.Add("AssignTool", typeof(string));
            table.Columns.Add("AssignAreaId", typeof(Guid));
            table.Columns.Add("AssignAreaCode", typeof(string));
            table.Columns.Add("AssignTeamId", typeof(Guid));
            table.Columns.Add("AssignTeamCode", typeof(string));
            table.Columns.Add("AssignTeamName", typeof(string));
            table.Columns.Add("AssignCollectorId", typeof(Guid));
            table.Columns.Add("AssignCollectorEmpId", typeof(string));
            table.Columns.Add("AssignCollectorName", typeof(string));
            table.Columns.Add("AssignTypeCode", typeof(string));
            table.Columns.Add("AssignTypeDesc", typeof(string));
            table.Columns.Add("FollowupStatusCode", typeof(string));
            table.Columns.Add("FollowupStatusDesc", typeof(string));
            table.Columns.Add("FollowupDate", typeof(DateTime));
            table.Columns.Add("UpdatedDate", typeof(DateTime));

            foreach (var item in items)
            {
                table.Rows.Add(
                    string.IsNullOrWhiteSpace(item.ContractNo) ? DBNull.Value : (object)item.ContractNo,
                    item.AssignDate.HasValue ? (object)item.AssignDate.Value : DBNull.Value,
                    string.IsNullOrWhiteSpace(item.JobTypeCode) ? DBNull.Value : (object)item.JobTypeCode,
                    string.IsNullOrWhiteSpace(item.JobTypeDesc) ? DBNull.Value : (object)item.JobTypeDesc,
                    string.IsNullOrWhiteSpace(item.AssignTool) ? DBNull.Value : (object)item.AssignTool,
                    //string.IsNullOrWhiteSpace(item.AssignAreaId) ? DBNull.Value : (object)item.AssignAreaId,
                    item.AssignAreaId.HasValue ? (object)item.AssignAreaId.Value : DBNull.Value,
                    string.IsNullOrWhiteSpace(item.AssignAreaCode) ? DBNull.Value : (object)item.AssignAreaCode,
                    //string.IsNullOrWhiteSpace(item.AssignTeamId) ? DBNull.Value : (object)item.AssignTeamId,
                    item.AssignTeamId.HasValue ? (object)item.AssignTeamId.Value : DBNull.Value,
                    string.IsNullOrWhiteSpace(item.AssignTeamCode) ? DBNull.Value : (object)item.AssignTeamCode,
                    string.IsNullOrWhiteSpace(item.AssignTeamName) ? DBNull.Value : (object)item.AssignTeamName,
                    //string.IsNullOrWhiteSpace(item.AssignCollectorId) ? DBNull.Value : (object)item.AssignCollectorId,
                    item.AssignCollectorId.HasValue ? (object)item.AssignCollectorId.Value : DBNull.Value,
                    string.IsNullOrWhiteSpace(item.AssignCollectorEmpId) ? DBNull.Value : (object)item.AssignCollectorEmpId,
                    string.IsNullOrWhiteSpace(item.AssignCollectorName) ? DBNull.Value : (object)item.AssignCollectorName,
                    string.IsNullOrWhiteSpace(item.AssignTypeCode) ? DBNull.Value : (object)item.AssignTypeCode,
                    string.IsNullOrWhiteSpace(item.AssignTypeDesc) ? DBNull.Value : (object)item.AssignTypeDesc,
                    string.IsNullOrWhiteSpace(item.FollowupStatusCode) ? DBNull.Value : (object)item.FollowupStatusCode,
                    string.IsNullOrWhiteSpace(item.FollowupStatusDesc) ? DBNull.Value : (object)item.FollowupStatusDesc,
                    item.AssignDate.HasValue ? (object)item.AssignDate.Value : DBNull.Value,
                    item.AssignDate.HasValue ? (object)item.AssignDate.Value : DBNull.Value
                );
            }

            return table;
        }

        private DataTable CreateWorkListDataTable(List<AllocateWorklistListUpdateWorkListDto> items)
        {
            var table = new DataTable();
            table.Columns.Add("WorklistId", typeof(Guid)); 
            table.Columns.Add("AssignDate", typeof(DateTime));
            table.Columns.Add("ContractNo", typeof(string)); 
            table.Columns.Add("JobTypeCode", typeof(string)); 
            table.Columns.Add("JobTypeDesc", typeof(string));
            table.Columns.Add("RiskLevel", typeof(string));
            table.Columns.Add("AssignTool", typeof(string));
            table.Columns.Add("AssignAreaId", typeof(Guid));
            table.Columns.Add("AssignAreaCode", typeof(string));
            table.Columns.Add("AssignTeamId", typeof(Guid));                      
            table.Columns.Add("AssignTeamCode", typeof(string)); 
            table.Columns.Add("AssignTeamName", typeof(string));
            table.Columns.Add("AssignCollectorId", typeof(Guid)); 
            table.Columns.Add("AssignCollectorEmpId", typeof(string)); 
            table.Columns.Add("AssignCollectorName", typeof(string));         
            table.Columns.Add("AssignTypeCode", typeof(string)); 
            table.Columns.Add("AssignTypeDesc", typeof(string)); 
            table.Columns.Add("ReassignById", typeof(Guid)); 
            table.Columns.Add("ReassignByEmpId", typeof(string)); 
            table.Columns.Add("ReassignByName", typeof(string));
            table.Columns.Add("ReassignRequestDate", typeof(DateTime)); 
            table.Columns.Add("ReassignFromId", typeof(Guid)); 
            table.Columns.Add("ReassignFromEmpId", typeof(string)); 
            table.Columns.Add("ReassignFromName", typeof(string));
            table.Columns.Add("ReassignToId", typeof(Guid)); 
            table.Columns.Add("ReassignToEmpId", typeof(string));
            table.Columns.Add("ReassignToName", typeof(string));
            table.Columns.Add("ReassignToTeamId", typeof(Guid));
            table.Columns.Add("ReassignToTeamCode", typeof(string));
            table.Columns.Add("ReassignToTeamName", typeof(string));
            table.Columns.Add("ReassignReason", typeof(string)); 
            table.Columns.Add("ReassignStatusCode", typeof(string));
            table.Columns.Add("ReassignStatusDesc", typeof(string)); 
            table.Columns.Add("ApprovedById", typeof(Guid));
            table.Columns.Add("ApprovedEmpId", typeof(string)); 
            table.Columns.Add("ApprovedName", typeof(string)); 
            table.Columns.Add("ApproveReason", typeof(string));
            table.Columns.Add("ApproveDate", typeof(DateTime)); 
            table.Columns.Add("FollowupStatusCode", typeof(string));
            table.Columns.Add("FollowupStatusDesc", typeof(string));
            table.Columns.Add("FollowupDate", typeof(DateTime));
            table.Columns.Add("DoNotCallFlag", typeof(string));
            table.Columns.Add("CreatedBy", typeof(string));
            table.Columns.Add("CreatedDate", typeof(DateTime));
            table.Columns.Add("UpdatedBy", typeof(string)); 
            table.Columns.Add("UpdatedDate", typeof(DateTime)); 

            foreach (var item in items)
            {
                table.Rows.Add(
                    Guid.NewGuid(),
                    item.AssignDate,//DateTime.Now,
                    item.ContractNo,
                    item.JobTypeCode,
                    item.JobTypeDesc,
                    null,
                    item.AssignTool,
                    item.AssignAreaId.HasValue ? (object)item.AssignAreaId.Value : DBNull.Value,
                    item.AssignAreaCode,
                    item.AssignTeamId.HasValue ? (object)item.AssignTeamId.Value : DBNull.Value,
                    item.AssignTeamCode,
                    item.AssignTeamName,
                    item.AssignCollectorId.HasValue ? (object)item.AssignCollectorId.Value : DBNull.Value,
                    item.AssignCollectorEmpId,
                    item.AssignCollectorName,
                    item.AssignTypeCode,
                    item.AssignTypeDesc,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    item.FollowupStatusCode,
                    item.FollowupStatusDesc,
                    item.AssignDate,//DateTime.Now,
                    null,
                    null,
                    item.AssignDate,//DateTime.Now,
                    null,
                    item.AssignDate//DateTime.Now,                      
                );
            }

            return table;
        }

        private DataTable CreateWorkListHistoryDataTable(List<WorklistHistoryDto> items)
        {
            var table = new DataTable();
            //table.Columns.Add("Id", typeof(string));
            //table.Columns.Add("WorklistId", typeof(string));
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("WorklistId", typeof(Guid));
            table.Columns.Add("AssignDate", typeof(DateTime));
            table.Columns.Add("ContractNo", typeof(string));
            table.Columns.Add("JobTypeId", typeof(string));
            table.Columns.Add("JobType", typeof(string));
            table.Columns.Add("RiskLevel", typeof(string));
            table.Columns.Add("AssignTool", typeof(string));
            table.Columns.Add("AssignAreaId", typeof(string));
            table.Columns.Add("AssignAreaCode", typeof(string));
            table.Columns.Add("AssignTeamId", typeof(string));
            table.Columns.Add("AssignTeamCode", typeof(string));
            table.Columns.Add("AssignCollectorId", typeof(string));
            table.Columns.Add("SupervisorId", typeof(string));
            table.Columns.Add("AssignTypeId", typeof(string));
            table.Columns.Add("AssignTypeCode", typeof(string));
            table.Columns.Add("ReassignFrom", typeof(string));
            table.Columns.Add("ReassignTo", typeof(string));
            table.Columns.Add("ReassignReason", typeof(string));
            table.Columns.Add("ReassignStatusId", typeof(string));
            table.Columns.Add("ReassignStatus", typeof(string));
            table.Columns.Add("CreatedBy", typeof(string));
            table.Columns.Add("CreatedDate", typeof(DateTime));
            table.Columns.Add("UpdatedBy", typeof(string));
            table.Columns.Add("UpdatedDate", typeof(DateTime));

            foreach (var item in items)
            {
                table.Rows.Add(
                    Guid.NewGuid().ToString(),
                    item.WorklistId,
                    item.AssignDate ?? (object)DBNull.Value,
                    item.ContractNo,
                    item.JobTypeId ?? (object)DBNull.Value,
                    item.JobType,
                    item.RiskLevel,
                    item.AssignTool,
                    item.AssignAreaId ?? (object)DBNull.Value,
                    item.AssignAreaCode,
                    item.AssignTeamId ?? (object)DBNull.Value,
                    item.AssignTeamCode,
                    item.AssignCollectorId ?? (object)DBNull.Value,
                    item.SupervisorId ?? (object)DBNull.Value,
                    item.AssignTypeId ?? (object)DBNull.Value,
                    item.AssignTypeCode,
                    item.ReassignFrom ?? (object)DBNull.Value,
                    item.ReassignTo ?? (object)DBNull.Value,
                    item.ReassignReason ?? (object)DBNull.Value,
                    item.ReassignStatusId ?? (object)DBNull.Value,
                    item.ReassignStatus ?? (object)DBNull.Value,
                    item.CreatedBy ?? (object)DBNull.Value,
                    item.CreatedDate ?? (object)DBNull.Value,
                    item.UpdatedBy ?? (object)DBNull.Value,
                    item.UpdatedDate ?? (object)DBNull.Value
                );
            }

            return table;
        }

        private List<DataItem> initialMemberCheckFullCapacity(int memberAmount)
        {
            List<DataItem> items = Enumerable.Range(0, memberAmount)
                .Select(i => new DataItem { Index = i, Value = 0 })
                .ToList();

            return items;
        }

        string DebugSql(string sql, SqlParameter[] parameters)
        {
            foreach (var param in parameters)
            {
                string valueStr;

                if (param.Value == DBNull.Value || param.Value == null)
                {
                    valueStr = "NULL";
                }
                else if (param.Value is string || param.Value is DateTime)
                {
                    valueStr = $"'{param.Value.ToString().Replace("'", "''")}'"; // escape '
                }
                else
                {
                    valueStr = param.Value.ToString();
                }

                sql = sql.Replace(param.ParameterName, valueStr);
            }

            return sql;
        }

        private string MoveFileToArchive(string archivePath, string csvPath, string fileName)
        {
            LogMessage(LogFilePath, "-----Archive file : "+ fileName + "-----");
            try
            {
                // Original file full path
                string sourcePath = Path.Combine(csvPath, fileName);

                if (!File.Exists(sourcePath))
                    throw new FileNotFoundException("File not found.", sourcePath);                

                // Create archive directory if not exists
                if (!Directory.Exists(archivePath))
                    Directory.CreateDirectory(archivePath);

                // New filename with timestamp
                string newFileName =
                DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture) + "_" +
                Path.GetFileNameWithoutExtension(fileName) +
                Path.GetExtension(fileName);

                string destinationPath = Path.Combine(archivePath, newFileName);

                // Move file
                File.Move(sourcePath, destinationPath);

                return destinationPath; // return new archived path
            }
            catch
            {
                throw; // let caller handle/log
            }
        }

        private async Task<bool> ValidateData()
        {
            LogMessage(LogFilePath, "-----Validate process----- ");
            bool validateResult = true;
            //await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            var conn = _unitOfWork.DbContext.Database.GetDbConnection();
            await _unitOfWork.DbContext.Database.OpenConnectionAsync();

            try
            {
                DateTime now = DateTime.Now;
                string validateDataSql = @"select a.ContractNo from AssignmentWorklist a
                        where a.ContractNo not in (select ContractNo from Contract)
                        or a.AllocateMode not in (select EnumCode from SysEnum where EnumName = 'AllocateMode')
                        or a.AssignType not in (select EnumCode from SysEnum where EnumName = 'AssignType')
                        or (a.AssignFlag not in (select EnumCode from SysEnum where EnumName = 'AssignFlag') or a.AssignFlag is null)
                        or (a.AssignMethod is not null and RTRIM(LTRIM(a.AssignMethod)) <>'' and a.AssignMethod not in (select EnumCode from SysEnum where EnumName = 'AssignMethod'))
                        or (a.AssignMethodSortField is not null and RTRIM(LTRIM(a.AssignMethodSortField)) <>'' and 'found' not in (SELECT 'found' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Contract' AND COLUMN_NAME = a.AssignMethodSortField))
                        or (a.AssignMethodSortOrder is not null and RTRIM(LTRIM(a.AssignMethodSortOrder)) <>''  and LOWER(a.AssignMethodSortOrder) not in (LOWER('asc'),LOWER('desc')))
                        or (a.AssignTool not in (select EnumCode from SysEnum where EnumName = 'AssignTool'))
                        or (a.AssignTeam not in (select ColTeamName from ColTeam))
                        or (a.AssignOverCapacity is not null and RTRIM(LTRIM(a.AssignOverCapacity)) <>'' and a.AssignOverCapacity not in (select EnumCode from SysEnum where EnumName = 'AssignOverCapacity'))
                        or (a.JobType is not null and RTRIM(LTRIM(a.JobType)) <>'' and a.JobType not in (select EnumCode from SysEnum where EnumName = 'JobType'))";
                
                var followupStatusData = await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, validateDataSql);

                if (followupStatusData.Count > 0) //found some invalid data, we will update isAssigned=1 of collect data---
                {
                    validateResult = false;

                    string updateIsAssignedFlagSql = @"update AssignmentWorklist set isAssigned = 1
                                            where ContractNo not in ("+ validateDataSql+")";
                    var updateIsAssignedFlagResult = await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, updateIsAssignedFlagSql);                    
                }
                else
                {
                    string updateIsAssignedFlagSql = @"update AssignmentWorklist set isAssigned = 1";
                    var updateIsAssignedFlagResult = await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, updateIsAssignedFlagSql);

                }                
                return validateResult;                
            }
            catch (Exception ex)
            {
                LogMessage(LogFilePath, "Error Process data validation!: " + ex.Message);
                return false;
            }
        }

        private async Task WriteSummaryAssignmentResultLog()
        {
            LogMessage(LogFilePath, "-----Summary Assignment Result-----");
            
            //await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            var conn = _unitOfWork.DbContext.Database.GetDbConnection();
            await _unitOfWork.DbContext.Database.OpenConnectionAsync();

            try
            {
                    string queryDataCountSql = @"SELECT
                                            COUNT(*) AS TotalRows,
                                            SUM(CASE WHEN a.IsAssigned = 1 THEN 1 ELSE 0 END) AS ValidRows,
                                            SUM(CASE WHEN a.IsAssigned <> 1 OR a.IsAssigned IS NULL THEN 1 ELSE 0 END) AS InValidRows,	
                                            SUM(CASE WHEN a.IsAssigned = 1 AND a.AssignFlag = 'Y' THEN 1 ELSE 0 END) AS ValidAssignRows,
                                            SUM(CASE WHEN a.IsAssigned = 1 AND a.AssignFlag = 'N' THEN 1 ELSE 0 END) AS ValidNotAssignRows,
                                            SUM(CASE WHEN a.IsAssigned = 1 AND a.AssignFlag = 'X' THEN 1 ELSE 0 END) AS ValidExcludeRows,	
                                            ISNULL((
                                            SELECT STUFF((
                                                SELECT ',' + aw.ContractNo
                                                FROM AssignmentWorklist aw
                                                WHERE aw.IsAssigned IS NULL
                                                   OR aw.IsAssigned <> 1
                                                FOR XML PATH(''), TYPE
                                            ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')
                                        ), '') AS ContractNoList
                                        FROM AssignmentWorklist a;";
                var countData = await DbContextExtensions.ExecuteQueryAsync(_unitOfWork.DbContext, queryDataCountSql);
                string totalDataInAssignWorkList = "";
                string totalDataValidInAssignWorkList = "";
                string totalDataInValidInAssignWorkList = "";
                string totalDataValidForAssign = "";
                string totalDataValidForNotAssign = "";
                string totalDataValidForExclude = "";
                string listOfContract = "";

                if (countData != null && countData.Count > 0)
                {
                    totalDataInAssignWorkList = countData[0]["TotalRows"].ToString();
                    totalDataValidInAssignWorkList = countData[0]["ValidRows"].ToString();
                    totalDataInValidInAssignWorkList = countData[0]["InValidRows"].ToString();
                    totalDataValidForAssign = countData[0]["ValidAssignRows"].ToString();
                    totalDataValidForNotAssign = countData[0]["ValidNotAssignRows"].ToString();
                    totalDataValidForExclude = countData[0]["ValidExcludeRows"].ToString();
                    listOfContract = countData[0]["ContractNoList"].ToString();
                }

                LogMessage(LogFilePath, "Amount of data in CSV file(s) : " + TotalCSVData + " row"); 
                LogMessage(LogFilePath, "Amount of data in WorklistAssignment,Worklist : " + totalDataInAssignWorkList + " row"); 
                LogMessage(LogFilePath, "Amount of valid data : " + totalDataValidInAssignWorkList + " row (Assign = "+ totalDataValidForAssign + ", Not assign = "+ totalDataValidForNotAssign + ", Exclude = "+ totalDataValidForExclude + ")"); 
                LogMessage(LogFilePath, "Amount of invalid data: " + totalDataInValidInAssignWorkList + " row (ContractNo = "+ listOfContract + " )");                

            }
            catch (Exception ex)
            {
                LogMessage(LogFilePath, "Error Process data validation!: " + ex.Message);                
            }           
        }
    }

    public static class DbContextExtensions
    {
        public static async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(
            this DbContext context,
            string sql,
            Dictionary<string, object> parameters = null)
        {
            var result = new List<Dictionary<string, object>>();
            var conn = context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = sql;
            command.Transaction = context.Database.CurrentTransaction?.GetDbTransaction();

            // Add parameters if any
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    var dbParam = command.CreateParameter();
                    dbParam.ParameterName = param.Key;
                    dbParam.Value = param.Value ?? DBNull.Value;
                    command.Parameters.Add(dbParam);
                }
            }

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var colName = reader.GetName(i);
                    var val = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                    row[colName] = val;
                }
                result.Add(row);
            }

            return result;
        }
    }
}
