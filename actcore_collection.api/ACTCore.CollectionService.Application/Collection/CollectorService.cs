using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Collection
{
    public class CollectorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CollectorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CollectorProfile>> GetListAsync(Expression<Func<CollectorProfile, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<CollectorProfile>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<CollectorProfile>> GetListPaginationAsync(Expression<Func<CollectorProfile, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<CollectorProfile>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }


        public async Task<CollectorProfile> GetAsync(Expression<Func<CollectorProfile, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<CollectorProfile>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<CollectorProfile> GetByIdAsync(Guid id, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<CollectorProfile>().GetAsync(x => x.CollectorId == id, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<CollectorProfile> AddAsync(CollectorProfile entity)
        {
            await _unitOfWork.Repository<CollectorProfile>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<CollectorProfile>> AddRangeAsync(IEnumerable<CollectorProfile> entities)
        {
            await _unitOfWork.Repository<CollectorProfile>().AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return entities;
        }

        public async Task<CollectorProfile> UpdateAsync(CollectorProfile entity)
        {
            await _unitOfWork.Repository<CollectorProfile>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(CollectorProfile entity)
        {
            await _unitOfWork.Repository<CollectorProfile>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<CollectorProfile> entities)
        {
            await _unitOfWork.Repository<CollectorProfile>().DeleteRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
