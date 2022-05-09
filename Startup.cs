using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingSystem.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using HotelBookingSystem.Data;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem
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

            // DbContexts
            services.AddDbContext<HotelBookingSystemContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("HotelBookingSystemContext")));
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AuthDbContextConnection")));

            // Identity
            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false; // true��Ϊ��false
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<IdentityRole>()   // ��ӽ�ɫ���� Identity
                .AddEntityFrameworkStores<AuthDbContext>();

            services.AddRazorPages();   // Added while deploying Identity Authentication
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

            app.UseAuthentication();    // Added while deploying Identity Authentication
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                // endpoints.MapControllerRoute(
                //     name: "customers",
                //     pattern: "customers");
                //
                // endpoints.MapControllerRoute(
                //     name:"MoviesByReleaseDate",
                //     pattern:"movies/released/{year}/{month}",
                //     defaults: new { controller = "Movies", action = "ByReleaseDate" },
                //     constraints: new { year = @"2015|2016", month = @"\d{2}"}); // not tested
                //     // constraints: new { year = @"\d{4}", month = @"\d{2}"}); // works
            });
        }
    }
}
