using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ValueObjects;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Helpers;
using SharedKernel.Models;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;


namespace ACTCore.CollectionService.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MenuController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _logger;
        private readonly IActivityLogService _activityLog;
        private readonly SysItemService _sysItemService;
        private readonly LanguageService _languageService;

        public MenuController(IMapper mapper, IErrorLogService logger, IActivityLogService activityLog
            , SysItemService sysItemService, LanguageService languageService) : base(logger)
        {
            _mapper = mapper;
            _logger = logger;
            _activityLog = activityLog;
            _sysItemService = sysItemService;
            _languageService = languageService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<APIResponse>> GetListSysItems()
        {
            var response = new APIResponse();
            try
            {
                var items = await _sysItemService.GetListAsync(x => x.IsActive == true);

                response.Data = _mapper.Map<List<SysItemResponseDto>>(items);
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

        [HttpPost("list-paged")]
        public async Task<ActionResult<APIResponse>> GetListSysItemsPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var items = await _sysItemService.GetListPaginationAsync(
                    filter: null,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn,
                    sortDirection: request.SortDirection,
                    filterColumns: request.Filters,
                    includeProperties: ""
                );

                var itemDtos = items.Items.Select(lang => new SysItemListResponseDto
                {
                    ItemId = lang.ItemId,
                    ParentId = lang.ParentId,
                    ItemNameEn = lang.ItemNameEn,
                    ItemNameTh = lang.ItemNameTh,
                    RouteName = lang.RouteName,
                    ItemOrder = lang.ItemOrder,
                    ItemLevel = lang.ItemLevel,
                    Icon = lang.Icon,
                    ToolTip = lang.ToolTip,
                    IsActive = lang.IsActive ? IconType.CHECKGREEN : IconType.CHECKRED
                }).ToList();

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                var displayProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<SysItemDisplayFilterDto>(enumValues);

                response = new ApiPaginationResponse()
                {
                    Data = new PaginationData<SysItemListResponseDto>
                    {
                        Datatables = itemDtos,
                        DisplayColumns = displayProperties,
                        TotalRecords = items.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)items.TotalCount / request.PageSize),
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

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetById(string id)
        {
            var response = new APIResponse();
            try
            {
                var item = await _sysItemService.GetAsync(x => x.ItemId == id);
                if (item == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Menu");
                    return Ok(response);
                }

                response.Data = _mapper.Map<SysItemResponseDto>(item);
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
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, id, ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> CreateSysItem([FromBody] SysItemCreateDto createDto)
        {
            var response = new APIResponse();
            var entityId = string.Empty;
            try
            {
                if (createDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidData);
                    response.Status = false;
                    return Ok(response);
                }
                // check ItemId Exists
                if (await _sysItemService.GetAsync(x => x.ItemId == createDto.ItemId) != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_ItemIdAlreadyExists);
                    response.Status = false;
                    return Ok(response);
                }
                // check ItemName Exists
                var itemNameJson = JsonSerializer.Serialize(createDto.ItemName, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                var itemNameExists = (await _sysItemService.GetListAsync(x => x.ItemName == itemNameJson)).Any();
                if (itemNameExists)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_ItemNameAlreadyExists);
                    response.Status = false;
                    return Ok(response);
                }

                var item = _mapper.Map<SysItem>(createDto);
                VersioningModelHelper.SetCreatedAudit(item, User.Identity.Name);
                await _sysItemService.AddAsync(item);
                entityId = item.ItemId;

                response.Data = _mapper.Map<SysItemResponseDto>(item);
                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_SavedSuccessfully);
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
                await _activityLog.ActivityLogCreateWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.CREATE, entityId.ToString(), ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActivityCreateLog(createDto)
                    );
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> UpdateSysItem([FromBody] SysItemUpdateDto updateDto)
        {
            var response = new APIResponse();
            SysItem beforeUpdate = null;
            SysItem item = null;
            try
            {
                if (updateDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidData);
                    response.Status = false;
                    return Ok(response);
                }

                item = await _sysItemService.GetAsync(x => x.ItemId == updateDto.ItemId, asNoTracking: false);
                if (item == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NoDataFound);
                    response.Status = false;
                    return Ok(response);
                }

                // check duplicate ItemName
                var itemNameJson = JsonSerializer.Serialize(updateDto.ItemName, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                var itemNameExists = (await _sysItemService.GetListAsync(x => x.ItemId != updateDto.ItemId && x.ItemName == itemNameJson)).Any();
                if (itemNameExists)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_ItemNameAlreadyExists);
                    response.Status = false;
                    return Ok(response);
                }

                beforeUpdate = Utilities.ModelHelper.CloneModel(item);
                _mapper.Map(updateDto, item);
                VersioningModelHelper.SetUpdatedAudit(item, User.Identity.Name);

                await _sysItemService.UpdateAsync(item);

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_SavedSuccessfully);
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
                await _activityLog.ActivityLogEditWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, updateDto.ItemId, ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActiviUpdateLog(beforeUpdate, item)
                    );
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse>> DeleteSysItem([FromBody] List<SysItemIdRequestDto> requests)
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

                var itemIds = requests.Select(r => r.ItemId).ToList();
                var items = await _sysItemService.GetListAsync(x => itemIds.Contains(x.ItemId), includeProperties: "ItemAccessRights,UserItemFavorites");

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_DeletedSuccessfully);
                response.Status = true;

                foreach (var item in items)
                {
                    await _sysItemService.DeleteAsync(item);
                    await _activityLog.ActivityLogAsync(
                        LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, item.ItemId
                        , ControllerContext, CurrentUserInfo, response));
                }

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