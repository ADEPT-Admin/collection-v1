using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ValueObjects;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SharedKernel.CommonConstants;
using SharedKernel.Models;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text.Json;


namespace ACTCore.CollectionService.Application.Security
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ParameterService _parameterService;
        private readonly PasswordHistoryService _passwordHistoryService;
        private readonly TeamAssignmentService _teamAssignmentService;
        private readonly LanguageService _languageService;
        private readonly PasswordHasher<SysUser> _passwordHasher = new();

        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            ParameterService parameterService, PasswordHistoryService passwordHistoryService,
            TeamAssignmentService teamAssignmentService,
            LanguageService languageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _parameterService = parameterService;
            _passwordHistoryService = passwordHistoryService;
            _teamAssignmentService = teamAssignmentService;
            _languageService = languageService;
        }
        
        public async Task<SysUser> AddAsync(SysUser entity)
        {
            var defaultPassword = await GetDefaultPasswordValue();
            entity.PasswordHash = _passwordHasher.HashPassword(entity, defaultPassword);

            await _unitOfWork.Repository<SysUser>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<SysUser> entities)
        {
            var defaultPassword = await GetDefaultPasswordValue();
            foreach (var e in entities)
            {
                e.PasswordHash = _passwordHasher.HashPassword(e, defaultPassword);
            }
            await _unitOfWork.Repository<SysUser>().AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(SysUser entity)
        {
            // delte child tables whit UserId
            var userGroupAccesses = entity.UserGroupAccesses;
            if (userGroupAccesses.Any())
                await _unitOfWork.Repository<SysUserGroupAccessRight>().DeleteRangeAsync(userGroupAccesses);

            var userItemFavorites = await _unitOfWork.Repository<SysUserItemFavorite>()
                .GetAllAsync(x => x.UserId == entity.UserId);
            if (userItemFavorites.Any())
                await _unitOfWork.Repository<SysUserItemFavorite>().DeleteRangeAsync(userItemFavorites);

            var passwordHistories = await _unitOfWork.Repository<SysPasswordHistory>()
                .GetAllAsync(x => x.UserId == entity.UserId);
            if (passwordHistories.Any())
                await _unitOfWork.Repository<SysPasswordHistory>().DeleteRangeAsync(passwordHistories);

            await _unitOfWork.Repository<SysUser>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SysUser>> GetListAsync(Expression<Func<SysUser, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysUser>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }
        public async Task<PagedResult<SysUser>> GetListPaginationAsync(Expression<Func<SysUser, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<SysUser>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<SysUser> GetAsync(Expression<Func<SysUser, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysUser>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<SysUser> GetByIdAsync(Guid id, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysUser>().GetAsync(x => x.UserId == id, asNoTracking: asNoTracking);
        }

        public async Task<SysUser> UpdateAsync(SysUser entity)
        {
            await _unitOfWork.Repository<SysUser>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<SysUser> UpdateUserDetailAsync(SysUser user, List<SysUserGroupAccessRight> newAccesses)
        {
            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            try
            {
                var repoAccess = _unitOfWork.Repository<SysUserGroupAccessRight>();
                var repoUser = _unitOfWork.Repository<SysUser>();

                var existingAccesses = await repoAccess.GetAllAsync(
                    x => x.UserId == user.UserId,
                    asNoTracking: true
                );

                if (existingAccesses.Any())
                    await repoAccess.DeleteRangeAsync(existingAccesses);

                if (newAccesses.Any())
                    await repoAccess.AddRangeAsync(newAccesses);

                await repoUser.UpdateAsync(user);

                await _unitOfWork.SaveChangesAsync();

                await tran.CommitAsync();

                return user;
            }
            catch
            {
                await tran.RollbackAsync();
                _unitOfWork.DbContext.ChangeTracker.Clear();
                throw;
            }
        }

        public async Task<(bool Success, LanguageValue Message)> ChangePasswordAsync(SysUser entity, string oldPassword, string newPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(entity, entity.PasswordHash, oldPassword);
            if (result != PasswordVerificationResult.Success)
            {
                return (false, (await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_CurrentPasswordIncorrect)));

            }

            entity.PasswordHash = _passwordHasher.HashPassword(entity, newPassword);

            await _unitOfWork.Repository<SysUser>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var passwordHistory = new SysPasswordHistory
            {
                UserId = entity.UserId,
                UserName = entity.UserName,
                PasswordHash = entity.PasswordHash,
                CreatedBy = entity.UserName,
                CreatedDate = DateTime.Now,
                UpdatedBy = entity.UserName,
                UpdatedDate = DateTime.Now
            };
            await _unitOfWork.Repository<SysPasswordHistory>().AddAsync(passwordHistory);
            await _unitOfWork.SaveChangesAsync();

            return (true, await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_PasswordChangedSuccess));
        }

        public bool VerifyPasswordAsync(SysUser entity, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(entity, entity.PasswordHash, password);
            return (result != PasswordVerificationResult.Success);
        }

        public async Task<(bool Success, LanguageValue Message)> ChangeNewUserPasswordAsync(SysUser entity, string newPassword)
        {
            entity.PasswordHash = _passwordHasher.HashPassword(entity, newPassword);

            await _unitOfWork.Repository<SysUser>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var passwordHistory = new SysPasswordHistory
            {
                UserId = entity.UserId,
                UserName = entity.UserName,
                PasswordHash = entity.PasswordHash,
                CreatedBy = entity.UserName,
                CreatedDate = DateTime.Now,
                UpdatedBy = entity.UserName,
                UpdatedDate = DateTime.Now
            };
            await _unitOfWork.Repository<SysPasswordHistory>().AddAsync(passwordHistory);
            await _unitOfWork.SaveChangesAsync();

            return (true, await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_PasswordChangedSuccess));
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(SysUser entity)
        {
            entity.MissingHit = 0;
            entity.IsLockUser = false;
            entity.IsNewUser = true;
            entity.LastChangePasswordDate = DateTime.Now;

            var defaultPassword = await _parameterService.GetAsync(x => x.ParameterName == "DefaultPassword");
            if (defaultPassword != null && !string.IsNullOrEmpty(defaultPassword.ParameterValue))
                entity.PasswordHash = _passwordHasher.HashPassword(entity, defaultPassword.ParameterValue);
            else
                entity.PasswordHash = _passwordHasher.HashPassword(entity, "password");

            await _unitOfWork.Repository<SysUser>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return (true, "Password changed successfully.");
        }

        public async Task<UserPermissionDto> GetUserPermissionAsync(Expression<Func<SysUser, bool>> filter = null, bool asNoTracking = true)
        {
            // 1. Get user with groups and employee
            var user = await _unitOfWork.Repository<SysUser>()
                .GetAsync(filter, includeProperties: "UserGroupAccesses.UserGroup,Employee.Department,Employee.Position,Employee.Prefix", asNoTracking: asNoTracking);

            if (user == null)
                return null;

            // 2. Get all user groups for this user
            var userGroups = user.UserGroupAccesses
                .Where(ug => ug.IsActive && ug.UserGroup.IsActive)
                .Select(ug => ug.UserGroup)
                .ToList();

            // 3. Get all item access rights for these groups
            var groupIds = userGroups.Select(g => g.UserGroupId).ToList();
            var itemAccessRights = await _unitOfWork.Repository<SysItemAccessRight>()
                .GetAllAsync(x => groupIds.Contains(x.UserGroupId) && x.AllowAccess == true, includeProperties: "Item");

            // 4. Map item access rights to UserPermissionItemAccess
            var permissions = itemAccessRights
                .Where(x => x.Item != null && x.Item.IsActive)
                .Select(x => new UserPermissionItemAccess
                {
                    ItemId = x.ItemId,
                    ParentId = x.Item.ParentId,
                    ItemName = string.IsNullOrWhiteSpace(x.Item.ItemName)
                        ? null
                        : JsonSerializer.Deserialize<LanguageValue>(x.Item.ItemName),
                    RouteName = x.Item.RouteName,
                    ItemLevel = x.Item.ItemLevel,
                    ItemOrder = x.Item.ItemOrder,
                    Icon = x.Item.Icon,
                    ToolTip = x.Item.ToolTip,
                    IsActive = x.Item.IsActive,
                    Permission = new PermissionAllowanceDto
                    {
                        AllowAccess = x.AllowAccess,
                        AllowView = x.AllowView,
                        AllowNew = x.AllowNew,
                        AllowEdit = x.AllowEdit,
                        AllowDelete = x.AllowDelete
                    }
                })
                .ToList();

            List<UserPermissionItemAccess> userPermissionItemAccesses = new();
            var itemLevel1 = permissions.Where(p => p.ItemLevel == 1).OrderBy(p => p.ItemOrder).ThenBy(p => p.ItemId).ToList();
            foreach (var level1 in itemLevel1)
            {
                if (level1.IsActive)
                {
                    userPermissionItemAccesses.Add(level1);
                    var itemLevel2 = permissions.Where(p => p.ParentId == level1.ItemId).OrderBy(p => p.ItemOrder).ThenBy(p => p.ItemId).ToList();
                    foreach (var level2 in itemLevel2)
                    {
                        userPermissionItemAccesses.Add(level2);
                        var itemLevel3 = permissions.Where(p => p.ParentId == level2.ItemId).OrderBy(p => p.ItemOrder).ThenBy(p => p.ItemId).ToList();
                        foreach (var level3 in itemLevel3)
                        {
                            userPermissionItemAccesses.Add(level3);
                        }
                    }
                }
            }

            // 5. Map user groups to UserGroupDto (using AutoMapper if configured)
            var userGroupDto = userGroups.Any() ? _mapper.Map<List<UserGroupDto>>(userGroups) : null;

            // 6. Map employee to EmployeeDto (using AutoMapper if configured)
            var employeeDto = user.Employee != null ? _mapper.Map<EmployeeProfileDto>(user.Employee) : null;
            employeeDto.Department = user.Employee?.Department != null ? _mapper.Map<DepartmentDto>(user.Employee.Department) : null;
            employeeDto.Position = user.Employee?.Position != null ? _mapper.Map<PositionDto>(user.Employee.Position) : null;


            var today = DateTime.Now.Date;
            var teamAssignment = await _teamAssignmentService.GetAsync(x =>
                x.Collector.UserId == user.UserId
                && (x.EffectiveDate == null || x.EffectiveDate.Value.Date <= today)
                && (x.ExpireDate == null || x.ExpireDate.Value.Date >= today)
            );

            bool isSupervisor = teamAssignment != null ? teamAssignment.IsSupervisor : false;

            // 8. Build and return result
            return new UserPermissionDto
            {
                UserID = user.UserId,
                IsCollectorSupervisor = isSupervisor,
                employee = employeeDto,
                UserGroup = userGroupDto,
                MenuItem = userPermissionItemAccesses
            };
        }

        public async Task<bool> IsPasswordInHistoryAsync(SysUser entity, string newPassword)
        {
            var passwordHistories = await _passwordHistoryService.GetListThrouPolicyAsync(x => x.UserId == entity.UserId);
            foreach (var history in passwordHistories)
            {
                var result = _passwordHasher.VerifyHashedPassword(entity, history.PasswordHash, newPassword);
                if (result == PasswordVerificationResult.Success)
                {
                    return true; // พบรหัสผ่านในประวัติ
                }
            }
            return false; // ไม่พบรหัสผ่านในประวัติ
        }

        private async Task<string> GetDefaultPasswordValue()
        {
            var param = await _parameterService.GetAsync(
                x => x.ParameterName == "DefaultPassword");

            return !string.IsNullOrWhiteSpace(param?.ParameterValue)
                ? param.ParameterValue
                : "password";
        }

    }
}
