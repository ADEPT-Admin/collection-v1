using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Domain.Entities.Securities;
using System.Linq.Expressions;

namespace ACTCore.CollectionService.Application.Interface
{
    public interface ILanguageService
    {
        Task<LanguageJsonDto> GetMessageAsync(Expression<Func<Language, bool>> filter = null, string includeProperties = "", bool asNoTracking = true);
    }
}
