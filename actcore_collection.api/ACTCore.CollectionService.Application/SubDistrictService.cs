using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application
{
    public class SubDistrictService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubDistrictService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubDistrict> AddAsync(SubDistrict entity)
        {
            await _unitOfWork.Repository<SubDistrict>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(SubDistrict entity)
        {
            await _unitOfWork.Repository<SubDistrict>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SubDistrict>> GetListAsync(Expression<Func<SubDistrict, bool>> filter = null, bool asNoTracking = true)
        {
            // Include District, Areas
            return await _unitOfWork.Repository<SubDistrict>().GetAllAsync(
                filter,
                includeProperties: "District,Areas",
                asNoTracking: asNoTracking
            );
        }

        public async Task<SubDistrict> GetAsync(Expression<Func<SubDistrict, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SubDistrict>().GetAsync(
                filter,
                includeProperties: "District,Areas",
                asNoTracking: asNoTracking
            );
        }

        public async Task<SubDistrict> GetByIdAsync(string subDistrictId, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SubDistrict>().GetAsync(
                x => x.SubDistrictId == subDistrictId,
                includeProperties: "District,Areas",
                asNoTracking: asNoTracking
            );
        }

        public async Task<SubDistrict> UpdateAsync(SubDistrict entity)
        {
            await _unitOfWork.Repository<SubDistrict>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }
    }
}