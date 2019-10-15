using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using RomanianVioletRollsRoyce.Crosscutting.Context;

namespace RomanianVioletRollsRoyce.Crosscutting.Middleware
{
    public class RequestContextMiddleware : ContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestContextMiddleware(RequestDelegate next)
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
                requestContext.CorrelationId = GetValueFromHeaders(context, "CorrelationId") ?? Guid.NewGuid().ToString();
                await _next(context);
            }
        }
    }
}
