using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiredepartmentAPI.Dbcontext;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace FiredepartmentAPI
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
            services.AddControllers();
             //   .AddJsonOptions(x =>
             //x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
           
            services.AddHttpClient();
            services.AddDbContext<FiredepartmentDbContext>(options =>
            {

                options.UseSqlServer(Configuration.GetConnectionString("APIConnection"));
            });

            services.AddAuthentication("Bearer").AddJwtBearer(option => {
                option.RequireHttpsMetadata = false;

                option.Authority = "http://192.168.10.152:4000";

                option.TokenValidationParameters = new TokenValidationParameters {

                    ValidateAudience = false
                };
            
            }


            
                    
                );
            services.AddAuthorization(option=> {
                
                option.AddPolicy("API", builder => {

                    builder.RequireAuthenticatedUser();
                    builder.RequireClaim("scope", "API");
                });
            
            });

          
            services.AddMailKit(config => {
               config.UseMailKit(Configuration.GetSection("Email").Get<MailKitOptions>());
            });
            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
         
            }
    
            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
