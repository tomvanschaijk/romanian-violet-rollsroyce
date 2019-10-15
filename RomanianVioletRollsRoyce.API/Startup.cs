using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RomanianVioletRollsRoyce.Crosscutting.HealthChecks;
using RomanianVioletRollsRoyce.Crosscutting.Middleware;
using RomanianVioletRollsRoyce.Crosscutting.RequestContext;
using System.Linq;
using System.Text.Json;
using Serilog;

namespace RomanianVioletRollsRoyce.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                                .ReadFrom
                                .Configuration(configuration)
                                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);
            services.AddControllers();
            services.AddMemoryCache();
            SetupDependencyInjection(services);
            SetupHealthChecks(services);
        }

        private void SetupDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<RequestContext>();
            services.AddTransient<IMemoryUsageLoader, MemoryUsageLoader>();
        }

        private void SetupHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<SystemMemoryHealthCheck>("System memory");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<RequestContextMiddleware>();
            app.UseMiddleware<LogContextMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SetupHealthCheckOptions(app);
        }

        private void SetupHealthCheckOptions(IApplicationBuilder app)
        {
            app.UseHealthChecks("/monitoring/health", new HealthCheckOptions()
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new
                        {
                            resource = e.Key,
                            status = e.Value.Status.ToString(),
                            e.Value.Description,
                            details = e.Value.Data,
                        }),
                        durationMs = report.TotalDuration.TotalMilliseconds
                    });

                    await context.Response.WriteAsync(result);
                }
            });
        }
    }
}
