using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Collection
{
    public class ColNoteActionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColNoteActionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColNoteAction>> GetListAsync(Expression<Func<ColNoteAction, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColNoteAction>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<ColNoteAction>> GetListPaginationAsync(Expression<Func<ColNoteAction, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<ColNoteAction>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }


        public async Task<ColNoteAction> GetAsync(Expression<Func<ColNoteAction, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColNoteAction>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<ColNoteAction> GetByIdAsync(int id, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColNoteAction>().GetAsync(x => x.ActionId == id, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<ColNoteAction> AddAsync(ColNoteAction entity)
        {
            await _unitOfWork.Repository<ColNoteAction>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<ColNoteAction> UpdateAsync(ColNoteAction entity)
        {
            await _unitOfWork.Repository<ColNoteAction>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(ColNoteAction entity)
        {
            await _unitOfWork.Repository<ColNoteAction>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
