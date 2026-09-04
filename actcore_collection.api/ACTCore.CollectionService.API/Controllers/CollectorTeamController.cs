using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayFilterDto;
using ACTCore.CollectionService.API.Models.Dto.ListPagedDto;
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
using System.Net;


namespace ACTCore.CollectionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CollectorTeamController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly IActivityLogService _activityLog;
        private readonly ColTeamService _collectorTeamService;
        private readonly LanguageService _languageService;

        public CollectorTeamController(IMapper mapper, IErrorLogService errorLog, IActivityLogService activityLog,
            ColTeamService collectorTeamService, LanguageService languageService) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _activityLog = activityLog;
            _collectorTeamService = collectorTeamService;
            _languageService = languageService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<APIResponse>> GetCollectorTeamList()
        {
            var response = new APIResponse();
            try
            {
                var actions = await _collectorTeamService.GetListAsync(x => x.IsActive == true);

                response.Data = _mapper.Map<List<ColTeamListResponseDto>>(actions);
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
        public async Task<ActionResult<APIResponse>> GetListCollectorTeamPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var colTeams = await _collectorTeamService.GetListPaginationAsync(
                    filter: null,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "ColTeamCode",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters,
                    includeProperties: ""
                );

                List<ColTeamListPagedResponseDto> listPaged = new List<ColTeamListPagedResponseDto>();
                foreach (var item in colTeams.Items)
                {
                    listPaged.Add(new ColTeamListPagedResponseDto()
                    {
                        ColTeamId = item.ColTeamId,
                        ColTeamCode = item.ColTeamCode,
                        ColTeamName = item.ColTeamName,
                        Capacity = item.Capacity,
                        Description = item.Description,
                        IsActive = item.IsActive ? IconType.CHECKGREEN : IconType.CHECKRED
                    });
                }

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                var displayProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<ColTeamDisplayFilterDto>(enumValues);
                response = new ApiPaginationResponse
                {
                    Data = new PaginationData<ColTeamListPagedResponseDto>
                    {
                        Datatables = listPaged,
                        DisplayColumns = displayProperties,
                        TotalRecords = colTeams.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)colTeams.TotalCount / request.PageSize),
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

        

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetById(Guid id)
        {
            var response = new APIResponse();
            try
            {
                var colTeam = await _collectorTeamService.GetAsync(x => x.ColTeamId == id,
                    includeProperties: "");
                if (colTeam == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Collector Team");
                    response.Status = false;

                    return Ok(response);
                }

                response.Data = _mapper.Map<ColTeamResponseDto>(colTeam);
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
        public async Task<ActionResult<APIResponse>> CreateCollectorTeam([FromBody] ColTeamCreateDto createDto)
        {
            var response = new APIResponse();
            var entityId = "";
            try
            {
                if (createDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidRequestData);
                    response.Status = false;
                    return Ok(response);
                }

                // check duplicate Code, Name
                var dupColTeam = await _collectorTeamService.GetAsync(
                    x => x.ColTeamCode == createDto.ColTeamCode 
                    || x.ColTeamName == createDto.ColTeamName);
                
                if(dupColTeam != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_DuplicateDataInColumn
                        , (dupColTeam.ColTeamCode == createDto.ColTeamCode) ? "Team Code" : "Team Name");
                    response.Status = false;
                    return Ok(response);
                }

                // check capacity
                if (createDto.Capacity < 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidCapacity);
                    response.Status = false;
                    return Ok(response);
                }

                var colTeam = _mapper.Map<ColTeam>(createDto);

                VersioningModelHelper.SetCreatedAudit(colTeam, User.Identity.Name);
                var createdColTeam = await _collectorTeamService.AddAsync(colTeam);

                entityId = createdColTeam.ColTeamId.ToString();

                var responseColTeam = await _collectorTeamService.GetAsync(x => x.ColTeamId == createdColTeam.ColTeamId);

                response.Data = _mapper.Map<ColTeamResponseDto>(responseColTeam);
                response.StatusCode = HttpStatusCode.Created;
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
        public async Task<ActionResult<APIResponse>> UpdateCollectorTeam([FromBody] ColTeamUpdateDto updateDto)
        {
            var response = new APIResponse();
            ColTeam updatedColTeam = null;
            ColTeam beforUpdate = null;
            try
            {
                // check ColTeam exists
                updatedColTeam = await _collectorTeamService.GetAsync(x => x.ColTeamId == updateDto.ColTeamId);
                if (updatedColTeam == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Collector Team");
                    response.Status = false;
                    return Ok(response);
                }

                // check capacity
                if (updateDto.Capacity < 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidCapacity);
                    response.Status = false;
                    return Ok(response);
                }

                beforUpdate = Utilities.ModelHelper.CloneModel(updatedColTeam);
                _mapper.Map(updateDto, updatedColTeam);

                VersioningModelHelper.SetUpdatedAudit(updatedColTeam, User.Identity.Name);
                await _collectorTeamService.UpdateAsync(updatedColTeam);

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
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, updateDto.ColTeamId.ToString(), ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActiviUpdateLog(beforUpdate, updatedColTeam)
                    );
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse>> DeleteCollectorTeam([FromBody] List<ColTeamDeleteDto> requests)
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

                var colTeamIds = requests.Select(r => r.ColTeamId).ToList();
                var colTeams = await _collectorTeamService.GetListAsync(x => colTeamIds.Contains(x.ColTeamId));

                // Check ColTeams are exist
                var existingIds = colTeams.Select(t => t.ColTeamId).ToHashSet();
                var notFoundIds = colTeamIds.Except(existingIds).ToList();
                if (notFoundIds.Any())
                {
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_2ParamsNotFound, "Collector Team Id", string.Join(", ", notFoundIds));
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Status = false;
                    return Ok(response);
                }

                // Check ColTeams are using
                //var usedIds = (await _collectorTeamService.GetListAsync(x => colTeamIds.Contains(x.ColTeamId))).Select(x => x.ColTeamId)
                //                .Distinct()
                //                .ToList();
                //if (usedIds.Any())
                //{
                //    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CollectorTeamRefCollector, string.Join(", ", usedIds));
                //    response.StatusCode = HttpStatusCode.BadRequest;
                //    response.Status = false;
                //    return Ok(response);
                //}

                await _collectorTeamService.DeleteRangeAsync(colTeams);

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_DeletedSuccessfully);

                await _activityLog.ActivityLogAsync(
                    colTeamIds.Select(id => LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, id.ToString(),
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
    }
}