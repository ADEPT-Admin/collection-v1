
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Security
{
    public class PasswordHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PolicyService _policyService;

        public PasswordHistoryService(IUnitOfWork unitOfWork, IMapper mapper, PolicyService policyService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _policyService = policyService;
        }

        public async Task<SysPasswordHistory> AddAsync(SysPasswordHistory entity)
        {
            await _unitOfWork.Repository<SysPasswordHistory>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(SysPasswordHistory entity)
        {
            await _unitOfWork.Repository<SysPasswordHistory>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SysPasswordHistory>> GetListAsync(Expression<Func<SysPasswordHistory, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysPasswordHistory>().GetAllAsync(filter, asNoTracking: asNoTracking);
        }

        public async Task<IEnumerable<SysPasswordHistory>> GetListThrouPolicyAsync(Expression<Func<SysPasswordHistory, bool>> filter = null, bool asNoTracking = true)
        {
            var query = await _unitOfWork.Repository<SysPasswordHistory>().GetAllAsync(filter, asNoTracking: asNoTracking);
            var policy = await _policyService.GetAsync(filter:x => x.PolicyCode == "password_his_length");
            string pwdHisLength = policy?.PolicyValue;
            return query
                .OrderByDescending(x => x.UpdatedDate)
                .Take(int.Parse(pwdHisLength))
                .ToList();
        }

        public async Task<SysPasswordHistory> GetAsync(Expression<Func<SysPasswordHistory, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysPasswordHistory>().GetAsync(filter, asNoTracking: asNoTracking);
        }

    }
}
