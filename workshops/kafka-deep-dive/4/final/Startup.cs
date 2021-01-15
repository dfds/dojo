using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kafka_deep_dive.Enablers;
using kafka_deep_dive.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace kafka_deep_dive
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            const string topic = "build.workshop.something";
            services.AddControllersWithViews();

            services.AddTransient<EventHandlerFactory>();
            services.AddTransient<IEventHandler<WorkshopCreated>, WorkshopCreatedHandler>();
            
            var eventRegistry = new EventRegistry();
            services.AddSingleton<EventRegistry>(eventRegistry);

            eventRegistry
                .Register<WorkshopCreated>("workshop_created", topic);

            services.AddTransient<KafkaConfiguration>();
            services.AddTransient<KafkaConsumerFactory>();
            services.AddTransient<KafkaProducerFactory>();
            services.AddHostedService<KafkaConsumer>();
            services.AddHostedService<KafkaProducer>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
