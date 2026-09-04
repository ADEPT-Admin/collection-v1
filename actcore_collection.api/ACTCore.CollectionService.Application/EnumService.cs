using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application
{
    public class EnumService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EnumService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SysEnum> AddAsync(SysEnum entity)
        {
            await _unitOfWork.Repository<SysEnum>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(SysEnum entity)
        {
            await _unitOfWork.Repository<SysEnum>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SysEnum>> GetListAsync(Expression<Func<SysEnum, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysEnum>().GetAllAsync(
                filter,
                asNoTracking: asNoTracking
            );
        }

        public async Task<PagedResult<SysEnum>> GetListPaginationAsync(Expression<Func<SysEnum, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            Dictionary<string, object> filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<SysEnum>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<SysEnum> GetAsync(Expression<Func<SysEnum, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysEnum>().GetAsync(
                filter,
                asNoTracking: asNoTracking
            );
        }

        public async Task<SysEnum> GetByIdAsync(int enumId)
        {
            return await _unitOfWork.Repository<SysEnum>().GetAsync(x => x.Id == enumId);
        }

        public async Task<SysEnum> UpdateAsync(SysEnum entity)
        {
            await _unitOfWork.Repository<SysEnum>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
    }
}