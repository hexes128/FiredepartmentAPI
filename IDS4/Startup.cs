using IdentityServer4;
using IdentityServer4.KeyManagement.EntityFramework;
using IDS4.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IDS4
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
            var IdentityConnection = Configuration.GetConnectionString("IdentityConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;


            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(IdentityConnection, opt => opt.MigrationsAssembly(migrationsAssembly)));
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = true;
                config.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddIdentityServer()
         .AddConfigurationStore(options =>
         {
             options.ConfigureDbContext = builder => builder.UseSqlServer(IdentityConnection,opt => opt.MigrationsAssembly(migrationsAssembly));
         }).

         AddOperationalStore(options =>
         {
             options.ConfigureDbContext = builder => builder.UseSqlServer(IdentityConnection,opt => opt.MigrationsAssembly(migrationsAssembly));
             options.EnableTokenCleanup = true;
         })
           .AddAspNetIdentity<IdentityUser>().AddSigningKeyManagement(
                option =>
                {
                    option.Licensee = "DEMO";
                    option.License = "eyJTb2xkRm9yIjowLjAsIktleVByZXNldCI6NiwiU2F2ZUtleSI6ZmFsc2UsIkxlZ2FjeUtleSI6ZmFsc2UsIlJlbmV3YWxTZW50VGltZSI6IjAwMDEtMDEtMDFUMDA6MDA6MDAiLCJhdXRoIjoiREVNTyIsImV4cCI6IjIwMjItMDctMDVUMDE6MDA6MDEuODkzMDMxOCswMDowMCIsImlhdCI6IjIwMjItMDYtMDVUMDE6MDA6MDEiLCJvcmciOiJERU1PIiwiYXVkIjo1fQ==.Gx0JJjxpk6zRh/rWbup9SXy+sx8i4diCcw4dPXdA0o/CBj/yvEe/gSMY+Jut2hl6Wsl1UFv9lBxawU2S4hedaLq6Cgm5fgkVKWuXmIbote52ZAFOCgY3RySBmcXCoQONoJ37AyOnMpAFAxaXtBnQaFE8CUi+zMhhgq3kuMIn9jXKMOJtYRMdkukRL//4mSeVfLJOIAqg2Z8U7XIwmQId9WwY5jR4X6tiI6+ihtD5wbfM0LtIFXQ8YlzzqxKQQ4Q4OniHGGtWRKoYuhgftCKTJEth4sFL5gD4kee0NjMnwif6i5q6IAedCDkP3UICfgn/WJhM0+WWQB7N63FhPwp2ibDUrcRi0t3olHq4OqeqtxBWsHFToKm6EQzfZK4Bd0a1B1mbVB9Ig+KWzLKzrh6sRp31r5+OwMLOG1P1Uv+tzCYX/lGW7X0fAtGxggfa3b1t5eVQm/CK5EReVL4vqjwz/QlB6Xglm68W7535R3JbOEcVXmd5Mb7GoRVTlii6paOjr4vDpYxNymXNEPy/AfeEG8MmHW/jcZ3qTTChEYhc8pXdDwHW7Upcnah9YPoV4Uld+yb7LpG8OcHCjVnmcMHKorHZk0Tufnk6//Eakk5Ih9sVX+uzMPFtY6mhdl0M0GdG/ElnVSc1Cx4au3L5AmDwdQ/bgcofYs9Ma1OtCLPh60Y=";  
                })
           .PersistKeysToDatabase(new DatabaseKeyManagementOptions
           {

               ConfigureDbContext = opt => opt.UseSqlServer(IdentityConnection)
           }).EnableInMemoryCaching();

 


            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
            });





            services.AddControllersWithViews();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {


            }
            app.UseDeveloperExceptionPage();
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
