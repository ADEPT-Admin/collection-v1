using ACTCore.CollectionService.API.Middlewares;

namespace ACTCore.CollectionService.API.Extensions
{
    public static class TraceIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseTraceId(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TraceIdMiddleware>();
        }
    }
}
