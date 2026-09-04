using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Collection
{
    public class ColNoteResultService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColNoteResultService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColNoteResult>> GetListAsync(Expression<Func<ColNoteResult, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColNoteResult>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<ColNoteResult>> GetListPaginationAsync(Expression<Func<ColNoteResult, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<ColNoteResult>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }


        public async Task<ColNoteResult> GetAsync(Expression<Func<ColNoteResult, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColNoteResult>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<ColNoteResult> GetByIdAsync(int id, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColNoteResult>().GetAsync(x => x.ResultId == id, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<ColNoteResult> AddAsync(ColNoteResult entity)
        {
            await _unitOfWork.Repository<ColNoteResult>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<ColNoteResult> UpdateAsync(ColNoteResult entity)
        {
            await _unitOfWork.Repository<ColNoteResult>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(ColNoteResult entity)
        {
            await _unitOfWork.Repository<ColNoteResult>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
