using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application
{
    public class DistrictService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DistrictService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<District> AddAsync(District entity)
        {
            await _unitOfWork.Repository<District>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(District entity)
        {
            await _unitOfWork.Repository<District>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<District>> GetListAsync(Expression<Func<District, bool>> filter = null, bool asNoTracking = true)
        {
            // Include Province, SubDistricts, Areas
            return await _unitOfWork.Repository<District>().GetAllAsync(
                filter,
                includeProperties: "Province,SubDistricts,Areas",
                asNoTracking: asNoTracking
            );
        }

        public async Task<District> GetAsync(Expression<Func<District, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<District>().GetAsync(
                filter,
                includeProperties: "Province,SubDistricts,Areas",
                asNoTracking: asNoTracking
            );
        }

        public async Task<District> GetByIdAsync(string districtId, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<District>().GetAsync(
                x => x.DistrictId == districtId,
                includeProperties: "Province,SubDistricts,Areas",
                asNoTracking: asNoTracking
            );
        }

        public async Task<District> UpdateAsync(District entity)
        {
            await _unitOfWork.Repository<District>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
    }
}