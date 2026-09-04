using SharedKernel.Models;

namespace ACTCore.CollectionService.Application.Interface
{
    public interface IErrorLogService
    {
        Task ErrrorLogAsync(ErrorLog log);
    }
}
