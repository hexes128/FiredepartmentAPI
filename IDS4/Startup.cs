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
                    option.License = "eyJTb2xkRm9yIjowLjAsIktleVByZXNldCI6NiwiU2F2ZUtleSI6ZmFsc2UsIkxlZ2FjeUtleSI6ZmFsc2UsIlJlbmV3YWxTZW50VGltZSI6IjAwMDEtMDEtMDFUMDA6MDA6MDAiLCJhdXRoIjoiREVNTyIsImV4cCI6IjIwMjEtMTEtMjdUMDE6MDA6MDIuNDMzMjU0NyswMDowMCIsImlhdCI6IjIwMjEtMTAtMjhUMDE6MDA6MDIiLCJvcmciOiJERU1PIiwiYXVkIjo1fQ==.HiKxeby5augM24DUGPwxpsEEcZ0bJqguRWoVRDn+GzGBt1jC4kN3SWSOxDQh7QyUrtA3A+Qg/uruqpUnifTvJ9bOcyiVt73MVsaJuokcmWnhwdOkzPgqdkfCS0hTp8ScpRFlUxedUmUP+o4QKYSDXy9yIdo6Ya+kY55QanJCmUMymjJWoq6f5kCIhB5jaxMuCI1sOc4ns9H81ay2GUi46Xa8ao+e7ajgcnry7R/aBNbtJasbbZvx+I830pyyD0cf/L7kDRVL9XhvVLVyr+B/d8kpwaMdlIiwE4ANWkuz8FVB8w1l5LESWfVujvJ5L1ZjZkI5YVqF4L/PdM77HVMh1y5qu3B2t7bHTRYHIvl3D+WJWRfSTI3GkU825O/7mXte3LgCQxiyk6UqbG3qCnS0mkybtI5EoAO1MKlJSWGbeQnm1ao8UYgXxanOJgPP8fu5fW3Eo3007Eju0N+9cQnjo4bPiXmlLO/JCl380+2vPv4nKSVNmf2uZKHYMx9xCt9g4T4XPGNA6jNxOPz+KO0zAIAOot+8D1rq/fTPaUM/MKpSjHkZG0sJjaXtxsG6+H3tHMdH6hjcAv0MMa2qGnTaH10hl7UPIhRlEHqOGDy1y9JVx/GsYw9RPVcc8/QiG7Zoan/Q7zGC+SUNQSgWTrARPcfPoxFqNQFSZANfg3WcmvY=";


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
