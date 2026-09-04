using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Extensions;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Models.Dto.ListPagedDto;
using ACTCore.CollectionService.API.Models.Dto.LogDetailDto;
using ACTCore.CollectionService.API.Models.Dto.poc;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ValueObjects;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Helpers;
using SharedKernel.Models;
using System.Linq.Expressions;
using System.Net;

namespace ACTCore.CollectionService.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly IActivityLogService _activityLog;
        private readonly UserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PolicyService _policyService;
        private readonly LanguageService _languageService;
        private readonly UserGroupService _userGroupService;
        private readonly CollectorService _collectorService;
        private readonly EmployeeService _employeeService;

        public UserController(IMapper mapper, IErrorLogService errorLog, IActivityLogService activityLog,
            UserService userService, IUnitOfWork unitOfWork, EmployeeService employeeService,
            PolicyService policyService, LanguageService languageService, UserGroupService userGroupService,
            CollectorService collectorService
            ) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _activityLog = activityLog;
            _userService = userService;
            _policyService = policyService;
            _languageService = languageService;
            _userGroupService = userGroupService;
            _collectorService = collectorService;
            _unitOfWork = unitOfWork;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Get list data with pagination
        /// </summary>
        /// <returns></returns>
        [HttpPost("list-paged")]
        public async Task<ActionResult<APIResponse>> GetListUsers([FromBody] PagedRequest<FilterContainer> request, [FromQuery] bool isActiveUser = false)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var isDialogRequest = !String.IsNullOrEmpty(request.Mode)
                    && String.Equals(request.Mode, ModeType.Dialog, StringComparison.OrdinalIgnoreCase);
                var filter = BuildExpressionFilter(request, isActiveUser || isDialogRequest);
                request.SortColumn = request.SortColumn switch
                {
                    "EmployeeName" => "Employee.FullName",
                    "UserGroup" => "UserGroupAccesses.UserGroup.UserGroupName",
                    "Email" => "Employee.Email",
                    "PhoneNo" => "Employee.PhoneNo",
                    _ => request.SortColumn
                };

                var users = await _userService.GetListPaginationAsync(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? "UserName",
                    sortDirection: request.SortDirection ?? "asc",
                    filterColumns: request.Filters,
                    includeProperties: "UserGroupAccesses.UserGroup,Employee,Employee.Prefix"
                );

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                response = new ApiPaginationResponse()
                {
                    Data = isDialogRequest
                        ? PaginationDataHelper.BuildPaginationData<UserDialogDisplayFilterDto, UserDialogListPagedResponseDto, SysUser>(users, request.PageNumber, request.PageSize, enumValues, _mapper)
                        : PaginationDataHelper.BuildPaginationData<UsersDisplayFilterDto, UserListPagedResponseDto, SysUser>(users, request.PageNumber, request.PageSize, enumValues, _mapper),
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
                var user = await _userService.GetAsync(x => x.UserId == id, 
                    includeProperties: "UserGroupAccesses.UserGroup,Employee,Employee.Department,Employee.Position,Employee.Prefix");
                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User");
                    response.Status = false;

                    return Ok(response);
                }

                var userdto = new UserResponseDto()
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    IsActive = user.IsActive,
                    IsLockUser = user.IsLockUser,
                    IsNewUser = user.IsNewUser,
                    EffectiveDate = user.EffectiveDate,
                    ExpireDate = user.ExpireDate,
                    LastSignOnDate = user.LastSignOnDate,
                    LastChangePasswordDate = user.LastChangePasswordDate,
                    UpdatedBy = user.UpdatedBy,
                    UpdatedDate = user.UpdatedDate,
                    Employee = user.Employee != null ? _mapper.Map<EmployeeResponseDto>(user.Employee) : null,
                    UserGroups = user.UserGroupAccesses != null
                                ? user.UserGroupAccesses
                                    .Where(ug => ug.UserGroup != null)
                                    .Select(ug => new UserGroupResponseDto
                                    {
                                        UserGroupId = ug.UserGroup.UserGroupId,
                                        UserGroupCode = ug.UserGroup.UserGroupCode,
                                        UserGroupName = ug.UserGroup.UserGroupName,
                                        IsActive = ug.IsActive
                                    }).ToList()
                                : new List<UserGroupResponseDto>()
                };
                response.Data = userdto;
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

        [HttpGet("get-user")]
        public async Task<ActionResult<APIResponse>> GetCurrentUser()
        {
            var response = new APIResponse();
            try
            {
                var user = await _userService.GetAsync(x => x.UserId == CurrentUserInfo.UserId, includeProperties: "UserGroupAccesses.UserGroup,Employee,Employee.Prefix");
                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User");
                    response.Status = false;

                    return Ok(response);
                }

                var userdto = new UserResponseDto()
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    IsActive = user.IsActive,
                    IsLockUser = user.IsLockUser,
                    IsNewUser = user.IsNewUser,
                    ExpireDate = user.ExpireDate,
                    LastSignOnDate = user.LastSignOnDate,
                    LastChangePasswordDate = user.LastChangePasswordDate,
                    Employee = user.Employee != null ? _mapper.Map<EmployeeResponseDto>(user.Employee) : null,
                    UserGroups = user.UserGroupAccesses != null
                                ? user.UserGroupAccesses
                                    .Where(ug => ug.UserGroup != null)
                                    .Select(ug => new UserGroupResponseDto
                                    {
                                        UserGroupId = ug.UserGroup.UserGroupId,
                                        UserGroupCode = ug.UserGroup.UserGroupCode,
                                        UserGroupName = ug.UserGroup.UserGroupName,
                                        IsActive = ug.IsActive
                                    }).ToList()
                                : new List<UserGroupResponseDto>()
                };
                response.Data = userdto;
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

        [HttpGet("get-user-permission")]
        public async Task<ActionResult<APIResponse>> GetCurrentUserPerMission()
        {
            var response = new APIResponse();
            try
            {
                var userPermission = await _userService.GetUserPermissionAsync(x => x.UserId == CurrentUserInfo.UserId);
                if (userPermission == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User");
                    response.Status = false;
                    return Ok(response);
                }

                response.Data = userPermission;
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(response, ex);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> CreateUser([FromBody] UserCreateDto createDto)
        {
            var response = new APIResponse();
            var entityId = "";
            try
            {
                // check empleyee already added
                if (createDto.EmployeeId != null)
                {
                    if (await _userService.GetAsync(x => x.EmployeeId == createDto.EmployeeId) != null)
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_EmployeeAlreadyUse);
                        response.Status = false;
                        return Ok(response);
                    }
                }

                // check duplicate user name
                if (await _userService.GetAsync(x => x.UserName.ToLower() == createDto.UserName.ToLower()) != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UsernameExist);
                    response.Status = false;
                    return Ok(response);
                }

                var user = _mapper.Map<SysUser>(createDto);

                // add usergroups
                foreach (var userGroupId in createDto.UserGroupIds)
                {
                    SysUserGroupAccessRight sysUserGroupAcess = new SysUserGroupAccessRight
                    {
                        UserId = user.UserId,
                        UserGroupId = userGroupId,
                        IsActive = true,
                    };
                    VersioningModelHelper.SetCreatedAudit(sysUserGroupAcess, User.Identity.Name);
                    user.UserGroupAccesses.Add(sysUserGroupAcess);
                }

                VersioningModelHelper.SetCreatedAudit(user, User.Identity.Name);
                await _userService.AddAsync(user);

                var newUser = await _userService.GetAsync(x => x.UserId == user.UserId, includeProperties: "UserGroupAccesses.UserGroup,Employee,Employee.Prefix");
                var userdto = new UserResponseDto()
                {
                    UserId = newUser.UserId,
                    UserName = newUser.UserName,
                    IsActive = newUser.IsActive,
                    IsLockUser = newUser.IsLockUser,
                    IsNewUser = newUser.IsNewUser,
                    ExpireDate = newUser.ExpireDate,
                    LastSignOnDate = newUser.LastSignOnDate,
                    LastChangePasswordDate = newUser.LastChangePasswordDate,
                    Employee = newUser.Employee != null ? _mapper.Map<EmployeeResponseDto>(newUser.Employee) : null,
                    UserGroups = newUser.UserGroupAccesses != null
                                ? newUser.UserGroupAccesses
                                    .Where(ug => ug.UserGroup != null)
                                    .Select(ug => new UserGroupResponseDto
                                    {
                                        UserGroupId = ug.UserGroup.UserGroupId,
                                        UserGroupCode = ug.UserGroup.UserGroupCode,
                                        UserGroupName = ug.UserGroup.UserGroupName,
                                        IsActive = ug.IsActive
                                    }).ToList()
                                : new List<UserGroupResponseDto>()
                };

                entityId = userdto.UserId.ToString();

                response.Data = _mapper.Map<UserResponseDto>(userdto);
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_SavedSuccessfully);
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
                await _activityLog.ActivityLogCreateWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.CREATE, entityId, ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActivityCreateLog(createDto)
                    );
            }
        }

        [HttpPost("bulk-create")]
        public async Task<ActionResult<APIResponse>> BulkCreateUser([FromBody] UserBulkCreateDto createDto)
        {
            IEnumerable<SysUser> users = null;
            var tran = await _unitOfWork.BeginTransactionAsync();
            var response = new APIResponse();
            try
            {
                // Validate empty employee ids
                if (createDto.EmployeeIds == null || createDto.EmployeeIds.Count == 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_AtLeastOneRequired, nameof(createDto.EmployeeIds));
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // Validate effective date
                if (createDto.ExpireDate.HasValue && createDto.EffectiveDate >= createDto.ExpireDate)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_EffectiveDateMustBeLessThanExpireDate);
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // Validate employee not found
                var employees = await _employeeService.GetListAsync(x => createDto.EmployeeIds.Contains(x.EmployeeId));
                if (employees.Count() != createDto.EmployeeIds.Count)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, nameof(createDto.EmployeeIds));
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // Validate active user group
                var userGroup = await _userGroupService.GetAsync(x => x.UserGroupId == createDto.UserGroupId && x.IsActive == true);
                if (userGroup == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_Inactive, "User Group");
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // Validate empleyees are already added in the SysUser
                users = await _userService.GetListAsync(
                    x => createDto.EmployeeIds.Contains(x.EmployeeId));

                if (users.Any())
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                        Message.Msg_EmployeeIdsAlreadyUse,
                        string.Join(",", users.Select(x => x.EmployeeId)));
                    response.Status = false;
                    await _unitOfWork.RollbackTransactionAsync();
                    return Ok(response);
                }

                // 1. Add Users from the Emp. list
                users = employees.Select( emp =>
                    new SysUser
                    {
                        UserName = emp.UserName,
                        EmployeeId = emp.EmployeeId,
                        IsActive = createDto.IsActive,
                        IsNewUser = createDto.ForceChangePassword,
                        EffectiveDate = createDto.EffectiveDate,
                        ExpireDate = createDto.ExpireDate
                    }).ToList();

                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_SavedSuccessfully);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;

                var logs = new List<ActivityLog>();
                foreach (var user in users)
                {
                    // 2. Add UserGroup Access Right
                    var sysUserGroupAcess = new SysUserGroupAccessRight
                    {
                        UserId = user.UserId,
                        UserGroupId = createDto.UserGroupId,
                        IsActive = createDto.IsActive,

                    };

                    user.UserGroupAccesses.Add(sysUserGroupAcess);

                    VersioningModelHelper.SetCreatedAudit(sysUserGroupAcess, User.Identity.Name);
                    VersioningModelHelper.SetCreatedAudit(user, User.Identity.Name);

                    var log = LoggerHelper.BuildActivityLog(ActivityLogAction.CREATE, user.UserId.ToString(), ControllerContext, CurrentUserInfo, response);

                    LoggerHelper.SetActivityLogDetail( log,
                        LoggerHelper.BuildActivityCreateLog( new LogDetailUserBulkCreateDto
                        {
                            UserGroupId = createDto.UserGroupId,
                            EffectiveDate = createDto.EffectiveDate,
                            ExpireDate = createDto.ExpireDate,
                            IsActive = createDto.IsActive,
                            ForceChangePassword = createDto.ForceChangePassword,
                            EmployeeId = user.EmployeeId
                        })
                    );

                    logs.Add(log);
                }
                await _userService.AddRangeAsync(users);
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

        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> UpdateUser([FromBody] UserUpdateDto updateDto)
        {
            var response = new APIResponse();
            SysUser updateUser = null;
            SysUser beforUpdate = null;
            try
            {
                // check user exists
                updateUser = await _userService.GetAsync(x => x.UserId == updateDto.UserId);
                if (updateUser == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User");
                    response.Status = false;
                    return Ok(response);
                }

                // check duplicate user name
                if (await _userService.GetAsync(x => x.UserName.ToLower() == updateDto.UserName.ToLower() && x.UserId != updateDto.UserId) != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UsernameExist);
                    response.Status = false;
                    return Ok(response);
                }

                beforUpdate = Utilities.ModelHelper.CloneModel(updateUser);
                _mapper.Map(updateDto, updateUser);

                var newAccesses = updateDto.UserGroupIds
                    .Distinct() // ป้องกัน duplicate
                    .Select(userGroupId => new SysUserGroupAccessRight
                    {
                        UserId = updateUser.UserId,
                        UserGroupId = userGroupId,
                        IsActive = true,
                    })
                    .ToList();

                foreach (var access in newAccesses)
                {
                    VersioningModelHelper.SetCreatedAudit(access, User.Identity.Name);
                }
                VersioningModelHelper.SetUpdatedAudit(updateUser, User.Identity.Name);
                await _userService.UpdateUserDetailAsync(updateUser, newAccesses);

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
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, updateDto.UserId.ToString(), ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActiviUpdateLog(beforUpdate, updateUser)
                    );
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse>> DeleteUser([FromBody] List<UserRequestDto> requests)
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

                var userIds = requests.Select(r => r.UserId).ToList();
                var users = await _userService.GetListAsync(x => userIds.Contains(x.UserId), includeProperties: "UserGroupAccesses");

                var notFoundIds = userIds.Except(users.Select(u => u.UserId)).ToList();
                if (notFoundIds.Any())
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_2ParamsNotFound, "User Id", string.Join(", ", notFoundIds));

                var IsOwnId = userIds.Contains(CurrentUserInfo.UserId);
                if (IsOwnId)
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CannotDeleteOwnAccount);

                var collectors = await _collectorService.GetListAsync(x => userIds.Contains(x.UserId), includeProperties: "User");
                if (collectors.Any())
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CannotDeleteUserWithExistCollectorFromUserName, string.Join(", ", collectors.Select(c => c.User.UserName)));

                if (response.Message != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Status = false;
                    return Ok(response);
                }

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_DeletedSuccessfully);
                response.Status = true;

                foreach (var item in users)
                {
                    await _userService.DeleteAsync(item);
                    await _activityLog.ActivityLogAsync(
                        LoggerHelper.BuildActivityLog(ActivityLogAction.DELETE, item.UserId.ToString()
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

        [HttpPost("change-password")]
        public async Task<ActionResult<APIResponse>> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            var response = new APIResponse();
            try
            {
                // check user if exists
                var user = await _userService.GetAsync(x => x.UserName == CurrentUser.Identity.Name);
                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User");
                    response.Status = false;
                    return Ok(response);
                }

                // check new password and confirm password
                if (request.NewPassword != request.ConfirmPassword)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NewandConfirmPasswordNotMatch);
                    response.Status = false;
                    return Ok(response);
                }

                // check current password
                if (_userService.VerifyPasswordAsync(user, request.CurrentPassword))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CurrentPasswordIncorrect);
                    response.Status = false;
                    return Ok(response);
                }

                // check password policy
                var checkPasswordPolicy = await _policyService.ValidatePasswordPolicy(CurrentUserInfo.UserName, request.NewPassword);
                if (!checkPasswordPolicy.Valid)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = checkPasswordPolicy.Message;
                    response.Status = false;
                    return Ok(response);
                }

                // check password history
                var isInHistory = await _userService.IsPasswordInHistoryAsync(user, request.NewPassword);
                if (isInHistory)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NewPassWordCannotBeTheSame);
                    response.Status = false;
                    return Ok(response);
                }

                // change password
                var result = await _userService.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                if (!result.Success)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Status = false;
                    response.Message = result.Message;
                    return Ok(response);
                }

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_PasswordChangedSuccessfully);
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
                    LoggerHelper.BuildActivityLog(ActivityLogAction.CHANGE_PASSWPRD, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response, "User change password")
                    );
            }
        }

        [HttpPost("change-password-new-user")]
        public async Task<ActionResult<APIResponse>> ChangeNewUserPassword([FromBody] ChangeNewUserPasswordRequestDto request)
        {
            var response = new APIResponse();
            try
            {
                var user = await _userService.GetAsync(x => x.UserName == CurrentUser.Identity.Name, asNoTracking: false);
                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User");
                    response.Status = false;
                    return Ok(response);
                }

                if (request.NewPassword != request.ConfirmPassword)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NewandConfirmPasswordNotMatch);

                    response.Status = false;
                    return Ok(response);
                }

                var checkPasswordPolicy = await _policyService.ValidatePasswordPolicy(CurrentUserInfo.UserName, request.NewPassword);
                if (!checkPasswordPolicy.Valid)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = checkPasswordPolicy.Message;
                    response.Status = false;
                    return Ok(response);
                }

                var result = await _userService.ChangeNewUserPasswordAsync(user, request.NewPassword);
                if (!result.Success)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Status = false;
                    response.Message = result.Message;
                    return Ok(response);
                }

                user.IsNewUser = false;
                VersioningModelHelper.SetUpdatedAudit(user, User.Identity.Name);
                await _userService.UpdateAsync(user);

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_PasswordChangedSuccessfully);
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = new LanguageValue()
                {
                    En = ex.Message,
                    Th = ex.Message
                };
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.CHANGE_PASSWPRD, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response, "Change new user password")
                    );
            }
        }

        [HttpPost("unlock-user")]
        public async Task<ActionResult<APIResponse>> UnlockUser([FromBody] UserRequestDto request)
        {
            var response = new APIResponse();
            try
            {
                var user = await _userService.GetAsync(x => x.UserId == request.UserId);
                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User");
                    response.Status = false;
                    return Ok(response);
                }

                user.IsLockUser = false;
                user.MissingHit = 0;
                VersioningModelHelper.SetUpdatedAudit(user, User.Identity.Name);
                await _userService.UpdateAsync(user);

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserUnlockedSuccessfully);
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = new LanguageValue()
                {
                    En = ex.Message,
                    Th = ex.Message
                };
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UNLOCK_USER, request.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<APIResponse>> ResetPassword([FromBody] UserRequestDto request)
        {
            var response = new APIResponse();
            try
            {
                var user = await _userService.GetAsync(x => x.UserId == request.UserId);
                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User");
                    response.Status = false;
                    return Ok(response);
                }

                await _userService.ResetPasswordAsync(user);

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_PasswordResetSuccessfully);
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = new LanguageValue()
                {
                    En = ex.Message,
                    Th = ex.Message
                };
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.RESET_PASSWORD, request.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpPost("validate-password-policy")]
        public async Task<ActionResult<APIResponse>> validatePasswordPolicy([FromBody] SysPolicyValidatePwdRequestDto validateDto)
        {
            var response = new APIResponse();
            try
            {
                SysUser user = await _userService.GetAsync(x => x.UserId == CurrentUserInfo.UserId, asNoTracking: false);
                if (user == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User");
                    return Ok(response);
                }

                SysPolicyValidatePwdPolicyResponseDto results = await _policyService.ValidatePasswordPolicy(user.UserName, validateDto.NewPwd, asNoTracking: false);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = results.Valid;
                response.Message = results.Message;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(response, ex);
            }
        }

        private static Expression<Func<SysUser, bool>> BuildExpressionFilter(PagedRequest<FilterContainer> request, bool isActiveUser)
        {
            Expression<Func<SysUser, bool>> filter = x => !isActiveUser || x.IsActive == isActiveUser;
            
            if (request.Filters?.DynamicFilters != null)
            {
                foreach (var dynamicFilter in request.Filters.DynamicFilters)
                {
                    string key = dynamicFilter.Key;
                    string value = dynamicFilter.Value?.returnStringValue();

                    if (value == null || string.IsNullOrWhiteSpace(value)) continue;
                    // usergroup
                    if (String.Equals(key, nameof(UserDialogListPagedResponseDto.UserGroup), StringComparison.OrdinalIgnoreCase)
                        && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<SysUser, bool>> groupFilter = u =>
                            u.UserGroupAccesses.Any(uga => uga.UserGroup.UserGroupName.Contains(value));
                        filter = FilterExtensions.AndAlso(filter, groupFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // employeename
                    else if (String.Equals(key, nameof(UserDialogListPagedResponseDto.EmployeeName), StringComparison.OrdinalIgnoreCase)
                        && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<SysUser, bool>> empFilter = u =>
                            u.Employee != null &&
                            (
                                string.Join(" ",
                                    new[] {u.Employee.Prefix != null ? u.Employee.Prefix.PrefixName : "",
                                u.Employee.FirstName ?? "",
                                u.Employee.LastName ?? "" }
                                ).Contains(value)
                            );
                        filter = FilterExtensions.AndAlso(filter, empFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // Emai
                    else if (String.Equals(key, nameof(UserDialogListPagedResponseDto.Email), StringComparison.OrdinalIgnoreCase)
                        && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<SysUser, bool>> emailFilter = u =>
                            u.Employee != null &&
                            u.Employee.Email.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, emailFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    //PhoneNo
                    else if (String.Equals(key, nameof(UserDialogListPagedResponseDto.PhoneNo), StringComparison.OrdinalIgnoreCase)
                        && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<SysUser, bool>> phoneFilter = u =>
                            u.Employee != null &&
                            u.Employee.PhoneNo.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, phoneFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                }
            }

            return filter;
        }
    }
}
