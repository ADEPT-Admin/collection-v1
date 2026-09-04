using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Collection
{
    public class ColTeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColTeamService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColTeam>> GetListAsync(Expression<Func<ColTeam, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColTeam>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<ColTeam>> GetListPaginationAsync(Expression<Func<ColTeam, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<ColTeam>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }


        public async Task<ColTeam> GetAsync(Expression<Func<ColTeam, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColTeam>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<ColTeam> AddAsync(ColTeam entity)
        {
            await _unitOfWork.Repository<ColTeam>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<ColTeam> UpdateAsync(ColTeam entity)
        {
            await _unitOfWork.Repository<ColTeam>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(ColTeam entity)
        {
            await _unitOfWork.Repository<ColTeam>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<ColTeam> entities)
        {
            await _unitOfWork.Repository<ColTeam>().DeleteRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
