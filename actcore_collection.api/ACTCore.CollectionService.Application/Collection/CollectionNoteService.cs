using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Collection
{
    public class CollectionNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CollectionNoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CollectionNote>> GetListAsync(Expression<Func<CollectionNote, bool>> filter = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<CollectionNote>()
                .GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<CollectionNote>> GetListPaginationAsync(Expression<Func<CollectionNote, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<CollectionNote>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }


        public async Task<CollectionNote> GetAsync(Expression<Func<CollectionNote, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<CollectionNote>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<CollectionNote> GetByIdAsync(Guid id, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<CollectionNote>().GetAsync(x => x.Id == id, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<CollectionNote> AddAsync(CollectionNote entity)
        {
            await _unitOfWork.Repository<CollectionNote>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<CollectionNote> UpdateAsync(CollectionNote entity)
        {
            await _unitOfWork.Repository<CollectionNote>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(CollectionNote entity)
        {
            await _unitOfWork.Repository<CollectionNote>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
