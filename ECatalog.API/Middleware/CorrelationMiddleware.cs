using Serilog.Context;
using System.Security.Claims;

namespace ECatalog.API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Check if Correlation-Id exist or not. If not then generate a new one, if exist then use existing
            var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var headerValue)
                ? headerValue.ToString()
                : Guid.NewGuid().ToString();

            context.Items[CorrelationIdHeader] = correlationId;

            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeader] = correlationId;
                return Task.CompletedTask;
            });

            var requestPath = context.Request.Path;
            var method = context.Request.Method;
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("RequestPath", requestPath))
            using (LogContext.PushProperty("RequestMethod", method))
            using (LogContext.PushProperty("UserId", userId))
            {
                await _next(context);
            }
        }
    }
}
