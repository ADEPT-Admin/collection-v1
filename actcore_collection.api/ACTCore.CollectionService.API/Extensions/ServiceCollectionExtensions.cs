using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Application.Validations;
using ACTCore.CollectionService.Infrastructure.Interface;
using ACTCore.CollectionService.Infrastructure.Repository;
using ACTCore.CollectionService.Infrastructure.UnitOfWork;
using SharedKernel.Data.Interface;

namespace ACTCore.CollectionService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseGenericRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IActivityLogService, ActivityLogService>();
            services.AddScoped<IApiLogService, ApiLogService>();
            services.AddScoped<IAuthLogService, AuthLogService>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IProcessExecutionLogService, ProcessExecutionLogService>();

            // system and Security services
            services.AddScoped<AuthService>();
            services.AddScoped<UserService>();
            services.AddScoped<UserGroupService>();
            services.AddScoped<SysItemService>();
            services.AddScoped<ParameterService>();
            services.AddScoped<EnumService>();
            services.AddScoped<PasswordHistoryService>();
            services.AddScoped<PolicyService>();
            services.AddScoped<ActiveSessionService>();
            services.AddScoped<ItemAccessRightService>();


            // master data services
            services.AddScoped<EmployeeService>();
            services.AddScoped<TeamService>();
            services.AddScoped<ProvinceService>();
            services.AddScoped<DistrictService>();
            services.AddScoped<SubDistrictService>();

            // collection service
            services.AddScoped<WorklistService>();
            services.AddScoped<WorklistHistoryService>();
            services.AddScoped<CollectorService>();
            services.AddScoped<CollectionNoteService>();
            services.AddScoped<ColNoteActionService>();
            services.AddScoped<ColNoteResultService>();
            services.AddScoped<ColTeamService>();
            services.AddScoped<ColRoleService>();
            //services.AddScoped<ColAreaService>();
            services.AddScoped<ContractService>();
            services.AddScoped<TeamAssignmentService>();
            services.AddScoped<TeamAssignmentValidationService>();

            // Language service
            services.AddScoped<LanguageService>();

            // Mapping Import service
            services.AddScoped<MappingImportService>();

            return services;
        }
    }
}
