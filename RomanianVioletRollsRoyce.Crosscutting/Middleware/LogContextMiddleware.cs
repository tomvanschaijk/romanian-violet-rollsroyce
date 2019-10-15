using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using RomanianVioletRollsRoyce.Crosscutting.Context;
using Serilog.Context;

namespace RomanianVioletRollsRoyce.Crosscutting.Middleware
{
    public class LogContextMiddleware : ContextMiddleware
    {
        private readonly RequestDelegate _next;

        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public override async Task InvokeAsync(HttpContext context, RequestContext requestContext)
        {
            if (IsMonitoringEndpoint(context))
            {
                await _next(context);
            }
            else
            {
                using (LogContext.PushProperty("CorrelationId", requestContext.CorrelationId ?? GetValueFromHeaders(context, "CorrelationId")))
                {
                    await _next(context);
                }
            }
        }
    }
}
