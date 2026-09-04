using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ValueObjects;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.CommonConstants;
using System.Linq.Expressions;


namespace ACTCore.CollectionService.Application.Security
{
    public class PolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly LanguageService _languageService;

        public PolicyService(IUnitOfWork unitOfWork, IMapper mapper, LanguageService languageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _languageService = languageService;
        }

        public async Task<IEnumerable<SysPolicy>> GetListAsync(Func<SysPolicy, bool> filter = null, bool asNoTracking = true)
        {
            var policies = await _unitOfWork.Repository<SysPolicy>().GetAllAsync(asNoTracking: asNoTracking);
            if (filter != null)
            {
                policies = policies.Where(filter).ToList();
            }
            return policies;
        }

        public async Task<SysPolicy> GetAsync(Expression<Func<SysPolicy, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<SysPolicy>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking);
        }

        public async Task<SysPolicy> GetByIdAsync(int id, bool asNoTracking = true)
        {
            var policy = await _unitOfWork.Repository<SysPolicy>().GetAllAsync(
                filter: n =>
                n.Id == id,
                asNoTracking: asNoTracking
            );
            return policy.FirstOrDefault();
        }

        public async Task<SysPolicy> UpdateAsync(SysPolicy entity)
        {
            await _unitOfWork.Repository<SysPolicy>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<SysPolicyValidatePwdPolicyResponseDto> ValidatePasswordPolicy(string username, string newPwd, bool asNoTracking = true)
        {

            IEnumerable<SysPolicy> policies = await GetListAsync(filter: p => p.IsActive && p.PolicyCategory == "Password Validation", asNoTracking: asNoTracking);

            foreach (var policy in policies)
            {
                var result = policy.PolicyCode switch
                {
                    "min_length" => ValidateMinLength(newPwd, policy),
                    "max_length" => ValidateMaxLength(newPwd, policy),
                    "consecutive_char" => ValidateNoConsecutiveChar(newPwd, policy),
                    "contain_3types" => await ValidateContain3Types(newPwd, policy),
                    "notallow_userid" => ValidateNotAllowUserId(newPwd, username, policy),
                    _ => null
                };

                if (result != null && !result.Valid)
                    return result;
            }

            return new SysPolicyValidatePwdPolicyResponseDto { Valid = true, Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_PasswordMeetsAllPolicy) };

        }

        // --- Helper ---
        private SysPolicyValidatePwdPolicyResponseDto Fail(SysPolicy policy, LanguageValue message) => new() { Valid = false, Message = message };
        private SysPolicyValidatePwdPolicyResponseDto Success() => new() { Valid = true };

        // --- Validators ---
        private SysPolicyValidatePwdPolicyResponseDto ValidateMinLength(string pwd, SysPolicy policy)
        {
            if (int.TryParse(policy.PolicyValue, out int min) && pwd.Length < min)
                return Fail(policy, new LanguageValue() { En = $"{policy.PolicyName} {min}", Th = $"{policy.PolicyName} {min}" });
            return Success();
        }

        private SysPolicyValidatePwdPolicyResponseDto ValidateMaxLength(string pwd, SysPolicy policy)
        {
            if (int.TryParse(policy.PolicyValue, out int max) && pwd.Length > max)
                return Fail(policy, new LanguageValue() { En = $"{policy.PolicyName} {max}", Th = $"{policy.PolicyName} {max}" });
            return Success();
        }

        private SysPolicyValidatePwdPolicyResponseDto ValidateNotAllowUserId(string pwd, string username, SysPolicy policy)
        {
            if (pwd.Contains(username, StringComparison.OrdinalIgnoreCase))
                return Fail(policy, new LanguageValue() { En = $"{policy.PolicyName}, {username}", Th = $"{policy.PolicyName}, {username}" });
            return Success();
        }

        private async Task<SysPolicyValidatePwdPolicyResponseDto> ValidateContain3Types(string pwd, SysPolicy policy)
        {
            int typeCount =
                (pwd.Any(char.IsUpper) ? 1 : 0) +
                (pwd.Any(char.IsLower) ? 1 : 0) +
                (pwd.Any(char.IsDigit) ? 1 : 0) +
                (pwd.Any(c => !char.IsLetterOrDigit(c)) ? 1 : 0);

            if (typeCount < 3)
                return Fail(policy, await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_PolicyNameUpperLowerNonalpha));

            return Success();
        }

        private SysPolicyValidatePwdPolicyResponseDto ValidateNoConsecutiveChar(string pwd, SysPolicy policy)
        {
            var seen = new HashSet<char>();
            foreach (char c in pwd)
            {
                if (!seen.Add(c))
                    return Fail(policy, new LanguageValue() { En = policy.PolicyName, Th = policy.PolicyName });
            }
            return Success();
        }


    }
}
