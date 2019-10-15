using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using RomanianVioletRollsRoyce.Web.Clients;
using RomanianVioletRollsRoyce.Web.Configuration;
using Serilog;
using System;
using RomanianVioletRollsRoyce.Crosscutting.Context;
using RomanianVioletRollsRoyce.Crosscutting.Middleware;

namespace RomanianVioletRollsRoyce.Web
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
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            SetupConfiguration(services);
            SetupDependencyInjection(services);
            SetupHttpClients(services);
        }

        private void SetupConfiguration(IServiceCollection services)
        {
            services.Configure<APIConfiguration>(options => Configuration.GetSection(nameof(APIConfiguration)).Bind(options));
            services.AddSingleton(e => e.GetRequiredService<IOptions<APIConfiguration>>().Value);
        }

        private void SetupDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<RequestContext>();
        }

        private void SetupHttpClients(IServiceCollection services)
        {
            var baseAddress = Configuration["apiConfiguration:baseAddress"];
            services.AddHttpClient<ICustomersClient, CustomersClient>(client =>
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Add("accept", "application/json");
                })
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(150)))
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<RequestContextMiddleware>();
            app.UseMiddleware<LogContextMiddleware>();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
