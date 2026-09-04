using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.Application.Collection
{
    public class ColAreaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColAreaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColArea>> GetListAsync(Expression<Func<ColArea, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColArea>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<ColArea>> GetListPaginationAsync(Expression<Func<ColArea, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<ColArea>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<ColArea> GetAsync(Expression<Func<ColArea, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<ColArea>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<ColArea> AddAsync(ColArea entity)
        {
            await _unitOfWork.Repository<ColArea>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<ColArea> UpdateAsync(ColArea entity)
        {
            await _unitOfWork.Repository<ColArea>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(ColArea entity)
        {
            await _unitOfWork.Repository<ColArea>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<ColArea> entities)
        {
            await _unitOfWork.Repository<ColArea>().DeleteRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}