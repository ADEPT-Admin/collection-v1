using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using SharedKernel.Models;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Security
{
    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LanguageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Language>> GetListAsync(Expression<Func<Language, bool>> filter = null, bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Language>().GetAllAsync(filter, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<Language>> GetListPaginationAsync(Expression<Func<Language, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1,
            string sortColumn = "", string sortDirection = "asc",
            FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<Language>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<Language> GetAsync(Expression<Func<Language, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Language>().GetAsync(
                filter,
                includeProperties: includeProperties,
                asNoTracking: asNoTracking
             );
        }

        public async Task<LanguageJsonDto> GetMessageAsync(Expression<Func<Language, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            var entity = _mapper.Map<LanguageJsonDto>(
                await _unitOfWork.Repository<Language>().GetAsync(
                    filter,
                    includeProperties: includeProperties,
                    asNoTracking: asNoTracking
                 )
            );
            return entity;
        }

        public async Task<Language> UpdateAsync(Language entity)
        {
            await _unitOfWork.Repository<Language>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<Language> CreateAsync(Language entity)
        {
            await _unitOfWork.Repository<Language>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public Dictionary<string, string> GetTranslatebyLanguage(IEnumerable<Language> languages, string lang)
        {
            var selectedLanguage = languages.ToDictionary(
                l => l.Key,
                l => GetValueByLang(l.Value, l.DefaultValue, lang, l.Key)
            );
            return selectedLanguage;
        }

        private string GetValueByLang(string value, string defaultValue, string lang, string fallbackKey)
        {
            // พยายามแปลง value เป็น Dictionary<string, string>
            Dictionary<string, string> valueDict = null;
            Dictionary<string, string> defaultDict = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(value) && value.TrimStart().StartsWith("{"))
                    valueDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(value);
            }
            catch { /* ignore */ }

            try
            {
                if (!string.IsNullOrWhiteSpace(defaultValue) && defaultValue.TrimStart().StartsWith("{"))
                    defaultDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(defaultValue);
            }
            catch { /* ignore */ }

            lang = lang?.ToLower() ?? "en";

            // ถ้า valueDict มี key ที่ต้องการ
            if (valueDict != null && valueDict.TryGetValue(lang, out var v) && !string.IsNullOrEmpty(v))
                return v;

            // ถ้า defaultDict มี key ที่ต้องการ
            if (defaultDict != null && defaultDict.TryGetValue(lang, out var dv) && !string.IsNullOrEmpty(dv))
                return dv;

            // fallback: en
            if (valueDict != null && valueDict.TryGetValue("en", out var ve) && !string.IsNullOrEmpty(ve))
                return ve;
            if (defaultDict != null && defaultDict.TryGetValue("en", out var dve) && !string.IsNullOrEmpty(dve))
                return dve;

            // fallback: string เดิม
            return fallbackKey;
        }
    }

}
