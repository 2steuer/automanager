using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using automanager.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace automanager
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
            services.AddControllersWithViews();

            services.AddDbContext<CarManagerDatabase>(opts => {
                 opts.UseSqlite("Data Source=tanken.db");
                 opts.UseLazyLoadingProxies();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CarManagerDatabase db)
        {
            app.UsePathBase(Configuration["PathBase"]);
            var supportedCultures = new string[] { "de-DE" };
            app.UseRequestLocalization(options =>
                        options
                        .AddSupportedCultures(supportedCultures)
                        .AddSupportedUICultures(supportedCultures)
                        .SetDefaultCulture("de-DE")
                        .RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                        {
                            return Task.FromResult(new ProviderCultureResult("de-DE"));
                        }))
                );
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                db.Database.Migrate();
            }
            
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
