using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RomanianVioletRollsRoyce.Crosscutting.HealthChecks
{
    public class SystemMemoryHealthCheck : IHealthCheck
    {
        private readonly ILogger<SystemMemoryHealthCheck> _logger;
        private readonly IMemoryUsageLoader _memoryUsageLoader;

        public SystemMemoryHealthCheck(ILogger<SystemMemoryHealthCheck> logger, IMemoryUsageLoader memoryUsageLoader)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryUsageLoader = memoryUsageLoader ?? throw new ArgumentNullException(nameof(memoryUsageLoader));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var memoryUsage = _memoryUsageLoader.GetMemoryUsage();
                var percentUsed = memoryUsage.Used * 100 / memoryUsage.Total;

                var status = DetermineHealthStatus(percentUsed);
                var result = new HealthCheckResult(status, data: new Dictionary<string, object>()
                {
                    { nameof(memoryUsage.Total), memoryUsage.Total },
                    { nameof(memoryUsage.Free), memoryUsage.Free },
                    { nameof(memoryUsage.Used), memoryUsage.Used }
                });
                return await Task.FromResult(result);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Error while retrieving memory usage metrics: {@Exception}", exc);
                return await Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy, "Could not determine memory status"));
            }
        }

        private HealthStatus DetermineHealthStatus(double percentUsed)
            => percentUsed switch
            {
                _ when percentUsed > 90 => HealthStatus.Unhealthy,
                _ when percentUsed > 80 => HealthStatus.Degraded,
                _ => HealthStatus.Healthy,
            };
    }
}
