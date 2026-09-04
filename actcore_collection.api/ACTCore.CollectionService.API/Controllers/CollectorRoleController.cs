using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Extensions;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Collections;
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
    public class CollectorRoleController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly IActivityLogService _activityLog;
        private readonly ColRoleService _colRoleService;
        private readonly CollectorService _collectorService;
        private readonly LanguageService _languageService;

        public CollectorRoleController(IMapper mapper, IErrorLogService errorLog, IActivityLogService activityLog,
            ColRoleService collectorRoleService, CollectorService collectorService, LanguageService languageService) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _activityLog = activityLog;
            _colRoleService = collectorRoleService;
            _collectorService = collectorService;
            _languageService = languageService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<APIResponse>> GetColRuleList()
        {
            var response = new APIResponse();
            try
            {
                var actions = await _colRoleService.GetListAsync(x => x.IsActive == true);

                response.Data = _mapper.Map<List<ColRoleListResponseDto>>(actions);
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

        [HttpPost("list-paged")]
        public async Task<ActionResult<APIResponse>> GetColRuleListPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new APIResponse();
            try
            {
                var filter = BuildCollectorFilter(request);

                var result = await _colRoleService.GetListPaginationAsync(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "ColRoleCode",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters,
                    includeProperties: string.Empty
                );

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                var displayProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<ColRoleDisplayFilterDto>(enumValues);

                response = new ApiPaginationResponse
                {
                    Data = new PaginationData<ColRoleResponseDto>
                    {
                        Datatables = _mapper.Map<IEnumerable<ColRoleResponseDto>>(result.Items),
                        DisplayColumns = displayProperties,
                        TotalRecords = result.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)result.TotalCount / request.PageSize),
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
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetColRoleById(Guid id)
        {
            var response = new APIResponse();
            try
            {
                var colRole = await _colRoleService.GetAsync(x => x.ColRoleId == id);
                if (colRole == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Collector Role");
                    response.Status = false;
                    return Ok(response);
                }
                response.Data = _mapper.Map<ColRoleResponseDto>(colRole);
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
        public async Task<ActionResult<APIResponse>> CreateColRole([FromBody] ColRoleRequestDto request)
        {
            var response = new APIResponse();
            var entityId = "";
            try
            {
                if (request == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidRequest);
                    response.Status = false;
                    return Ok(response);
                }
                // Check for duplicate ColRoleCode
                if (await _colRoleService.GetAsync(x => x.ColRoleCode == request.ColRoleCode) != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CollectorRoleExists);
                    response.Status = false;
                    return Ok(response);
                }
                var colRole = _mapper.Map<ColRole>(request);
                VersioningModelHelper.SetCreatedAudit(colRole, User.Identity.Name);
                await _colRoleService.AddAsync(colRole);

                entityId = colRole.ColRoleId.ToString();

                response.Data = _mapper.Map<ColRoleResponseDto>(colRole);
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
                    LoggerHelper.BuildActivityCreateLog(request)
                    );
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> UpdateColRole([FromBody] ColRoleUpdateDto request)
        {
            var response = new APIResponse();
            ColRole existingColRole = null;
            ColRole beforeUpdate = null;
            try
            {
                // Check existing ColRole
                existingColRole = await _colRoleService.GetAsync(x => x.ColRoleId == request.ColRoleId);
                if (existingColRole == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Collector Role");
                    response.Status = false;
                    return Ok(response);
                }
                // Check for duplicate ColRoleCode
                if (await _colRoleService.GetAsync(x => x.ColRoleCode == request.ColRoleCode && x.ColRoleId != request.ColRoleId) != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CollectorRoleExists);
                    response.Status = false;
                    return Ok(response);
                }

                beforeUpdate = Utilities.ModelHelper.CloneModel(existingColRole);
                _mapper.Map(request, existingColRole);

                VersioningModelHelper.SetUpdatedAudit(existingColRole, User.Identity.Name);
                await _colRoleService.UpdateAsync(existingColRole);

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
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, request.ColRoleId.ToString(), ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActiviUpdateLog(beforeUpdate, existingColRole)
                );
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse>> DeleteColRole([FromBody] List<ColRoleDeleteDto> requests)
        {
            var response = new APIResponse();
            try
            {
                if (requests == null || !requests.Any())
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidRequest);
                    response.Status = false;
                    return Ok(response);
                }

                var colRoleIds = requests.Select(r => r.ColRoleId).ToList();
                var colRoles = await _colRoleService.GetListAsync(x => colRoleIds.Contains(x.ColRoleId));

                // Check if all ColRoles exist
                var notFoundIds = colRoleIds.Except(colRoles.Select(r => r.ColRoleId)).ToList();
                if (notFoundIds.Any())
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_2ParamsNotFound, "Collector Role Id", string.Join(", ", notFoundIds));
                    response.Status = false;
                    return Ok(response);
                }

                // Check ColRole in use
                var usedRoles = (await _collectorService.GetListAsync(x => colRoleIds.Contains(x.ColRoleId))).Select(x => x.ColRoleId)
                    .Distinct().ToList();
                if (usedRoles.Any())
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CollectorRoleIdRefCollector, string.Join(", ", usedRoles));
                    response.Status = false;
                    return Ok(response);
                }

                await _colRoleService.DeleteRangeAsync(colRoles);

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_DeletedSuccessfully);

                await _activityLog.ActivityLogAsync(
                    colRoleIds.Select(id => LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, id.ToString(),
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
                return await HandleExceptionAsync(response, ex);
            }
        }


        private static Expression<Func<ColRole, bool>> BuildCollectorFilter(PagedRequest<FilterContainer> request)
        {
            Expression<Func<ColRole, bool>> filter = null;
            if (request.Filters?.DynamicFilters != null)
            {
                foreach (var dynamicFilter in request.Filters.DynamicFilters)
                {
                    string key = dynamicFilter.Key;
                    string value = dynamicFilter.Value?.returnStringValue();

                    if (value == null || string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }
                    if (key.ToLower() == "colrolename" && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<ColRole, bool>> roleFilter = r =>
                            r != null && (r.ColRoleName.Contains(value));
                        filter = filter == null ? roleFilter : FilterExtensions.AndAlso(filter, roleFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                }
            }

            return filter;
        }
    }
}
