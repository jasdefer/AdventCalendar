using AdventCalendarWebApp.Helper;
using AdventCalendarWebApp.Helper.Middleware;
using AdventCalendarWebApp.Helper.TimeProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AdventCalendarWebApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment appEnv;

        public Startup(IConfiguration configuration, IWebHostEnvironment appEnv)
        {
            Configuration = configuration;
            this.appEnv = appEnv;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            if (appEnv.IsDevelopment())
            {
                services.AddTransient<ITimeProvider, DebugTimeProvider>();
            }
            else
            {
                services.AddTransient<ITimeProvider, UtcTimeProvider>();
            }
            services.AddTransient<DayValidation>();
            services.AddTransient<AzureHelper>();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseMiddleware<StatisticLogger>();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}