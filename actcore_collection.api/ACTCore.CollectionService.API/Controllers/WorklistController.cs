using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Models.Dto.ListPagedDto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Models;
using System.Net;


namespace ACTCore.CollectionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WorklistController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _logger;
        private readonly IActivityLogService _activityLog;
        private readonly TeamAssignmentService _teamAssignmentService;
        private readonly WorklistService _worklistService;
        private readonly EmployeeService _employeeService;
        private readonly CollectorService _collectorService;
        private readonly ColNoteActionService _colNoteActionService;
        private readonly ColNoteResultService _colNoteResultService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EnumService _enumService;
        private readonly LanguageService _languageService;
        private readonly WorklistHistoryService _worklistHistoryService;
        private readonly ColTeamService _colTeamService;

        public WorklistController(IMapper mapper, IErrorLogService logger, IActivityLogService activityLog,
            TeamAssignmentService teamAssignmentService, WorklistService worklistService, EmployeeService employeeService, CollectorService collectorService,
            ColNoteActionService colNoteActionService, ColNoteResultService colNoteResultService, IUnitOfWork unitOfWork,
            EnumService enumService, LanguageService languageService, WorklistHistoryService worklistHistoryService, ColTeamService colTeamService) : base(logger)
        {
            _mapper = mapper;
            _logger = logger;
            _activityLog = activityLog;
            _teamAssignmentService = teamAssignmentService;
            _worklistService = worklistService;
            _employeeService = employeeService;
            _collectorService = collectorService;
            _colNoteActionService = colNoteActionService;
            _colNoteResultService = colNoteResultService;
            _unitOfWork = unitOfWork;
            _enumService = enumService;
            _languageService = languageService;
            _worklistHistoryService = worklistHistoryService;
            _colTeamService = colTeamService;
        }

        [HttpPost("list-paged")]
        public async Task<ActionResult<APIResponse>> GetWorklistPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var today = DateTime.Now.Date;
                var teamAssignment = await _teamAssignmentService.GetActiveColTeamAssignment(CurrentUserInfo.UserId, today);

                // Check Active Collector/ Collector Team
                if (teamAssignment == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_AccessDenyColOrTeamInactive);
                    response.Status = false;
                    return Ok(response);
                }

                var worklists = await _worklistService.GetListPaginationByViewAsync(
                    teamAssignment: teamAssignment,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "DayPastDue, OutstandingBalance",
                    sortDirection: request.SortDirection ?? "desc",
                    filterColumns: request.Filters
                );

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                if (teamAssignment.IsSupervisor)
                {
                    var filterSupervisorProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<WorklistListingSupervisorDisplayFilterDto>(enumValues);
                    response = new ApiPaginationResponse()
                    {
                        Data = new PaginationData<WorklistListPagedSupervisorResponseDto>
                        {
                            Datatables = worklists.Items.Select(item => _mapper.Map<WorklistListPagedSupervisorResponseDto>(item)).ToList(),
                            DisplayColumns = filterSupervisorProperties,
                            TotalRecords = worklists.TotalCount,
                            TotalPages = (int)Math.Ceiling((double)worklists.TotalCount / request.PageSize),
                            PageNumber = request.PageNumber,
                            PageSize = request.PageSize,
                        },
                        StatusCode = HttpStatusCode.OK,
                        Status = true
                    };
                    return Ok(response);
                }
                else
                {
                    var filterCollectorProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<WorklistListingDisplayFilterDto>(enumValues);
                    response = new ApiPaginationResponse()
                    {
                        Data = new PaginationData<WorklistListPagedResponseDto>
                        {
                            Datatables = worklists.Items.Select(item => _mapper.Map<WorklistListPagedResponseDto>(item)).ToList(),
                            DisplayColumns = filterCollectorProperties,
                            TotalRecords = worklists.TotalCount,
                            TotalPages = (int)Math.Ceiling((double)worklists.TotalCount / request.PageSize),
                            PageNumber = request.PageNumber,
                            PageSize = request.PageSize,
                        },
                        StatusCode = HttpStatusCode.OK,
                        Status = true
                    };
                    return Ok(response);
                }
                ;

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

        [HttpPost("approve-reassign-list-paged")]
        public async Task<ActionResult<APIResponse>> GetApproveWorklistPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var today = DateTime.Now.Date;
                var approver = await _teamAssignmentService.GetActiveColTeamAssignment(CurrentUserInfo.UserId, today, isSupervisor: true);

                // Check Active Supervisor
                if (approver == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_AccessDenyColOrTeamInactive);
                    response.Status = false;
                    return Ok(response);
                }

                // Get reassign of team members and reassign to supervisor
                var requestReassigns = await _worklistService.GetListPaginationReassignWorklistAsync(x =>
                        x.ReassignToTeamId == approver.ColTeamId,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "RequestedDate",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters
                );

                List<ReassignWorklistListPagedResponseDto> worklistList = new List<ReassignWorklistListPagedResponseDto>();
                foreach (var item in requestReassigns.Items)
                {
                    var worklist = new ReassignWorklistListPagedResponseDto();
                    worklist.WorklistId = item.WorklistId;
                    worklist.ContractNo = item.ContractNo;
                    worklist.CustomerName = item.CustomerName;
                    worklist.AssetType = item.AssetType;
                    worklist.ContractStatus = item.ContractStatus;
                    worklist.FollowupStatus = item.FollowupStatus;
                    worklist.ReassignBy = item.ReassignBy;
                    worklist.TeamFrom = item.TeamFrom;
                    worklist.ReassignTo = item.ReassignTo;
                    worklist.TeamTo = item.TeamTo;
                    worklist.RequestedDate = item.RequestedDate;
                    worklist.Reason = item.Reason;
                    worklist.ReassignStatus = item.ReassignStatus;
                    worklist.IsMyTeam = item.IsMyTeam ? IconType.CHECKGREEN
                        : "";
                    worklistList.Add(worklist);
                }

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                var filterSupervisorProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<ReassignWorklistDisplayFilterDto>(enumValues);
                response = new ApiPaginationResponse()
                {
                    Data = new PaginationData<ReassignWorklistListPagedResponseDto>
                    {
                        Datatables = worklistList,
                        DisplayColumns = filterSupervisorProperties,
                        TotalRecords = requestReassigns.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)requestReassigns.TotalCount / request.PageSize),
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
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

        [HttpPost("unassign-worklist-list-paged")]
        public async Task<ActionResult<APIResponse>> GetUnassignWorklistPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var today = DateTime.Now.Date;
                var teamAssignment = await _teamAssignmentService.GetActiveColTeamAssignment(CurrentUserInfo.UserId, today, isSupervisor: true);

                // Check Active Supervisor
                if (teamAssignment == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_AccessDenyColOrTeamInactive);
                    response.Status = false;
                    return Ok(response);
                }

                // Get reassign of team members and reassign to supervisor
                var requestReassigns = await _worklistService.GetListPaginationUnassignWorklistAsync(
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "AssignDate",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters
                );

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                var filterSupervisorProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<UnassignWorklistDisplayFilterDto>(enumValues);
                response = new ApiPaginationResponse()
                {
                    Data = new PaginationData<UnassignWorklistListPagedResponseDto>
                    {
                        Datatables = requestReassigns.Items.Select(item => _mapper.Map<UnassignWorklistListPagedResponseDto>(item)).ToList(),
                        DisplayColumns = filterSupervisorProperties,
                        TotalRecords = requestReassigns.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)requestReassigns.TotalCount / request.PageSize),
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
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

        [NonAction]
        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetById(Guid id)
        {
            var response = new APIResponse();
            try
            {
                var workList = await _worklistService.GetAsync(x => x.WorklistId == id);
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Worklist");
                    response.Status = false;

                    return Ok(response);
                }

                response.Data = workList;
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

        [HttpGet("followup-action-list")]
        public async Task<ActionResult<APIResponse>> GetFollowupActionList()
        {
            var response = new APIResponse();
            try
            {
                var actions = await _colNoteActionService.GetListAsync(x => x.IsActive == true);

                response.Data = _mapper.Map<List<ColNoteActionResponseDto>>(actions);
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

        [HttpGet("followup-result-list/{actionId}")]
        public async Task<ActionResult<APIResponse>> GetFollowupActionList(int actionId)
        {
            var response = new APIResponse();
            try
            {
                var results = await _colNoteResultService.GetListAsync(x => x.ActionId == actionId && x.IsActive == true);

                response.Data = _mapper.Map<List<ColNoteResultResponseDto>>(results);
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

        [HttpGet("detail/{worklistId}")]
        public async Task<IActionResult> GetDetail(Guid? worklistId)
        {
            var response = new APIResponse();
            try
            {
                //var EmployeeList = await _employeeService.GetListAsync();
                var workList = await _worklistService.GetAsync(x => x.WorklistId == worklistId && x.ReassignStatusCode != Enums.ReassignStatus.WaitForApprove,
                    includeProperties: "Contract.ContractPersons.ContractPhones,Contract.ContractPersons.Prefix,CollectionNotes.ColNoteAction,Collector.User");
                if (workList == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Worklist");
                    response.Status = false;

                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                var contractPerson = workList?.Contract?.ContractPersons?.FirstOrDefault(y => y.PersonType == "B");
                var followupAction = workList?.CollectionNotes?.OrderByDescending(n => n.UpdatedDate).FirstOrDefault()?.ColNoteAction.ActionDescription;

                var enumStatus = _enumService.GetListAsync(x => x.EnumName == "ContractStatus").Result
                    .ToDictionary(e => e.EnumCode, e => e.EnumDescription);
                if (!String.IsNullOrEmpty(workList?.Contract?.ContractStatus)
                    && enumStatus.TryGetValue(workList?.Contract?.ContractStatus, out var status))
                {
                    workList.Contract.ContractStatus = status;
                }

                
                response.Data = _mapper.Map<WorklistDetailResponseDto>(workList);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
        }

        [HttpPost("manual-reassign")]
        public async Task<ActionResult<APIResponse>> ManualReassign([FromBody] List<ManualReassignRequestDto> requests, [FromQuery] bool unassign = false)
        {
            var response = new APIResponse();
            // Temporarily disable change tracker (speed)
            _unitOfWork.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                if (requests == null || requests.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidData);
                    response.Status = false;
                    return Ok(response);
                }

                var reassignStatuses = await _enumService.GetListAsync(x => x.EnumName == "ReassignStatus");

                var assignType = await _enumService.GetAsync(x => x.EnumName == "AssignType" && x.EnumCode == "ReassignManual");

                var requesterTeam = await _teamAssignmentService.GetAsync(x => x.Collector.UserId == CurrentUserInfo.UserId,
                    includeProperties: "Collector,Collector.User,Collector.User.Employee,Collector.User.Employee.Prefix");

                // Preload: collector & team info for all targets
                var collectorIds = requests.Select(r => r.CollectorId).Distinct().ToList();

                var collectors = await _collectorService.GetListAsync( 
                    x => collectorIds.Contains(x.CollectorId)
                    , includeProperties: "User,User.Employee,User.Employee.Prefix");

                var teamAssignments = await _teamAssignmentService.GetListAsync(
                    x => collectorIds.Contains(x.CollectorId), includeProperties: "ColTeam");

                // ดึง worklist ทั้งหมดทีเดียว
                var requestIds = requests.Select(r => r.WorklistId).ToList();
                
                var worklists = (await _worklistService.GetListAsync(
                    x => requestIds.Contains(x.WorklistId),
                    includeProperties: "Collector.User"
                )).ToList();

                if (worklists.Count != requests.Count)
                {
                    response.Status = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Worklist");
                    return Ok(response);
                }
                // Performance: to lookup instead of List
                var dictWorklists = worklists.ToDictionary(x => x.WorklistId);
                var dictCollectors = collectors.ToDictionary(x => x.CollectorId);
                var dictTeams = teamAssignments.ToDictionary(x => x.CollectorId);

                // Preload: Permission
                var currentUserTeamAssignments = await _teamAssignmentService.GetListAsync(
                    x => x.Collector.UserId == CurrentUserInfo.UserId);
                var now = DateTime.Now;
                var dictCurrentUserTeamAssignments = currentUserTeamAssignments
                    .Where(x =>
                        (x.EffectiveDate == null || x.EffectiveDate.Value.Date <= now) &&
                        (x.ExpireDate == null || x.ExpireDate.Value.Date >= now)
                    )
                    .ToDictionary(x => x.ColTeamId, x => x);

                foreach (var request in requests)
                {
                    var worklist = dictWorklists[request.WorklistId];
                    var reassignTo = dictCollectors[request.CollectorId];
                    var reassignTeam = dictTeams[request.CollectorId];

                    var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermissionByTeamAssignment(worklist, CurrentUserInfo.UserId, _teamAssignmentService, dictCurrentUserTeamAssignments, unassign);
                    if (!hasPermission)
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                        response.Status = false;
                        return Ok(response);
                    }

                    if (reassignTo == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, $"Collector Id: {request.CollectorId}");
                        response.Status = false;
                        return Ok(response);
                    }

                    worklist.ReassignReason = request.ReassignReason;
                    worklist.ReassignRequestDate = DateTime.Now;

                    worklist.ReassignById = requesterTeam.Collector.CollectorId;
                    worklist.ReassignByEmpId = requesterTeam.Collector.User.EmployeeId;
                    worklist.ReassignByName = requesterTeam.Collector.User.Employee.FullName;

                    worklist.ReassignFromId = worklist.AssignCollectorId;
                    worklist.ReassignFromEmpId = worklist.AssignCollectorEmpId;
                    worklist.ReassignFromName = worklist.AssignCollectorName;

                    worklist.ReassignToId = reassignTo.CollectorId;
                    worklist.ReassignToEmpId = reassignTo.User.EmployeeId;
                    worklist.ReassignToName = reassignTo.User.Employee.FullName;

                    worklist.ReassignToTeamId = reassignTeam.ColTeamId;
                    worklist.ReassignToTeamCode = reassignTeam.ColTeam.ColTeamCode;
                    worklist.ReassignToTeamName = reassignTeam.ColTeam.ColTeamName;

                    bool isSameTeam = requesterTeam.ColTeamId == reassignTeam.ColTeamId;
                    bool isRequesterSup = requesterTeam.IsSupervisor;
                    bool isReassignSup = reassignTeam.IsSupervisor;

                    // Case: Requester Collector, Reassign Collector, same team
                    if (!isRequesterSup && !isReassignSup && isSameTeam)
                    {
                        SetWaitingForApprove(worklist, reassignStatuses);
                    }
                    // Case: Requester Supervisor, Reassign Collector, same team
                    else if (isRequesterSup && !isReassignSup && isSameTeam)
                    {
                        SetApproveReassign(worklist: worklist, approveReason: request.ReassignReason, assignCollector: reassignTo, assignType: assignType, assignTeam: reassignTeam.ColTeam, approveBy: requesterTeam, reassignStatuses);
                    }
                    // case: Requester Collector, Reassign Supervisor, same team
                    else if (!isRequesterSup && isReassignSup && isSameTeam)
                    {
                        SetWaitingForApprove(worklist, reassignStatuses);
                    }
                    // Case: Requester Supervisor, Reassign Supervisor
                    else if (isRequesterSup && isReassignSup)
                    {
                        // Case: Self reassign
                        if (reassignTo.CollectorId == requesterTeam.Collector.CollectorId)
                            SetApproveReassign(worklist: worklist, approveReason: request.ReassignReason, assignCollector: reassignTo, assignType: assignType, assignTeam: reassignTeam.ColTeam, approveBy: requesterTeam, reassignStatuses);
                        else
                            SetWaitingForApprove(worklist, reassignStatuses);
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CrossTeamReasignmentNotAllow);
                        response.Status = false;
                        return Ok(response);
                    }

                    VersioningModelHelper.SetUpdatedAudit(worklist, User.Identity.Name);

                }

                await _worklistService.BulkUpdateAsync(worklists);

                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_ReassignmentCompleted);
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
                _unitOfWork.DbContext.ChangeTracker.Clear();
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }

        }

        [HttpPost("approve-reassign")]
        public async Task<ActionResult<APIResponse>> ApproveReassign([FromBody] List<ApproveReassignRequestDto> requests)
        {
            var response = new APIResponse();
            var tran = await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (requests == null || requests.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidData);
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                var reassignStatuses = await _enumService.GetListAsync(x => x.EnumName == "ReassignStatus");

                var assignType = await _enumService.GetAsync(x => x.EnumName == "AssignType" && x.EnumCode == "ReassignManual");

                var approveBy = await _teamAssignmentService.GetAsync(x => x.Collector.UserId == CurrentUserInfo.UserId,
                    includeProperties: "Collector,Collector.User,Collector.User.Employee,Collector.User.Employee.Prefix");

                foreach (var request in requests)
                {
                    var worklist = await _worklistService.GetAsync(x => x.WorklistId == request.WorklistId, asNoTracking: false,
                        includeProperties: "Collector.User,ReassignTo,ReassignToTeam");
                    if (worklist == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, $"Worklist Id: {request.WorklistId}");
                        response.Status = false;
                        await _unitOfWork.RollbackTransactionAsync();
                        return Ok(response);
                    }

                    var hasPermission = await WorklistPermissionHelpers.CheckApprovePermission(worklist, CurrentUserInfo.UserId, _teamAssignmentService);
                    if (!hasPermission)
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NoPermissiontoApproveContactID, worklist.ContractNo);
                        response.Status = false;
                        await _unitOfWork.RollbackTransactionAsync();
                        return Ok(response);
                    }

                    var assignCollector = await _collectorService.GetAsync(x => x.CollectorId == worklist.ReassignTo.CollectorId, includeProperties: "User,User.Employee,User.Employee.Prefix");

                    if (request.Type.ToUpperInvariant() == "R")
                    {
                        SetRejectReassign(worklist: worklist, rejectReason: request.ApproveReason, rejectedBy: approveBy, reassignStatuses);
                    }
                    else
                    {
                        SetApproveReassign(worklist: worklist,
                            approveReason: request.ApproveReason,
                            assignCollector: assignCollector,
                            assignType: assignType,
                            assignTeam: worklist.ReassignToTeam,
                            approveBy: approveBy,
                            reassignStatuses: reassignStatuses);
                    }

                    VersioningModelHelper.SetUpdatedAudit(worklist, User.Identity.Name);
                    await _worklistService.UpdateAsync(worklist);
                }

                await _unitOfWork.CommitTransactionAsync();

                var msg = requests?[0].Type?.ToUpperInvariant() == "R" ? Message.Msg_RejectedSuccessfully : Message.Msg_ApprovedSuccessfully;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, msg);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                await _unitOfWork.RollbackTransactionAsync();
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        private void SetApproveReassign(Worklist worklist, string approveReason, CollectorProfile assignCollector, SysEnum assignType, ColTeam assignTeam, ColTeamAssignment approveBy, IEnumerable<SysEnum> reassignStatuses)
        {
            worklist.AssignDate = DateTime.Now;

            worklist.ApproveDate = DateTime.Now;
            worklist.ApproveReason = approveReason;

            worklist.ApprovedById = approveBy.Collector.CollectorId;
            worklist.ApprovedEmpId = approveBy.Collector.User.EmployeeId;
            worklist.ApprovedName = approveBy.Collector.User.Employee.FullName;

            worklist.AssignCollectorId = assignCollector.CollectorId;
            worklist.AssignCollectorEmpId = assignCollector.User.EmployeeId;
            worklist.AssignCollectorName = assignCollector.User.Employee.FullName;

            worklist.AssignTypeCode = assignType.EnumCode;
            worklist.AssignTypeDesc = assignType.EnumDescription;

            worklist.AssignTeamId = assignTeam.ColTeamId;
            worklist.AssignTeamCode = assignTeam.ColTeamCode;
            worklist.AssignTeamName = assignTeam.ColTeamName;

            worklist.ReassignStatusCode = reassignStatuses.FirstOrDefault(x => x.EnumCode == Enums.ReassignStatus.Approved)?.EnumCode;
            worklist.ReassignStatusDesc = reassignStatuses.FirstOrDefault(x => x.EnumCode == Enums.ReassignStatus.Approved)?.EnumDescription;
        }

        private void SetWaitingForApprove(Worklist worklist, IEnumerable<SysEnum> reassignStatuses)
        {
            worklist.ReassignStatusCode = reassignStatuses.FirstOrDefault(x => x.EnumCode == Enums.ReassignStatus.WaitForApprove)?.EnumCode;
            worklist.ReassignStatusDesc = reassignStatuses.FirstOrDefault(x => x.EnumCode == Enums.ReassignStatus.WaitForApprove)?.EnumDescription;

            worklist.ApproveDate = null;
            worklist.ApproveReason = null;

            worklist.ApprovedById = null;
            worklist.ApprovedEmpId = null;
            worklist.ApprovedName = null;
        }

        private void SetRejectReassign(Worklist worklist, string rejectReason, ColTeamAssignment rejectedBy, IEnumerable<SysEnum> reassignStatuses)
        {
            worklist.ReassignStatusCode = reassignStatuses.FirstOrDefault(x => x.EnumCode == Enums.ReassignStatus.Rejected)?.EnumCode;
            worklist.ReassignStatusDesc = reassignStatuses.FirstOrDefault(x => x.EnumCode == Enums.ReassignStatus.Rejected)?.EnumDescription;

            worklist.ApproveDate = DateTime.Now;
            worklist.ApproveReason = rejectReason;

            worklist.ApprovedById = rejectedBy.Collector.CollectorId;
            worklist.ApprovedEmpId = rejectedBy.Collector.User.EmployeeId;
            worklist.ApprovedName = rejectedBy.Collector.User.Employee.FullName;
        }
    }
}
