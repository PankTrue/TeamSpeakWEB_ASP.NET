using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TeamSpeakWEB.Data;
using TeamSpeakWEB.Filters;
using TeamSpeakWEB.Models;
using TeamSpeakWEB.Services;

namespace TeamSpeakWEB
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this._env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection")))
            );
            services.AddDatabaseDeveloperPageExceptionFilter();


            services.AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

            })  .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            var mvcBuilder = services.AddControllersWithViews();

            #if DEBUG
            if (_env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
            #endif


            services.AddFlashes();
            services.AddRazorPages();

            services.AddAutoMapper
                (typeof(AutoMapperProfile).Assembly);

            //Custom filters
            services.AddScoped<TsserverBelongsToCurrentUserFilter>();


            //custom services
            services.AddSingleton<TeamSpeakQueryClient>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
