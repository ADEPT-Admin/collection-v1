using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Models.Dto.ListPagedDto;
using ACTCore.CollectionService.API.Models.Dto.poc;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Helpers;
using SharedKernel.Models;
using System.Data;
using System.Linq.Expressions;
using System.Net;


namespace ACTCore.CollectionService.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserGroupController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _logger;
        private readonly IActivityLogService _activityLog;
        private readonly UserGroupService _userGroupService;
        private readonly SysItemService _sysItemService;
        private readonly ItemAccessRightService _itemAccessRightService;
        private readonly LanguageService _languageService;
        private readonly IUnitOfWork _unitOfWork;

        public UserGroupController(IMapper mapper, IErrorLogService logger, IActivityLogService activityLog,
            UserGroupService userGroupService,
            SysItemService sysItemService, ItemAccessRightService itemAccessRightService,
            LanguageService languageService, IUnitOfWork unitOfWork) : base(logger)
        {
            _mapper = mapper;
            _logger = logger;
            _activityLog = activityLog;
            _userGroupService = userGroupService;
            _sysItemService = sysItemService;
            _itemAccessRightService = itemAccessRightService;
            _languageService = languageService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("list")]
        public async Task<ActionResult<APIResponse>> GetListUserGroups([FromQuery] bool? status)
        {
            var response = new APIResponse();
            try
            {
                var userGroups = await _userGroupService.GetListAsync(x => !status.HasValue || x.IsActive == status);
                var userGroupDtos = _mapper.Map<IEnumerable<UserGroupResponseDto>>(userGroups).OrderBy(x => x.UserGroupName);

                response.Data = userGroupDtos;
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
        public async Task<ActionResult<APIResponse>> GetListUserGroupsPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var userGroups = await _userGroupService.GetListPaginationAsync(
                    filter: null,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn,
                    sortDirection: request.SortDirection,
                    filterColumns: request.Filters,
                    includeProperties: ""
                );

                List<UserGroupListPagedResponseDto> listPaged = new List<UserGroupListPagedResponseDto>();
                foreach (var item in userGroups.Items)
                {
                    var userGroup = new UserGroupListPagedResponseDto()
                    {
                    UserGroupId = item.UserGroupId,
                    UserGroupCode = item.UserGroupCode,
                    UserGroupName = item.UserGroupName,
                    Description = item.Description,
                    IsActive = item.IsActive ? IconType.CHECKGREEN
                        : IconType.CHECKRED
                    };

                    listPaged.Add(userGroup);
                }

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                var displayProperties = DataTableHelper.GetDisplayColumnAndDisplayProperties<UserGroupDisplayFilterDto>(enumValues);

                response = new ApiPaginationResponse()
                {
                    Data = new PaginationData<UserGroupListPagedResponseDto>
                    {
                        Datatables = listPaged,
                        DisplayColumns = displayProperties,
                        TotalRecords = userGroups.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)userGroups.TotalCount / request.PageSize),
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
        [HttpPost("search")]
        public async Task<ActionResult<APIResponse>> SearchUsers([FromBody] SearchRequestDto request)
        {
            var response = new APIResponse();
            try
            {
                IEnumerable<SysUserGroup> userGroups;
                if (request == null || string.IsNullOrEmpty(request.SearchBy) || string.IsNullOrEmpty(request.SearchValue))
                {
                    userGroups = await _userGroupService.GetListAsync();
                }
                else
                {
                    Expression<Func<SysUserGroup, bool>> filter = request.SearchBy.ToLower() switch
                    {
                        "code" => x => x.UserGroupCode.Contains(request.SearchValue),
                        "name" => x => x.UserGroupName.Contains(request.SearchValue),
                        _ => x => true
                    };
                    userGroups = await _userGroupService.GetListAsync(filter);
                }

                var userDtos = _mapper.Map<IEnumerable<SearchUserGroupResponseDto>>(userGroups);

                response.Data = userDtos;
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


        [HttpGet("get-by-id/{id?}")]
        public async Task<ActionResult<APIResponse>> Getbyid(Guid? id)
        {
            var response = new APIResponse();
            try
            {
                // Get only Template
                var permissionTemplates = await _sysItemService.GetListAsync(
                    x => x.IsActive == true,
                    includeProperties: "ItemAccessRights,ItemAccessRights.UserGroup,UserItemFavorites");
                var templateDtos = _mapper.Map<List<UserGroupMenuPermissionDto>>(permissionTemplates);
                SysUserGroup userGroup = null;

                // Get menu permission following userGroupId
                if (id != null)
                {
                    userGroup = await _userGroupService.GetAsync(g => g.UserGroupId == id);

                    var items = await _itemAccessRightService.GetListAsync(a => a.UserGroupId == id);
                    var permissionDtos = _mapper.Map<List<UserGroupMenuPermissionDto>>(items);

                    templateDtos
                        .Join(
                            permissionDtos,
                            template => template.ItemId,
                            permission => permission.ItemId,
                            (template, permission) => new { template, permission }
                        )
                        .ToList()
                        .ForEach(x =>
                        {
                            x.template.UserGroupId = x.permission.UserGroupId;
                            x.template.AllowAccess = x.permission.AllowAccess;
                            x.template.AllowView = x.permission.AllowView;
                            x.template.AllowNew = x.permission.AllowNew;
                            x.template.AllowEdit = x.permission.AllowEdit;
                            x.template.AllowDelete = x.permission.AllowDelete;
                        });
                }
                response.Data = new UserGroupPermissionResponseDto
                {
                    UserGroup = userGroup != null ? _mapper.Map<UserGroupResponseDto>(userGroup) : null,
                    MenuItems = templateDtos
                };
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

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> CreateUserGroup([FromBody] UserGroupCreateDto createDto)
        {
            var response = new APIResponse();
            var entityId = string.Empty;
            var tran = await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (createDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidData);
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // check duplicate code & name
                var dupGroup = await _userGroupService.GetAsync(x => x.UserGroupCode.ToLower() == createDto.UserGroup.UserGroupCode.ToLower()
                                                                || x.UserGroupName.ToLower() == createDto.UserGroup.UserGroupName.ToLower());
                if (dupGroup != null)
                {
                    var messages = new List<string>();
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserGroupCodeorNameExists);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // create a user group
                var userGroup = _mapper.Map<SysUserGroup>(createDto.UserGroup);
                VersioningModelHelper.SetCreatedAudit(userGroup, User.Identity.Name);
                await _userGroupService.AddAsync(userGroup);
                entityId = userGroup.UserGroupId.ToString();

                // set recursive for parent menu allowance
                PropagateMenuItemPermissions(createDto.MenuItems, userGroup.UserGroupId);

                var itemAccesses = _mapper.Map<List<SysItemAccessRight>>(createDto.MenuItems);
                itemAccesses.ForEach(a =>
                {
                    VersioningModelHelper.SetCreatedAudit(a, User.Identity.Name);
                });
                await _itemAccessRightService.AddRangeAsync(itemAccesses);
                await _unitOfWork.CommitTransactionAsync();

                response.Data = new UserGroupPermissionResponseDto
                {
                    UserGroup = _mapper.Map<UserGroupResponseDto>(userGroup),
                    MenuItems = _mapper.Map<List<UserGroupMenuPermissionDto>>(createDto.MenuItems)
                };

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_SavedSuccessfully);
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
                await _activityLog.ActivityLogCreateWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.CREATE, entityId.ToString(), ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActivityCreateLog(createDto)
                );
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> UpdateUserGroup([FromBody] UserGroupUpdateDto updateDto)
        {
            var response = new APIResponse();
            SysUserGroup userGroup = null;
            SysUserGroup beforeUpdate = null;

            try
            {
                if (updateDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidData);
                    response.Status = false;
                    return Ok(response);
                }

                // check duplicate code & name except itself
                var dupeCode = await _userGroupService.GetAsync(
                                x => (x.UserGroupCode.ToLower() == updateDto.UserGroup.UserGroupCode.ToLower()
                                        || x.UserGroupName.ToLower() == updateDto.UserGroup.UserGroupName.ToLower())
                                        && x.UserGroupId != updateDto.UserGroup.UserGroupId
                );

                if (dupeCode != null)
                {
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserGroupCodeorNameExists);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Status = false;
                    return Ok(response);
                }

                // check user group existing
                userGroup = await _userGroupService.GetAsync(x => x.UserGroupId == updateDto.UserGroup.UserGroupId);
                if (userGroup == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NoDataFound);
                    response.Status = false;
                    return Ok(response);
                }

                // update user group, delete and create new accesses 
                beforeUpdate = Utilities.ModelHelper.CloneModel(userGroup);
                _mapper.Map(updateDto.UserGroup, userGroup);

                // set recursive for parent menu allowance
                PropagateMenuItemPermissions(updateDto.MenuItems, userGroup.UserGroupId);

                var itemAccesses = _mapper.Map<List<SysItemAccessRight>>(updateDto.MenuItems);
                foreach (var menuItem in itemAccesses)
                {
                    VersioningModelHelper.SetCreatedAudit(menuItem, User.Identity.Name);
                }

                VersioningModelHelper.SetUpdatedAudit(userGroup, User.Identity.Name);
                await _userGroupService.UpdateUserGroupDetailAsync(userGroup, itemAccesses);

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
                   LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, updateDto.UserGroup.UserGroupId.ToString(), ControllerContext, CurrentUserInfo, response),
                   LoggerHelper.BuildActiviUpdateLog(beforeUpdate, userGroup)
                   );
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse>> DeleteUserGroup([FromBody] List<UserGroupRequestByIdDto> requests)
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

                var userGroupIds = requests.Select(r => r.UserGroupId).ToList();
                var userGroups = (await _userGroupService.GetListAsync(x => userGroupIds.Contains(x.UserGroupId), includeProperties: "UserGroupAccesses,ItemAccessRights", asNoTracking: false)).ToList();

                var notFoundIds = userGroupIds.Except(userGroups.Select(ug => ug.UserGroupId)).ToList();
                if (notFoundIds.Any())
                {
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_2ParamsNotFound, "User Group Id", string.Join(", ", notFoundIds));
                }

                // check user existing references in UserGroup
                var cannotDeleteGroups = userGroups.Where(ug => ug.UserGroupAccesses.Count != 0).ToList();
                if (cannotDeleteGroups.Any())
                {
                    var groupNames = string.Join(", ", cannotDeleteGroups
                        .Select(g => g.UserGroupName)
                        .Distinct()
                    );
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserGroupCannotDelete, groupNames);
                    await _activityLog.ActivityLogAsync(
                        userGroups.Select(x => LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, x.UserGroupId.ToString(),
                        ControllerContext, CurrentUserInfo, response)).ToList()
                    );
                }

                if (response.Message != null)
                {
                    response.Status = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // delete SysItemAccessRight
                var allAccessRightsToDelete = await _itemAccessRightService.GetListAsync(a => userGroupIds.Contains(a.UserGroupId), asNoTracking: false);
                if (allAccessRightsToDelete.Any())
                {
                    await _unitOfWork.Repository<SysItemAccessRight>().DeleteRangeAsync(allAccessRightsToDelete);
                    await _unitOfWork.SaveChangesAsync();
                }
                await _userGroupService.DeleteRangeAsync(userGroups);

                // delete UserGroup
                await _unitOfWork.CommitTransactionAsync();

                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_DeletedSuccessfully);
                response.StatusCode = HttpStatusCode.OK;

                await _activityLog.ActivityLogAsync(
                    requests.Select(x => LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, x.UserGroupId.ToString(),
                    ControllerContext, CurrentUserInfo, response)).ToList()
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add(ex.Message);
                await _activityLog.ActivityLogAsync(
                        LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, string.Empty
                        , ControllerContext, CurrentUserInfo, response));
                return await HandleExceptionAsync(response, ex);
            }
        }

        private void PropagateMenuItemPermissions(List<UserGroupMenuPermissionDto> menuItems, Guid userGroupId)
        {
            var menuItemsDict = menuItems.ToDictionary(x => x.ItemId);

            foreach (var item in menuItems)
            {
                // assign usergroupId
                item.UserGroupId = userGroupId;
                // assign allowance on parent
                if (item.AllowAccess || item.AllowView || item.AllowNew || item.AllowEdit || item.AllowDelete)
                {
                    var parentId = item.ParentId;
                    while (!string.IsNullOrEmpty(parentId) && menuItemsDict.TryGetValue(parentId, out var parentItem))
                    {
                        // union permissions
                        parentItem.AllowAccess |= item.AllowAccess;
                        parentItem.AllowView |= item.AllowView;
                        parentItem.AllowNew |= item.AllowNew;
                        parentItem.AllowEdit |= item.AllowEdit;
                        parentItem.AllowDelete |= item.AllowDelete;

                        parentId = parentItem.ParentId;
                    }
                }
            }

        }
    }
}