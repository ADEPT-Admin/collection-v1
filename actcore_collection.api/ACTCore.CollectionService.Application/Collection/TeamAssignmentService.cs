using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Collection
{
    public class TeamAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamAssignmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColTeamAssignment>> GetListAsync(Expression<Func<ColTeamAssignment, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColTeamAssignment>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<ColTeamAssignment>> GetListPaginationAsync(Expression<Func<ColTeamAssignment, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<ColTeamAssignment>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<ColTeamAssignment> GetAsync(Expression<Func<ColTeamAssignment, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColTeamAssignment>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<ColTeamAssignment> GetActiveColTeamAssignment(Guid userId, DateTime today, bool isSupervisor = false, Guid? colTeamId = null)
        {
            Expression<Func<ColTeamAssignment, bool>> filter = x =>
                    x.Collector.UserId == userId
                    && x.Collector.IsActive == true

                    && (x.EffectiveDate != null && x.EffectiveDate.Value.Date <= today)
                    && (x.ExpireDate == null || x.ExpireDate.Value.Date >= today)
                    
                    && x.IsActive == true

                    && x.ColTeam.IsActive == true
                    && (colTeamId == null || x.ColTeamId == colTeamId)

                    && (!isSupervisor || x.IsSupervisor == true);

            return await _unitOfWork.Repository<ColTeamAssignment>().GetAsync(filter, includeProperties:"", asNoTracking: true);
        }

        public async Task<ColTeamAssignment> AddAsync(ColTeamAssignment entity)
        {
            await _unitOfWork.Repository<ColTeamAssignment>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<ColTeamAssignment>> AddRangeAsync(IEnumerable<ColTeamAssignment> entities)
        {
            await _unitOfWork.Repository<ColTeamAssignment>().AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return entities;
        }

        public async Task<ColTeamAssignment> UpdateAsync(ColTeamAssignment entity)
        {
            await _unitOfWork.Repository<ColTeamAssignment>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(ColTeamAssignment entity)
        {
            await _unitOfWork.Repository<ColTeamAssignment>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<ColTeamAssignment> entities)
        {
            await _unitOfWork.Repository<ColTeamAssignment>().DeleteRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

