using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application
{
    public class WorklistHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorklistHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<WorklistHistory> AddAsync(WorklistHistory entity)
        {
            await _unitOfWork.Repository<WorklistHistory>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(WorklistHistory entity)
        {
            await _unitOfWork.Repository<WorklistHistory>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WorklistHistory>> GetListAsync(Expression<Func<WorklistHistory, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<WorklistHistory>().GetAllAsync(filter, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<WorklistHistory>> GetListPaginationAsync(Expression<Func<WorklistHistory, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            Dictionary<string, object> filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<WorklistHistory>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<WorklistHistory> GetAsync(Expression<Func<WorklistHistory, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<WorklistHistory>().GetAsync(filter, includeProperties: "", asNoTracking: asNoTracking);
        }

        public async Task<WorklistHistory> GetByIdAsync(Guid id, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<WorklistHistory>().GetAsync(x => x.Id == id, includeProperties: "", asNoTracking: asNoTracking);
        }

        public async Task<WorklistHistory> UpdateAsync(WorklistHistory entity)
        {
            await _unitOfWork.Repository<WorklistHistory>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<WorklistHistory> AddHistoryByContractAsync(Worklist workList, string userName)
        {
            WorklistHistory WorklistHistory = _mapper.Map<WorklistHistory>(workList);
            //VersioningModelHelper.SetCreatedAudit(WorklistHistory, userName);

            await _unitOfWork.Repository<WorklistHistory>().AddAsync(WorklistHistory);
            await _unitOfWork.SaveChangesAsync();
            return WorklistHistory;
        }
    }
}
