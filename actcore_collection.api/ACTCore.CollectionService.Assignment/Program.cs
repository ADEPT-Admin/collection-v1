//using System.Data.SqlClient;
using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Assignment;
using ACTCore.CollectionService.Infrastructure;
using ACTCore.CollectionService.Infrastructure.Interface;
using ACTCore.CollectionService.Infrastructure.Repository;
using ACTCore.CollectionService.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Data.Interface;

// This class holds configuration settings read from appsettings.json
public class AppSettings
{
    public string? AdeptProjectName { get; set; }
    public string? AdeptInputFileName { get; set; }
    public string? CsvInputFolderPath { get; set; }
    public string? CsvFolderPath { get; set; }
    public string? ArchiveFolderPath { get; set; }
    public string? LogFilePath { get; set; }
    public string? AdeptOutputFileName { get; set; }
    public string? LogFileName { get; set; }
    public string? AdeptURL { get; set; }

}

public class Program
{
    private static IConfigurationRoot? _configuration;
    private static AppSettings? _appSettings;

    public static async Task Main(string[] args)
    {
        //Build configuration from appsettings.json ---
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        _configuration = builder.Build();
        _appSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();

        // Ensure configuration is valid before proceeding
        if (_appSettings == null || string.IsNullOrEmpty(_appSettings.AdeptProjectName) || string.IsNullOrEmpty(_appSettings.AdeptInputFileName) || string.IsNullOrEmpty(_appSettings.CsvInputFolderPath) || string.IsNullOrEmpty(_appSettings.AdeptOutputFileName) || string.IsNullOrEmpty(_appSettings.CsvFolderPath) || string.IsNullOrEmpty(_appSettings.ArchiveFolderPath) || string.IsNullOrEmpty(_appSettings.AdeptURL) || string.IsNullOrEmpty(_appSettings.LogFilePath) || string.IsNullOrEmpty(_appSettings.LogFileName))
        {
            Console.WriteLine("Error: AppSettings or its properties are not configured correctly in appsettings.json.");
            Utils.LogMessage("Error: AppSettings or its properties are not configured correctly in appsettings.json.");
            return;
        }

        //assign golbal variable ---
        GlobalState.Instance._logFilePath = _appSettings.LogFilePath + _appSettings.LogFileName + "_" + Utils.GetCurrentFormattedDateTime()+".txt"  ;
        GlobalState.Instance._adeptOutputFileName = _appSettings.AdeptOutputFileName;
        GlobalState.Instance._csvFolderPath = _appSettings.CsvFolderPath;
        GlobalState.Instance._archiveFolderPath = _appSettings.ArchiveFolderPath;

        Utils.LogMessage("-----Batch application started.-----");
        //Console.WriteLine("Batch application started.");

        try
        {
            using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Add DbContext
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultConnection")));

                // Register Service---
                services.AddAutoMapper(typeof(MappingConfig));
                services.AddScoped(typeof(IBaseGenericRepository<>), typeof(Repository<>));
                services.AddScoped<IUnitOfWork, UnitOfWork>();

                //services.AddScoped<IAssignmentService, AssigmentService>();
                services.AddScoped<AssignmentService>();

                // Register Batch Runner---
                services.AddScoped<BatchRunner>();

            })
            .Build();

            // Run Batch
            using var scope = host.Services.CreateScope();
            //var runner = scope.ServiceProvider.GetRequiredService<BatchRunner>();
            //await runner.ExecuteAsync();

            var assignmentSv = scope.ServiceProvider.GetRequiredService<AssignmentService>();            
            await assignmentSv.RunAssignmentJobAsync(GlobalState.Instance._csvFolderPath, GlobalState.Instance._adeptOutputFileName, GlobalState.Instance._logFilePath, GlobalState.Instance._archiveFolderPath);

        }
        catch (Exception ex)
        {
            Utils.LogMessage($"An unexpected error occurred: {ex.Message}");
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }        
    }

}
