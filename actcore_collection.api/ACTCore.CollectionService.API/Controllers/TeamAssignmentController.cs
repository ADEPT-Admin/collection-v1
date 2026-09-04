using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Extensions;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Models.Dto.ListPagedDto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Dto.Validation;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Application.Validations;
using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Helpers;
using SharedKernel.Models;
using System.Linq.Expressions;
using System.Net;
using System.Text.Json;

namespace ACTCore.CollectionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TeamAssignmentController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly IActivityLogService _activityLog;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TeamAssignmentService _teamAssignmentService;
        private readonly LanguageService _languageService;
        private readonly CollectorService _collectorService;
        private readonly ColTeamService _colTeamService;
        private readonly TeamAssignmentValidationService TeamAssignmentValidation;

        public TeamAssignmentController(IMapper mapper, IErrorLogService errorLog, IActivityLogService activityLog,
            TeamAssignmentService teamAssignmentService, IUnitOfWork unitOfWork,
            LanguageService languageService, CollectorService collectorService, ColTeamService colTeamService,
            TeamAssignmentValidationService teamAssignmentValidation) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _activityLog = activityLog;
            _unitOfWork = unitOfWork;
            _teamAssignmentService = teamAssignmentService;
            _colTeamService = colTeamService;
            _collectorService = collectorService;
            _languageService = languageService;
            TeamAssignmentValidation = teamAssignmentValidation;
        }

        [HttpPost("list-paged")]
        public async Task<ActionResult<APIResponse>> GetListCollectorPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var filter = BuildListPagedFilter(request);
                if (!string.IsNullOrEmpty(request.SortColumn))
                {
                    if (request.SortColumn == nameof(TeamAssignmentListPagedResponseDto.ColTeamName)) request.SortColumn = "ColTeam.ColTeamName";
                    if (request.SortColumn == nameof(TeamAssignmentListPagedResponseDto.CollectorEmpId)) request.SortColumn = "Collector.User.Employee.EmployeeId";
                    if (request.SortColumn == nameof(TeamAssignmentListPagedResponseDto.CollectorEmpName)) request.SortColumn = "Collector.User.Employee.FullName";
                }

                var teamAsignments = await _teamAssignmentService.GetListPaginationAsync(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "ColTeam.ColTeamName",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters,
                    includeProperties: "ColTeam,Collector,Collector.User,Collector.User.Employee,Collector.User.Employee.Prefix"
                );

                List<TeamAssignmentListPagedResponseDto> teamAssignmentList = new List<TeamAssignmentListPagedResponseDto>();
                foreach (var item in teamAsignments.Items)
                {
                    var teamAssignment = new TeamAssignmentListPagedResponseDto{
                    AssignmentId = item.AssignmentId,
                    ColTeamName = item.ColTeam != null ? item.ColTeam.ColTeamName : "",
                    CollectorEmpId = item.Collector?.User?.Employee != null ? item.Collector.User.Employee.EmployeeId : "",
                    CollectorEmpName = item.Collector?.User?.Employee != null ? item.Collector.User.Employee.FullName : "",
                    IsSupervisor = item.IsSupervisor ? IconType.CHECKGREEN : "",
                    Capacity = item.Capacity,
                    EffectiveDate = item.EffectiveDate,
                    ExpireDate = item.ExpireDate,
                    IsActive = item.IsActive ? IconType.CHECKGREEN : IconType.CHECKRED
                    };
                    teamAssignmentList.Add(teamAssignment);
                }

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                var displayProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<TeamAssignmentDisplayFilterDto>(enumValues);
                response = new ApiPaginationResponse
                {
                    Data = new PaginationData<TeamAssignmentListPagedResponseDto>
                    {
                        Datatables = teamAssignmentList,
                        DisplayColumns = displayProperties,
                        TotalRecords = teamAsignments.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)teamAsignments.TotalCount / request.PageSize),
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize
                    },
                    StatusCode = HttpStatusCode.OK,
                    Status = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, "", ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpPost("list-paged-assign-by-team")]
        public async Task<ActionResult<APIResponse>> GetListCollectorAssignByTeamPaged([FromQuery] Guid ColTeamId,[FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var filter = BuildExpressionFilter(request, ColTeamId);
                if (!string.IsNullOrEmpty(request.SortColumn))
                {
                    if (String.Equals(request.SortColumn, "CollectorId", StringComparison.OrdinalIgnoreCase)) request.SortColumn = "Collector.User.Employee.EmployeeId";
                    if (String.Equals(request.SortColumn, "Supervisor", StringComparison.OrdinalIgnoreCase)) request.SortColumn = "IsSupervisor";
                    if (String.Equals(request.SortColumn, "CollectorName", StringComparison.OrdinalIgnoreCase)) request.SortColumn = "Collector.User.Employee.FullName";
                    if (String.Equals(request.SortColumn, "ColRoleName", StringComparison.OrdinalIgnoreCase)) request.SortColumn = "Collector.ColRole.ColRoleName";
                    if (String.Equals(request.SortColumn, "CollectorCapacity", StringComparison.OrdinalIgnoreCase)) request.SortColumn = "Capacity";
                }

                var assignCollectors = await _teamAssignmentService.GetListPaginationAsync(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "Collector.User.Employee.FullName",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters,
                    includeProperties: "Collector.ColRole,Collector.User.Employee.Prefix,Collector.User.Employee.Supervisor,Collector.User.Employee.Supervisor.Prefix"
                );

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                
                response = new ApiPaginationResponse
                {
                    Data = PaginationDataHelper.BuildPaginationData<CollectorAssignByTeamDisplayFilterDto, ColTeamListAssignByTeamPagedResponseDto, ColTeamAssignment>
                        (assignCollectors, request.PageNumber, request.PageSize, enumValues, _mapper),
                    StatusCode = HttpStatusCode.OK,
                    Status = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, "", ControllerContext, CurrentUserInfo, response));
            }
        }

        [HttpPost("list-paged-by-collector")]
        public async Task<ActionResult<APIResponse>> GetListByCollectorPaged([FromQuery] string id,[FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var filter = BuildExpressionFilter(request, collectorId: id);
                //change to switch case for better readability and maintainability
                request.SortColumn = request.SortColumn switch
                {
                    "Supervisor" => "IsSupervisor",
                    "CollectorCapacity" => "Capacity",
                    "ColTeamName" => "ColTeam.ColTeamName",
                    _ => request.SortColumn
                };

                var assignCollectors = await _teamAssignmentService.GetListPaginationAsync(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "ColTeam.ColTeamName",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters,
                    includeProperties: "ColTeam"
                );

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);

                response = new ApiPaginationResponse
                {
                    Data = PaginationDataHelper.BuildPaginationData<TeamAssignmentByCollectorDisplayFilterDto, TeamAssignmentByCollectorListPagedResponseDto, ColTeamAssignment>
                        (assignCollectors, request.PageNumber, request.PageSize, enumValues, _mapper),
                    StatusCode = HttpStatusCode.OK,
                    Status = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, "", ControllerContext, CurrentUserInfo, response));
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetById(Guid id)
        {
            var response = new APIResponse();
            try
            {
                var teamAssignment = await _teamAssignmentService.GetAsync(x => x.AssignmentId == id,
                    includeProperties: "ColTeam,Collector.User.Employee.Prefix,Collector.ColRole");
                if (teamAssignment == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "TeamAssignment");
                    response.Status = false;

                    return Ok(response);
                }

                response.Data = _mapper.Map<TeamAssignmentResponseDto>(teamAssignment);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpGet("get-collector-in-team/{isInTeam}")]
        public async Task<ActionResult<APIResponse>> GetByTeamFlag(int isInTeam)
        {
            var response = new APIResponse();
            try
            {
                var ownTeam = await _teamAssignmentService.GetAsync(x => x.Collector.UserId == CurrentUserInfo.UserId,
                        includeProperties: "Collector");

                if (ownTeam == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "TeamAssignment");
                    response.Status = false;
                    return Ok(response);
                }

                var isInSameTeam = isInTeam == 1;

                Expression<Func<ColTeamAssignment, bool>> filter = GetCollectorInTeamFilter(ownTeam, isInSameTeam);
                if (filter == null)
                {
                    // Select out team => Not allowed
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(
                        _languageService,
                        Message.Msg_NotHavePermissionToAccessCollectorList);
                    response.Status = false;
                    return Ok(response);
                }

                var collectors = await _teamAssignmentService.GetListAsync(
                    filter: filter,
                    includeProperties: "ColTeam,Collector,Collector.User,Collector.User.Employee,Collector.User.Employee.Prefix"
                );

                response.Data = _mapper.Map<IEnumerable<TeamAssignmentFlagResponseDto>>(collectors).DistinctBy(x => x.CollectorId);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> CreateColTeamAssignment([FromBody] TeamAssignmentCreateDto createDto, [FromQuery] bool overwrite = false)
        {
            var response = new APIResponse();
            var entityId = "";
            try
            {
                var validation = await TeamAssignmentValidation.ValidateCreateAsync(
                    _mapper.Map<TeamAssignmentValidationCreateDto>(createDto), overwrite);
                if(!validation.IsValid)
                {
                    response.StatusCode = validation.WarningMessage != null ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                    response.Message = validation.Message;
                    response.WarningMessages = validation.WarningMessage;
                    response.IsRequireConfirmation = validation.WarningMessage != null;
                    response.Status = false;
                    return Ok(response);
                }
                
                // overwrite previous supervisor
                if (createDto.IsSupervisor)
                {
                    // remove previous supervisor
                    var previousSupervisor = await _teamAssignmentService.GetAsync(x => x.ColTeamId == createDto.ColTeamId && x.IsSupervisor);
                    if (previousSupervisor != null)
                    {
                        previousSupervisor.IsSupervisor = false;
                        VersioningModelHelper.SetUpdatedAudit(previousSupervisor, User.Identity.Name);
                        await _teamAssignmentService.UpdateAsync(previousSupervisor);
                    }
                }
                

                var teamAssignment = _mapper.Map<ColTeamAssignment>(createDto);

                VersioningModelHelper.SetCreatedAudit(teamAssignment, User.Identity.Name);
                await _teamAssignmentService.AddAsync(teamAssignment);

                entityId = teamAssignment.AssignmentId.ToString();

                var newTeamAssignment = await _teamAssignmentService.GetAsync(x => x.AssignmentId == teamAssignment.AssignmentId
                , includeProperties: "ColTeam,Collector.User.Employee.Prefix,Collector.ColRole");

                response.Data = _mapper.Map<TeamAssignmentResponseDto>(newTeamAssignment);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_SavedSuccessfully);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogCreateWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.CREATE, entityId, ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActivityCreateLog(createDto)
                    );
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> UpdateTeamAssignment([FromBody] TeamAssignmentUpdateDto updateDto, [FromQuery] bool overwrite)
        {
            var response = new APIResponse();
            ColTeamAssignment updatedTeamAssignment = null;
            ColTeamAssignment beforUpdate = null;
            try
            {
                var validation = await TeamAssignmentValidation.ValidateUpdateAsync(_mapper.Map<TeamAssignmentValidationUpdateDto>(updateDto), overwrite);
                if(!validation.IsValid)
                {
                    response.StatusCode = validation.WarningMessage != null ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                    response.Message = validation.Message;
                    response.WarningMessages = validation.WarningMessage;
                    response.IsRequireConfirmation = validation.WarningMessage != null;
                    response.Status = false;
                    return Ok(response);
                }


                // check supervisor exists in team && updateDto.IsSupervisor
                updatedTeamAssignment = await _teamAssignmentService.GetAsync(x => x.AssignmentId == updateDto.AssignmentId,asNoTracking: false);
                if (!updatedTeamAssignment.IsSupervisor && updateDto.IsSupervisor)
                {
                    
                    // remove previous supervisor
                    var previousSupervisor = await _teamAssignmentService.GetAsync(x => x.ColTeamId == updateDto.ColTeamId && x.IsSupervisor, asNoTracking: false);
                    if (previousSupervisor != null)
                    {
                        previousSupervisor.IsSupervisor = false;
                        VersioningModelHelper.SetUpdatedAudit(previousSupervisor, User.Identity.Name);
                        await _teamAssignmentService.UpdateAsync(previousSupervisor);
                    }
                    
                }

                beforUpdate = Utilities.ModelHelper.CloneModel(updatedTeamAssignment);
                _mapper.Map(updateDto, updatedTeamAssignment);

                VersioningModelHelper.SetUpdatedAudit(updatedTeamAssignment, User.Identity.Name);
                await _teamAssignmentService.UpdateAsync(updatedTeamAssignment);

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_SavedSuccessfully);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogEditWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, updateDto.AssignmentId.ToString(), ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActiviUpdateLog(beforUpdate, updatedTeamAssignment)
                    );
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse>> DeleteTeamAssignment([FromBody] List<TeamAssignmentDeleteDto> requests)
        {
            var response = new APIResponse();
            var tran = await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (requests == null || !requests.Any())
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                var validation= await TeamAssignmentValidation.ValidateDeleteAsync(requests.Select(r => r.AssignmentId).ToList());
                if(!validation.IsValid)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = validation.Message;
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }
                var assignments = await _teamAssignmentService.GetListAsync(x => requests.Select(r => r.AssignmentId)
                    .Contains(x.AssignmentId));

                await _teamAssignmentService.DeleteRangeAsync(assignments);
                await _unitOfWork.CommitTransactionAsync();

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_DeletedSuccessfully);

                await _activityLog.ActivityLogAsync(
                    assignments.Select(x => LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, x.AssignmentId.ToString(),
                    ControllerContext, CurrentUserInfo, response)).ToList()
                );

                return Ok(response);
            }
            catch (Exception ex)
            {

                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add(ex.Message);
                await _activityLog.ActivityLogAsync(
                        LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, string.Empty
                        , ControllerContext, CurrentUserInfo, response));
                await _unitOfWork.RollbackTransactionAsync();
                return await HandleExceptionAsync(response, ex);
            }
        }

        [HttpPost("bulk-assign-team")]
        public async Task<ActionResult<APIResponse>> BulkAssignTeam([FromBody] CollectorAssignTeamCreateDto createDto)
        {
            var response = new APIResponse();
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var logs = new List<ActivityLog>();
                var teamAssignments = new List<ColTeamAssignment>();

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_SavedSuccessfully);
                    
                var validation = await TeamAssignmentValidation.ValidateBulkAssignTeamAsync(_mapper.Map<CollectorAssignTeamValidationCreateDto>(createDto));
                if(!validation.IsValid)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = validation.Message;
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                foreach( var collectorId in createDto.CollectorIds)
                {
                    var colTeamAssignment = new ColTeamAssignment
                    {
                        ColTeamId = createDto.ColTeamId,
                        CollectorId = collectorId,
                        Capacity = createDto.Capacity,
                        EffectiveDate = createDto.EffectiveDate,
                        ExpireDate = createDto.ExpireDate,
                        IsActive = createDto.IsActive
                    };
                    VersioningModelHelper.SetCreatedAudit(colTeamAssignment, User.Identity.Name);

                    var log = LoggerHelper.BuildActivityLog(ActivityLogAction.CREATE, colTeamAssignment.CollectorId.ToString()
                        , ControllerContext, CurrentUserInfo, response);
                    
                    LoggerHelper.SetActivityLogDetail( log, 
                        LoggerHelper.BuildActivityCreateLog(new LogDetailCollectorAssignTeamCreateDto
                        {
                            ColTeamId = createDto.ColTeamId,
                            CollectorId = collectorId,
                            Capacity = createDto.Capacity,
                            EffectiveDate = createDto.EffectiveDate,
                            ExpireDate = createDto.ExpireDate,
                            IsActive = createDto.IsActive
                         }));

                    teamAssignments.Add(colTeamAssignment);
                    logs.Add(log);
                }

                await _teamAssignmentService.AddRangeAsync(teamAssignments);
                await _activityLog.BulkInsertActivityLogCreateWithDetailsAsync(logs);

                await _unitOfWork.CommitTransactionAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                response.Message = null;
                await _unitOfWork.RollbackTransactionAsync();
                await _activityLog.ActivityLogAsync(
                        LoggerHelper.BuildActivityLog(ActivityLogAction.CREATE, string.Empty
                        , ControllerContext, CurrentUserInfo, response));
                return await HandleExceptionAsync(response, ex);
            }
        }

        #region Private Methods
        private Expression<Func<ColTeamAssignment, bool>> GetCollectorInTeamFilter(ColTeamAssignment ownTeam, bool isInSameTeam)
        {
            Expression<Func<ColTeamAssignment, bool>> filter;

            // Case: Not Supervisor
            if (!ownTeam.IsSupervisor)
            {
                if (!isInSameTeam)
                {
                    // Not allowed
                    return null;
                }

                // Same team only + cannot see own self
                filter = x =>
                    x.ColTeamId == ownTeam.ColTeamId
                    && x.Collector.UserId != CurrentUserInfo.UserId;

                return filter;
            }

            // Case: Supervisor
            if (isInSameTeam)
            {
                // Supervisor, same team => can see everyone including self
                filter = x => x.ColTeamId == ownTeam.ColTeamId;
            }
            else
            {
                // Supervisor, out-team =.see only supervisors of other teams, not self
                filter = x =>
                    x.IsSupervisor
                    && x.ColTeamId != ownTeam.ColTeamId
                    && x.Collector.UserId != CurrentUserInfo.UserId;
            }

            return filter;
        }

        private static Expression<Func<ColTeamAssignment, bool>> BuildListPagedFilter(PagedRequest<FilterContainer> request)
        {
            Expression<Func<ColTeamAssignment, bool>> filter = x => true;

            if (request.Filters?.DynamicFilters != null)
            {
                foreach (var dynamicFilter in request.Filters.DynamicFilters)
                {
                    string key = dynamicFilter.Key;
                    string value = dynamicFilter.Value?.returnStringValue();

                    if (value == null || string.IsNullOrWhiteSpace(value)) continue;

                    // Team Name
                    if (String.Equals(key, nameof(TeamAssignmentListPagedResponseDto.ColTeamName), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<ColTeamAssignment, bool>> teamFilter = e =>
                            e != null && e.ColTeam.ColTeamName != null && e.ColTeam.ColTeamName.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, teamFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // CollectorId (EmployeeId)
                    else if (String.Equals(key, nameof(TeamAssignmentListPagedResponseDto.CollectorEmpId), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<ColTeamAssignment, bool>> empFilter = e =>
                            e != null && e.Collector.User.EmployeeId != null && e.Collector.User.EmployeeId.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, empFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // CollectorName (Employee FullName)
                    else if (String.Equals(key, nameof(TeamAssignmentListPagedResponseDto.CollectorEmpName), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<ColTeamAssignment, bool>> empFilter = e =>
                            e != null && e.Collector.User.Employee != null && (
                                string.Join(" ",
                                    new[] {e.Collector.User.Employee.Prefix != null ? e.Collector.User.Employee.Prefix.PrefixName : "",
                                e.Collector.User.Employee.FirstName ?? "",
                                e.Collector.User.Employee.LastName ?? "" }
                                ).Contains(value)
                            );
                        filter = FilterExtensions.AndAlso(filter, empFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                }
            }
            return filter;
        }

        private static Expression<Func<ColTeamAssignment, bool>> BuildExpressionFilter(PagedRequest<FilterContainer> request, Guid ColTeamId = default, string collectorId = null)
        {
            Expression<Func<ColTeamAssignment, bool>> filter = x => true;

            if (ColTeamId != default) filter = x => x.ColTeamId == ColTeamId;
            if (!string.IsNullOrWhiteSpace(collectorId)) filter = x => x.Collector.User.EmployeeId.Contains(collectorId);

            if (request.Filters?.DynamicFilters != null)
            {
                foreach (var dynamicFilter in request.Filters.DynamicFilters)
                {
                    string key = dynamicFilter.Key;
                    string value = dynamicFilter.Value?.returnStringValue();

                    if (value == null || string.IsNullOrWhiteSpace(value)) continue;

                    // CollectorId (EmployeeId)
                    if (String.Equals(key, nameof(ColTeamListAssignByTeamPagedResponseDto.CollectorId), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<ColTeamAssignment, bool>> collectorIdFilter = c =>
                            c.Collector != null && 
                            c.Collector.User != null && 
                            c.Collector.User.EmployeeId != null && 
                            c.Collector.User.EmployeeId.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, collectorIdFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // CollectorName (Employee FullName)
                    else if (String.Equals(key, nameof(ColTeamListAssignByTeamPagedResponseDto.CollectorName), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<ColTeamAssignment, bool>> collectorNameFilter = c =>
                            c.Collector != null && c.Collector.User != null 
                            && c.Collector.User.Employee != null 
                            && (
                                string.Join(" ",
                                    new[] {c.Collector.User.Employee.Prefix != null ? c.Collector.User.Employee.Prefix.PrefixName : String.Empty,
                                c.Collector.User.Employee.FirstName ?? String.Empty,
                                c.Collector.User.Employee.LastName ?? String.Empty }
                                ).Contains(value)
                            );
                        filter = FilterExtensions.AndAlso(filter, collectorNameFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // ColRoleName
                    else if (String.Equals(key, nameof(ColTeamListAssignByTeamPagedResponseDto.ColRoleName), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<ColTeamAssignment, bool>> colRoleNameFilter = c =>
                            c.Collector != null && c.Collector.ColRole != null && c.Collector.ColRole.ColRoleName.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, colRoleNameFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // Supervisor
                    else if (String.Equals(key, nameof(ColTeamListAssignByTeamPagedResponseDto.Supervisor), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<ColTeamAssignment, bool>> supervisorFilter = c => c.IsSupervisor == bool.Parse(value);
                        filter = FilterExtensions.AndAlso(filter, supervisorFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // Assignment Capacity
                    else if (String.Equals(key, nameof(ColTeamListAssignByTeamPagedResponseDto.CollectorCapacity), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        // change key name for request.Filters.DynamicFiltersRaw from "CollectorCapacity" to Capacity
                        if(request.Filters.DynamicFiltersRaw.TryGetValue(key, out JsonElement filterItem))
                        {
                            request.Filters.DynamicFiltersRaw.Remove(key);
                            request.Filters.DynamicFiltersRaw[nameof(ColTeamAssignment.Capacity)] = filterItem;
                        }
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // ColTeamName
                    else if (String.Equals(key, nameof(TeamAssignmentByCollectorListPagedResponseDto.ColTeamName), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<ColTeamAssignment, bool>> teamNameFilter = c =>
                            c.ColTeam != null && c.ColTeam.ColTeamName != null && c.ColTeam.ColTeamName.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, teamNameFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                }
            }
            return filter;
        }
        #endregion

    }
}
