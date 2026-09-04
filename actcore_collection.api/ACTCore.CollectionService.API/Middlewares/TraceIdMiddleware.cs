namespace ACTCore.CollectionService.API.Middlewares
{
    public class TraceIdMiddleware
    {
        private readonly RequestDelegate _next;

        public TraceIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("X-Trace-Id"))
            {
                context.Items["TraceId"] = Guid.NewGuid().ToString();
            }
            else
            {
                context.Items["TraceId"] = context.Request.Headers["X-Trace-Id"].ToString();
            }

            context.Response.Headers["X-Trace-Id"] = context.Items["TraceId"].ToString();
            await _next(context);
        }
    }
}
