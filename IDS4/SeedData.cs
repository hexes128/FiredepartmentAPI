// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
 using IdentityServer4.EntityFramework.Mappers;
 using IdentityServer4.EntityFramework.Storage;

using IDS4.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
 using Microsoft.Extensions.DependencyInjection;
 using Serilog;

 namespace IDS4
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.AddOperationalDbContext(options =>
            {
                options.ConfigureDbContext = db =>
                  db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            });
            services.AddConfigurationDbContext(options =>
            {
                options.ConfigureDbContext = db =>
                  db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            });

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
                context.Database.Migrate();

                EnsureSeedData(context);

                var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
                ctx.Database.Migrate();
                EnsureUsers(scope);
            }
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var wunshuai = userMgr.FindByNameAsync("hexes128").Result;
            if (wunshuai == null)
            {
                wunshuai = new IdentityUser
                {
                    UserName = "hexes128",
                    Email = "AliceSmith@email.com",
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(wunshuai, "Hexes128%").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(wunshuai, new Claim[]
                {
          new Claim(JwtClaimTypes.Name, "文率"),
          new Claim(JwtClaimTypes.GivenName, "文率"),
          new Claim(JwtClaimTypes.FamilyName, "鄭"),
          new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
          new Claim(JwtClaimTypes.Email,"hexes128@gmail.com"),
               
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("wunshuai created");
            }
            else
            {
                Log.Debug("wunshuai already exists");
            }

            var wunshuai2 = userMgr.FindByNameAsync("hexes127").Result;
            if (wunshuai2 == null)
            {
                wunshuai2 = new IdentityUser
                {
                    UserName = "hexes127",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(wunshuai2, "Hexes127%").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(wunshuai2, new Claim[]
                {
          new Claim(JwtClaimTypes.Name, "文帥"),
          new Claim(JwtClaimTypes.GivenName, "文帥"),
          new Claim(JwtClaimTypes.FamilyName, "鄭"),
          new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
          new Claim("location", "somewhere")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("文帥 created");
            }
            else
            {
                Log.Debug("文帥 already exists");
            }
        }


        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Log.Debug("Clients being populated");
                foreach (var client in Config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Log.Debug("IdentityResources being populated");
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("IdentityResources already populated");
            }

            if (!context.ApiScopes.Any())
            {
                Log.Debug("ApiScopes being populated");
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("ApiScopes already populated");
            }

            //if (!context.ApiResources.Any())
            //{
            //    Log.Debug("ApiResources being populated");
            //    foreach (var resource in Config.ApiResources.ToList())
            //    {
            //        context.ApiResources.Add(resource.ToEntity());
            //    }

            //    context.SaveChanges();
            //}
            //else
            //{
            //    Log.Debug("ApiScopes already populated");
            //}
        }
    }
}
