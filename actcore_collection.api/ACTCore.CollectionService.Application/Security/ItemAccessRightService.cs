
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Security
{
    public class ItemAccessRightService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemAccessRightService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<SysItemAccessRight> AddAsync(SysItemAccessRight entity)
        {
            await _unitOfWork.Repository<SysItemAccessRight>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> AddRangeAsync(IEnumerable<SysItemAccessRight> entities)
        {
            await _unitOfWork.Repository<SysItemAccessRight>().AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SysItemAccessRight>> GetListAsync(Expression<Func<SysItemAccessRight, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysItemAccessRight>().GetAllAsync(
                filter,
                includeProperties: "Item,UserGroup",
                asNoTracking: asNoTracking
            );
        }
    }
}
