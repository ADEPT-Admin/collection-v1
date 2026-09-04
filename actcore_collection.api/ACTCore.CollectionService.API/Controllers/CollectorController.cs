using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Extensions;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Models.Dto.ListPagedDto;
using ACTCore.CollectionService.API.Models.Dto.LogDetailDto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
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

namespace ACTCore.CollectionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CollectorController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly IActivityLogService _activityLog;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CollectorService _collectorService;
        private readonly ColRoleService _colRoleService;
        private readonly LanguageService _languageService;
        private readonly UserService _userService;

        public CollectorController(IMapper mapper, IErrorLogService errorLog, IActivityLogService activityLog,
            CollectorService collectorService, ColRoleService colRoleService, LanguageService languageService,
            IUnitOfWork unitOfWork, UserService userService) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _activityLog = activityLog;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _collectorService = collectorService;
            _colRoleService = colRoleService;
            _languageService = languageService;
        }

        [HttpPost("list-paged")]
        public async Task<ActionResult<APIResponse>> GetListCollectorPaged([FromBody] PagedRequest<FilterContainer> request, [FromQuery] bool isActiveCollector = false)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var filter = BuildCollectorFilter(request, isActiveCollector);
                request.SortColumn = request.SortColumn switch
                {
                    "CollectorId"=> "User.Employee.EmployeeId",
                    "CollectorName" => "User.Employee.FullName",
                    "ColRoleName" => "ColRole.ColRoleName",
                    "PhoneNo" => "User.Employee.PhoneNo",
                    "Email" => "User.Employee.Email",
                    _ => request.SortColumn
                };

                var collectors = await _collectorService.GetListPaginationAsync(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "User.Employee.EmployeeId",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters,
                    includeProperties: "User,User.Employee.Prefix,ColRole"
                );

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                
                response = new ApiPaginationResponse
                {
                    Data = isActiveCollector 
                        ? PaginationDataHelper.BuildPaginationData
                        <CollectorDisplayFilterActiveDto, CollectorListPagedResponseDto, CollectorProfile>
                        (collectors, request.PageNumber, request.PageSize, enumValues, _mapper)
                        : PaginationDataHelper.BuildPaginationData
                        <CollectorDisplayFilterDto, CollectorListPagedResponseDto, CollectorProfile>
                        (collectors, request.PageNumber, request.PageSize, enumValues, _mapper),
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

        [HttpPost("list-paged-assign-collector")]
        public async Task<ActionResult<APIResponse>> GetListCollectorAssignTeamPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var filter = BuildCollectorFilter(request, true);
                request.SortColumn = request.SortColumn switch
                {
                    "CollectorId" => "User.Employee.EmployeeId",
                    "CollectorName" => "User.Employee.FullName",
                    "ColRoleName" => "ColRole.ColRoleName",
                    _ => request.SortColumn
                };

                var collectors = await _collectorService.GetListPaginationAsync(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "User.Employee.EmployeeId",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters,
                    includeProperties: "User,User.Employee,User.Employee.Prefix,ColRole"
                );

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);

                response = new ApiPaginationResponse
                {
                    Data = PaginationDataHelper.BuildPaginationData
                        <CollectorAssignTeamDisplayFilterDto, CollectorAssignTeamListPagedResponseDto, CollectorProfile>
                        (collectors, request.PageNumber, request.PageSize, enumValues, _mapper),
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

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetById(string id)
        {
            var response = new APIResponse();
            try
            {
                var collector = await _collectorService.GetAsync(x => x.User.EmployeeId == id, includeProperties: "User.Employee.Prefix,ColRole");
                if (collector == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Collector");
                    response.Status = false;

                    return Ok(response);
                }

                response.Data = _mapper.Map<CollectorGetByIdResponseDto>(collector);
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
        public async Task<ActionResult<APIResponse>> CreateCollector([FromBody] CollectorCreateDto createDto)
        {
            var response = new APIResponse();
            var entityId = "";
            try
            {
                // validate Collector exists
                var existingCollector = await _collectorService.GetAsync(x => x.UserId == createDto.UserId, includeProperties: "User.Employee");
                if (existingCollector != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CollectorExist, existingCollector.User.Employee.EmployeeId);
                    response.Status = false;
                    return Ok(response);
                }

                // Validate ColRoleId
                var colRole = await _colRoleService.GetAsync(x => x.ColRoleId == createDto.ColRoleId);
                if (colRole == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "ColRoleId");
                    response.Status = false;
                    return Ok(response);
                }
                else if (!colRole.IsActive)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_Inactive, "Collector Role");
                    response.Status = false;
                    return Ok(response);
                }

                // Validate UserId not found
                var user = await _userService.GetAsync(x => x.UserId == createDto.UserId);
                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "UserId");
                    response.Status = false;
                    return Ok(response);
                }

                var collector = _mapper.Map<CollectorProfile>(createDto);
                collector.CollectorId = Guid.NewGuid();

                VersioningModelHelper.SetCreatedAudit(collector, User.Identity.Name);
                await _collectorService.AddAsync(collector);

                entityId = collector.CollectorId.ToString();

                var newCollector = await _collectorService.GetAsync(x => x.CollectorId == collector.CollectorId, includeProperties: "User,User.Employee,User.Employee.Prefix,ColRole");

                response.Data = _mapper.Map<CollectorResponseDto>(newCollector);
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

        [HttpPost("bulk-create")]
        public async Task<ActionResult<APIResponse>> BulkCreateCollector([FromBody] CollectorBulkCreateDto createDtos)
        {
            List<CollectorProfile> createdCollectors = null;
            var response = new APIResponse();
            var tran = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Validate Empty UserIds
                if (createDtos.UserIds == null || !createDtos.UserIds.Any())
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_AtLeastOneRequired, "UserIds");
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }
                
                // Validate ColRoleId
                var colRole = await _colRoleService.GetAsync(x => x.ColRoleId == createDtos.ColRoleId);
                if (colRole == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "ColRoleId");
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }
                else if (!colRole.IsActive)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_Inactive, "Collector Role");
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // Validate Existing Collectors
                var existingCollectors = await _collectorService.GetListAsync(x => createDtos.UserIds.Contains(x.UserId), includeProperties: "User.Employee");
                if (existingCollectors.Any())
                {
                    var existingEmployeeIds = existingCollectors.Select(ec => ec.User.Employee.EmployeeId);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CollectorExistByEmployeeId, string.Join(", ", existingEmployeeIds));
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // Validate UserId not found
                var userIdsInDb = await _userService.GetListAsync(x => createDtos.UserIds.Contains(x.UserId));
                var notFoundUserIds = createDtos.UserIds.Except(userIdsInDb.Select(u => u.UserId)).ToList();
                if (notFoundUserIds.Any())
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_2ParamsNotFound, "User ID", string.Join(", ", notFoundUserIds));
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_SavedSuccessfully);

                createdCollectors = createDtos.UserIds.Select(userId => new CollectorProfile
                {
                    CollectorId = Guid.NewGuid(),
                    UserId = userId,
                    ColRoleId = createDtos.ColRoleId,
                    IsActive = createDtos.IsActive,
                }).ToList();

                // Set Audit Fields && Logging
                var logs = new List<ActivityLog>();
                foreach (var collector in createdCollectors)
                {
                    VersioningModelHelper.SetCreatedAudit(collector, User.Identity.Name);
                    
                    var log = LoggerHelper.BuildActivityLog(ActivityLogAction.CREATE, collector.UserId.ToString(), ControllerContext, CurrentUserInfo, response);

                    LoggerHelper.SetActivityLogDetail( log,
                        LoggerHelper.BuildActivityCreateLog( new LogDetailCollectorBulkCreateDto
                        {
                            ColRoleId = createDtos.ColRoleId,
                            UserId = collector.UserId,
                            IsActive = createDtos.IsActive,
                        })
                    );

                    logs.Add(log);
                }

                await _collectorService.AddRangeAsync(createdCollectors);
                await _unitOfWork.CommitTransactionAsync();
                await _activityLog.BulkInsertActivityLogCreateWithDetailsAsync(logs);
                
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

        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> UpdateCollector([FromBody] CollectorUpdateDto updateDto)
        {
            var response = new APIResponse();
            CollectorProfile updatedCollector = null;
            CollectorProfile beforUpdate = null;
            try
            {
                // check collector id not exists
                updatedCollector = await _collectorService.GetAsync(x => x.CollectorId == updateDto.CollectorId);
                if (updatedCollector == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "CollectorId");
                    response.Status = false;
                    return Ok(response);
                }

                beforUpdate = Utilities.ModelHelper.CloneModel(updatedCollector);
                _mapper.Map(updateDto, updatedCollector);

                VersioningModelHelper.SetUpdatedAudit(updatedCollector, User.Identity.Name);
                await _collectorService.UpdateAsync(updatedCollector);

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_SavedSuccessfully);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogEditWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, updateDto.CollectorId.ToString(), ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActiviUpdateLog(beforUpdate, updatedCollector)
                    );
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse>> DeleteCollector([FromBody] List<CollectorDeleteDto> requests)
        {
            var response = new APIResponse();
            try
            {
                if (requests == null || !requests.Any())
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    return Ok(response);
                }

                var collectorIds = requests.Select(r => r.CollectorId).ToList();
                var collectors = await _collectorService.GetListAsync(x => collectorIds.Contains(x.CollectorId));

                var notFoundIds = collectorIds.Except(collectors.Select(u => u.CollectorId)).ToList();
                if (notFoundIds.Any())
                {
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_2ParamsNotFound, "Collector Id", string.Join(", ", notFoundIds));
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Status = false;
                    return Ok(response);
                }
                await _collectorService.DeleteRangeAsync(collectors);

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_DeletedSuccessfully);

                await _activityLog.ActivityLogAsync(
                    collectors.Select(x => LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, x.CollectorId.ToString(),
                    ControllerContext, CurrentUserInfo, response)).ToList()
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add(ex.Message);

                if (ex.ToString().Contains("REFERENCE"))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Status = false;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CannotDeleteCollectorExistTeam);

                    await LogDeleteAsync(response);
                    return Ok(response);
                }

                await LogDeleteAsync(response);
                return await HandleExceptionAsync(response, ex);
            }
        }

        private async Task LogDeleteAsync(APIResponse response)
        {
            await _activityLog.ActivityLogAsync(
                LoggerHelper.BuildActivityLog(
                    ActivityLogAction.DELETE,
                    string.Empty,
                    ControllerContext,
                    CurrentUserInfo,
                    response));
        }

        private static Expression<Func<CollectorProfile, bool>> BuildCollectorFilter(PagedRequest<FilterContainer> request, bool isActiveCollector)
        {
            Expression<Func<CollectorProfile, bool>> filter = x => !isActiveCollector || x.IsActive == true;

            if (request.Filters?.DynamicFilters != null)
            {
                foreach (var dynamicFilter in request.Filters.DynamicFilters)
                {
                    string key = dynamicFilter.Key;
                    string value = dynamicFilter.Value?.returnStringValue();

                    if (value == null || string.IsNullOrWhiteSpace(value)) continue;
                    
                    // CollectorId (EmployeeId)
                    if (String.Equals(key, nameof(CollectorListPagedResponseDto.CollectorId), StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<CollectorProfile, bool>> empFilter = e =>
                            e != null && (
                                e.User != null &&
                                e.User.EmployeeId != null &&
                                e.User.EmployeeId.Contains(value));
                        filter = FilterExtensions.AndAlso(filter, empFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // CollectorName
                    else if (String.Equals(key, nameof(CollectorListPagedResponseDto.CollectorName), StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<CollectorProfile, bool>> empFilter = u =>
                            u != null && u.User.Employee != null && (
                                string.Join(" ",
                                    new[] {u.User.Employee.Prefix != null ? u.User.Employee.Prefix.PrefixName : "",
                                u.User.Employee.FirstName ?? "",
                                u.User.Employee.LastName ?? "" }
                                ).Contains(value)
                            );
                        filter = FilterExtensions.AndAlso(filter, empFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // ColRoleName
                    else if (String.Equals(key, nameof(CollectorListPagedResponseDto.ColRoleName), StringComparison.OrdinalIgnoreCase)   
                    && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<CollectorProfile, bool>> roleFilter = r =>
                            r != null && r.ColRole.ColRoleName != null && r.ColRole.ColRoleName.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, roleFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // PhoneNo
                    else if (string.Equals(key, nameof(CollectorListPagedResponseDto.PhoneNo), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<CollectorProfile, bool>> phoneFilter = p =>
                            p != null && p.User.Employee != null && p.User.Employee.PhoneNo != null && p.User.Employee.PhoneNo.Contains(value);
                        filter = filter == null ? phoneFilter : FilterExtensions.AndAlso(filter, phoneFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // Email
                    else if (string.Equals(key, nameof(CollectorListPagedResponseDto.Email), StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<CollectorProfile, bool>> emailFilter = em =>
                            em != null && em.User.Employee != null && em.User.Employee.Email != null && em.User.Employee.Email.Contains(value);
                        filter = filter == null ? emailFilter : FilterExtensions.AndAlso(filter, emailFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                }
            }

            return filter;
        }

    }
}
