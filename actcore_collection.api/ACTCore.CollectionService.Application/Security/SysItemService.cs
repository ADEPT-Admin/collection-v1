using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Security
{
    public class SysItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SysItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SysItem> AddAsync(SysItem entity)
        {
            await _unitOfWork.Repository<SysItem>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(SysItem entity)
        {
            // delete child data
            var itemAccesses = entity.ItemAccessRights;
            if(itemAccesses.Any())
            {
                await _unitOfWork.Repository<SysItemAccessRight>().DeleteRangeAsync(itemAccesses);
            }

            var userItemFavorites = entity.UserItemFavorites;
            if (userItemFavorites.Any())
                await _unitOfWork.Repository<SysUserItemFavorite>().DeleteRangeAsync(userItemFavorites);

            await _unitOfWork.Repository<SysItem>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SysItem>> GetListAsync(Expression<Func<SysItem, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysItem>().GetAllAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking
            );
        }

        public async Task<PagedResult<SysItem>> GetListPaginationAsync(Expression<Func<SysItem, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<SysItem>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<SysItem> GetAsync(Expression<Func<SysItem, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysItem>().GetAsync(
                filter,
                includeProperties: includeProperties, //"ItemAccessRights,UserItemFavorites",
                asNoTracking: asNoTracking
            );
        }

        public async Task<SysItem> UpdateAsync(SysItem entity)
        {
            await _unitOfWork.Repository<SysItem>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
    }
}