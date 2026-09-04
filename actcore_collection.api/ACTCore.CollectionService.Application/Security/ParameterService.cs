using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Security
{
    public class ParameterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParameterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SysParameter> AddAsync(SysParameter entity)
        {
            await _unitOfWork.Repository<SysParameter>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(SysParameter entity)
        {
            await _unitOfWork.Repository<SysParameter>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SysParameter>> GetListAsync(Expression<Func<SysParameter, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysParameter>().GetAllAsync(filter, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<SysParameter>> GetListPaginationAsync(Expression<Func<SysParameter, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<SysParameter>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection,
                filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<SysParameter> GetAsync(Expression<Func<SysParameter, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysParameter>().GetAsync(filter, asNoTracking: asNoTracking);
        }

        public async Task<SysParameter> GetByIdAsync(int id, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysParameter>().GetAsync(x => x.Id == id, asNoTracking: asNoTracking);
        }

        public async Task<SysParameter> UpdateAsync(SysParameter entity)
        {
            await _unitOfWork.Repository<SysParameter>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
    }
}