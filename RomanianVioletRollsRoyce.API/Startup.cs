using System;
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
using Microsoft.Extensions.Caching.Memory;
using RomanianVioletRollsRoyce.Crosscutting.Factories;
using RomanianVioletRollsRoyce.Data.Interfaces;
using RomanianVioletRollsRoyce.Data.Repositories;
using RomanianVioletRollsRoyce.Domain.Entities;
using RomanianVioletRollsRoyce.Services.Interfaces;
using RomanianVioletRollsRoyce.Services.Services;
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
            services.AddTransient<IServiceFactory, ServiceFactory>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
        }

        private void SetupHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<SystemMemoryHealthCheck>("System memory");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache cache)
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
            SeedCustomers(cache);
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

        private void SeedCustomers(IMemoryCache cache)
        {
            cache.Set("C-1", new Customer { CustomerId = 1, Name = "Karl Jan", Surname = "Bossart" }, TimeSpan.FromHours(1));
            cache.Set("C-2", new Customer { CustomerId = 2, Name = "Robert", Surname = "Cailliau" }, TimeSpan.FromHours(1));
            cache.Set("C-3", new Customer { CustomerId = 3, Name = "Ingrid", Surname = "Daubechies" }, TimeSpan.FromHours(1));
        }
    }
}
