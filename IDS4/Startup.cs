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
            services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(IdentityConnection, opt => opt.MigrationsAssembly(migrationsAssembly)));

         
       
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = true;
                config.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
            {


                options.Discovery.CustomEntries.Add("localApi", "~/localapi");
            })
        .AddConfigurationStore(options =>
         {
             options.ConfigureDbContext = builder => builder.UseSqlServer(IdentityConnection,
                 opt => opt.MigrationsAssembly(migrationsAssembly));
         })

         .AddOperationalStore(options =>
         {
             options.ConfigureDbContext = builder => builder.UseSqlServer(IdentityConnection,
                   opt => opt.MigrationsAssembly(migrationsAssembly));
             options.EnableTokenCleanup = true;
         })
           .AddAspNetIdentity<IdentityUser>().AddSigningKeyManagement(
                option =>
                {

                    option.Licensee = "DEMO";
                    option.License = "eyJTb2xkRm9yIjowLjAsIktleVByZXNldCI6NiwiU2F2ZUtleSI6ZmFsc2UsIkxlZ2FjeUtleSI6ZmFsc2UsIlJlbmV3YWxTZW50VGltZSI6IjAwMDEtMDEtMDFUMDA6MDA6MDAiLCJhdXRoIjoiREVNTyIsImV4cCI6IjIwMjEtMTItMjhUMDE6MDA6MDMuNDU4ODE5NyswMDowMCIsImlhdCI6IjIwMjEtMTEtMjhUMDE6MDA6MDMiLCJvcmciOiJERU1PIiwiYXVkIjo1fQ==.KbSEd70Lc1h1+RXMh/ZouelsnIRgVuKLssyYs3qtKJ0GDvUWYup468GWMCbgom1hLF/ymfvX0auuo+U5M8wz5myeFgzF8+L4WMfOxTc/PcR1LLuuBibjG1ycggJRNKowri8lLmxYvKXf9c4ySvUHdXhlD/H6rJStw1R5ZeyWMqev+QtWR8YOuuwb8+Z7HqU1dV2Y2Yy0j8CQuvkbQgSGt9GHcY5FH/di67BsAU5TM9oKDACIRUiyUllJ3kKD88nQSp7y6ELUKoKztV4vFVcE6VjHsAh3onoAww3iq8bbtVjaseJ9bwYrX1eWY53AIVmpIlgQL2fS6YaFrOGuyWO7LA3DKwRy1jwgSgpTI6eifsINuMuKGBKb26a5uUKF+8qG9k+2Ap35vBug4cslN8vR6B4DjcNjuwtwKZ4tPegpR0H6qiy9NlYeCh9EotRaNEj20jKhLHwNuRssTt9XzXLuREv+QMF3f0tzlzVKCc1yt2EuQWnZU8ciMvRHfh9mUqhrga+LMtTcn8uNrLmiBELABnyduFajc/w4oY1KcUYsGqlqR/4Ek9CvsMJVy8Q/3P+5hievk8ESmJ+7NFhfRBCMJkTWIBJMVBlf01QQm4zvobw/1oS4T+2qS2hKOfQXx/yJ/9cT+zV+R03AgQJ5EtpNxqUYSPvOkJK33knNR+7Oltg=";


                }
                )
           .PersistKeysToDatabase(new DatabaseKeyManagementOptions
           {

               ConfigureDbContext = opt => opt.UseSqlServer(IdentityConnection)
           }).EnableInMemoryCaching();
           //.AddDeveloperSigningCredential()
           ;
            
            services.AddLocalApiAuthentication();

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
