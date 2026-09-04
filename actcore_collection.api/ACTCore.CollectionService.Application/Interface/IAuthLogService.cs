namespace ACTCore.CollectionService.Application.Interface
{
    public interface IAuthLogService
    {
        Task AddAuthLogAsync(Guid? userId, string userName, string status, string failReason, Guid sessionId);
        Task UpdateAuthLogAsync(Guid sessionId, string authLogStatus);
    }
}
