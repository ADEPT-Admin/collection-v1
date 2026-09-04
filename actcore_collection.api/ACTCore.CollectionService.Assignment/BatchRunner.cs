using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Assignment;
using CsvHelper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace ACTCore.CollectionService.Assignment
{
    public class BatchRunner
    {
        //private readonly IAssignmentService _assignmentService;

        //public BatchRunner(IAssignmentService assignmentService)
        //{
        //    _assignmentService = assignmentService;
        //}

        public async Task ExecuteAsync()
        {
            // 1. Get all CSV files than name match with output file name config from the configured folder---            
            string[] csvFiles = Directory.GetFiles(GlobalState.Instance._csvFolderPath, GlobalState.Instance._adeptOutputFileName+"*.csv");

            // 2. Check if any files were found
            if (csvFiles.Length == 0)
            {
                Utils.LogMessage($"No CSV files found in the specified folder: {GlobalState.Instance._csvFolderPath}.");
                Console.WriteLine("No CSV files found. Check the log file for details.");
                return;
            }

            Utils.LogMessage($"Found {csvFiles.Length} CSV file(s) to process.");

            // 3. Process each file
            foreach (var filePath in csvFiles)
            {
                await ProcessCsvFile(filePath);
            }

            // เรียก AssignmentService-----------------------------
            //await _assignmentService.RunAssignmentJobAsync();                       
        }
        

        /// <summary>
        /// Reads a single CSV file and inserts its data into the database.
        /// </summary>
        /// <param name="filePath">The full path to the CSV file.</param>
        private static async Task ProcessCsvFile(string filePath)
        {
            Utils.LogMessage($"Processing file: {Path.GetFileName(filePath)}");

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
                    while (await csv.ReadAsync())
                    {
                        rowsInserted++;
                        sql = "INSERT INTO dbo.MyData (" + csv.GetField<string>(0) + ", " + csv.GetField<string>(1) + " ," + csv.GetField<string>(2) + " )";
                        Utils.LogMessage($"Successfully inserted {rowsInserted + "." + sql} rows from {Path.GetFileName(filePath)}.");
                    }
                }

                //using (var connection = new SqlConnection(connectionString))
                //{
                //    await connection.OpenAsync();

                //    // Read header row to get column names
                //    await csv.ReadAsync();
                //    csv.ReadHeader();

                //    int rowsInserted = 0;
                //    while (await csv.ReadAsync())
                //    {
                //        // Create an SQL command for insertion
                //        string sql = "INSERT INTO dbo.MyData (Column1, Column2, Column3) VALUES (@value1, @value2, @value3)";
                //        using (var command = new SqlCommand(sql, connection))
                //        {
                //            // Assuming your CSV has 3 columns:
                //            command.Parameters.AddWithValue("@value1", csv.GetField<string>(0));
                //            command.Parameters.AddWithValue("@value2", csv.GetField<int>(1));
                //            command.Parameters.AddWithValue("@value3", csv.GetField<decimal>(2));

                //            await command.ExecuteNonQueryAsync();
                //            rowsInserted++;
                //        }
                //    }
                //    LogMessage($"Successfully inserted {rowsInserted} rows from {Path.GetFileName(filePath)}.");
                //}
            }
            catch (Exception ex)
            {
                Utils.LogMessage($"Error processing file {Path.GetFileName(filePath)}: {ex.Message}");
            }
        }

        static async Task  test(string[] args)
        {
            string connectionString = "your_connection_string_here";

            using (var db = new DbHelper(connectionString))
            {
                try
                {
                    db.BeginTransaction();

                    string insertSql = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
                    db.ExecuteNonQuery(insertSql,
                        new SqlParameter("@Name", "chai"),
                        new SqlParameter("@Age", 30)
                    );

                    string countSql = "SELECT COUNT(*) FROM Users";
                    var count = db.ExecuteScalar(countSql);
                    Console.WriteLine($"User count: {count}");

                    db.Commit();
                }
                catch (Exception ex)
                {
                    db.Rollback();
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
