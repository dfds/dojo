using System.Net.Http;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;

namespace DFDS.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "AspNetCore-WebApi-Basic", Version = "v1"}); });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new FeatureLocationExpander());
            });

            services.AddHttpContextAccessor();
            services.AddCorrelationId();

            services.AddTransient<ForwardedHeadersAsBasePathExtensions.ForwardedHeaderBasePath>();
            services.AddTransient<CorrelationIdMessageHandler>();

            services.AddSingleton(serviceProvider =>
            {
                DelegatingHandler[] handlers = new[]
                {
                    serviceProvider.GetRequiredService<CorrelationIdMessageHandler>()
                };

                return HttpClientFactory.Create(handlers);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Log.Warning($"CRM_SERVICE_URL {Configuration["CRM_SERVICE_URL"]}");
            Log.Warning($"RECOMMENDATION_SERVICE_URL {Configuration["RECOMMENDATION_SERVICE_URL"]}");
            Log.Warning($"ORDER_SERVICE_URL {Configuration["ORDER_SERVICE_URL"]}");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetCore-WebApi-Basic V1"));
            }

            app.UseCorrelationId(new CorrelationIdOptions
            {
                Header = "x-correlation-id",
                IncludeInResponse = true,
                UpdateTraceIdentifier = true,
            });

            app.UseForwardedHeadersAsBasePath();
            app.UseStaticFiles();
            app.UseMetricServer();
            app.UseMvc();
        }
    }
}