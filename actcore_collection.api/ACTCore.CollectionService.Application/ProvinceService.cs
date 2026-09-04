using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application
{
    public class ProvinceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProvinceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Province> AddAsync(Province entity)
        {
            await _unitOfWork.Repository<Province>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Province entity)
        {
            await _unitOfWork.Repository<Province>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Province>> GetListAsync(Expression<Func<Province, bool>> filter = null, bool asNoTracking = true)
        {
            // Include all related properties
            return await _unitOfWork.Repository<Province>().GetAllAsync(
                filter,
                includeProperties: "Districts,Areas,Areas.District,Areas.SubDistrict,Districts.SubDistricts"
                , asNoTracking: asNoTracking
            );
        }

        public async Task<Province> GetAsync(Expression<Func<Province, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Province>().GetAsync(
                filter,
                includeProperties: "Districts,Areas,Areas.District,Areas.SubDistrict,Districts.SubDistricts",
                asNoTracking: asNoTracking
            );
        }

        public async Task<Province> GetByIdAsync(string provinceId, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Province>().GetAsync(
                x => x.ProvinceId == provinceId,
                includeProperties: "Districts,Areas,Areas.District,Areas.SubDistrict,Districts.SubDistricts",
                asNoTracking: asNoTracking
            );
        }

        public async Task<Province> UpdateAsync(Province entity)
        {
            await _unitOfWork.Repository<Province>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
    }
}