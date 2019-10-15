using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using RomanianVioletRollsRoyce.Crosscutting.Context;

namespace RomanianVioletRollsRoyce.Crosscutting.Middleware
{
    public abstract class ContextMiddleware
    {
        public abstract Task InvokeAsync(HttpContext context, RequestContext requestContext);

        protected static string GetValueFromHeaders(HttpContext context, string key)
        {
            string value = null;
            if (context.Request.Headers.TryGetValue(key, out var headers))
            {
                value = headers.FirstOrDefault();
            }

            return value;
        }

        protected static bool IsMonitoringEndpoint(HttpContext context) => context.Request.Path.Value.ToLower().Contains("monitoring");
    }
}
