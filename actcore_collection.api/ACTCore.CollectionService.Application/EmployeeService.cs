using ACTCore.CollectionService.Domain.Entities.Profiles;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application
{
    public class EmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeProfile> AddAsync(EmployeeProfile entity)
        {
            await _unitOfWork.Repository<EmployeeProfile>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(EmployeeProfile entity)
        {
            await _unitOfWork.Repository<EmployeeProfile>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeProfile>> GetListAsync(Expression<Func<EmployeeProfile, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<EmployeeProfile>().GetAllAsync(filter
                , includeProperties: "Prefix",
                asNoTracking: asNoTracking
                );
        }

        public async Task<PagedResult<EmployeeProfile>> GetListPaginationAsync(Expression<Func<EmployeeProfile, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<EmployeeProfile>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<EmployeeProfile> GetAsync(Expression<Func<EmployeeProfile, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<EmployeeProfile>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking
            );
        }

        public async Task<EmployeeProfile> GetByIdAsync(string employeeId, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<EmployeeProfile>().GetAsync(x => x.EmployeeId == employeeId, includeProperties: "Prefix", asNoTracking: asNoTracking);
        }

        public async Task<EmployeeProfile> UpdateAsync(EmployeeProfile entity)
        {
            await _unitOfWork.Repository<EmployeeProfile>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
    }
}