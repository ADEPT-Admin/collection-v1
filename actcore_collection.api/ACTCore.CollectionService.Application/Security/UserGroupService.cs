using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;
using System.Linq.Expressions;
using EFCore.BulkExtensions;

namespace ACTCore.CollectionService.Application.Security
{
    public class UserGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SysUserGroup> AddAsync(SysUserGroup entity)
        {
            await _unitOfWork.Repository<SysUserGroup>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(SysUserGroup entity)
        {
            await _unitOfWork.Repository<SysUserGroup>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SysUserGroup>> GetListAsync(Expression<Func<SysUserGroup, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysUserGroup>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<SysUserGroup>> GetListPaginationAsync(Expression<Func<SysUserGroup, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<SysUserGroup>();

            if (string.IsNullOrEmpty(sortColumn))
                sortColumn = "UserGroupCode";

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<SysUserGroup> GetAsync(Expression<Func<SysUserGroup, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysUserGroup>().GetAsync(filter, includeProperties: "UserGroupAccesses,UserGroupAccesses.User,ItemAccessRights,ItemAccessRights.Item", asNoTracking: asNoTracking);
        }

        public async Task<SysUserGroup> GetByIdAsync(Guid id, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysUserGroup>().GetAsync(x => x.UserGroupId == id, includeProperties: "UserGroupAccesses,ItemAccessRights", asNoTracking: asNoTracking);
        }

        public async Task<SysUserGroup> UpdateAsync(SysUserGroup entity)
        {
            await _unitOfWork.Repository<SysUserGroup>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task<SysUserGroup> UpdateUserGroupDetailAsync(SysUserGroup userGroup, List<SysItemAccessRight> newAccesses)
        {
            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            try
            {

                var repoAccess = _unitOfWork.Repository<SysItemAccessRight>();
                var repoUserGroup = _unitOfWork.Repository<SysUserGroup>();

                await _unitOfWork.DbContext.Set<SysItemAccessRight>()
                    .Where(x => x.UserGroupId == userGroup.UserGroupId)
                    .ExecuteDeleteAsync();
                _unitOfWork.DbContext.ChangeTracker.Clear();

                if (newAccesses.Any())
                {
                    await repoAccess.AddRangeAsync(newAccesses);
                    await _unitOfWork.SaveChangesAsync();
                }

                foreach (var entry in _unitOfWork.DbContext.ChangeTracker.Entries<SysItemAccessRight>().ToList())
                {
                    entry.State = EntityState.Detached;
                }

                foreach (var entry in _unitOfWork.DbContext.ChangeTracker.Entries<SysUserGroupAccessRight>().ToList())
                {
                    entry.State = EntityState.Detached;
                }

                _unitOfWork.DbContext.Attach(userGroup);
                _unitOfWork.DbContext.Entry(userGroup).State = EntityState.Modified;

                await repoUserGroup.UpdateAsync(userGroup);
                await _unitOfWork.SaveChangesAsync();

                await tran.CommitAsync();

                return userGroup;
            }
            catch
            {
                await tran.RollbackAsync();
                _unitOfWork.DbContext.ChangeTracker.Clear();
                throw;
            }
        }
        public async Task<bool> DeleteRangeAsync(IEnumerable<SysUserGroup> entities)
        {
            await _unitOfWork.Repository<SysUserGroup>().DeleteRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // BulkExtension upsert user groups
        public async Task<bool> BulkMergeAsnc(IEnumerable<SysUserGroup> entities)
        {
            await using var tran = await _unitOfWork.DbContext.Database.BeginTransactionAsync();
            try
            {
                /* await _unitOfWork.DbContext.BulkInsertOrUpdateAsync(entities, bulkConfig: new BulkConfig
                {
                    PreserveInsertOrder = true,        // รักษาลำดับ insert เดิม
                    SetOutputIdentity = true,          // คืนค่า identity ที่เพิ่มเข้าใหม่
                    BatchSize = 5000,                  // กำหนดขนาด batch ต่อรอบ
                    UseTempDB = true,                  // ใช้ temp table (เร็วขึ้นใน bulk)
                    TrackingEntities = false,
                    
                });*/
                await _unitOfWork.DbContext.BulkInsertOrUpdateAsync(entities);
                await tran.CommitAsync();
                return true;
            }
            catch
            {
                await tran.RollbackAsync();
                _unitOfWork.DbContext.ChangeTracker.Clear();
                throw;
            }
        }

    }
}