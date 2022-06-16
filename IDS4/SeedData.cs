// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using System.IO;
using IDS4.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Data;
using ExcelDataReader;

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

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = true;
                config.SignIn.RequireConfirmedEmail = false;
            })
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
            using (var stream = File.Open(@"C:\Users\hexes128\Desktop\FiredepartmentUsers.xls", FileMode.Open, FileAccess.Read))
            {      
                using (IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(stream))  
                {
                    DataSet set = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                   DataRowCollection   dataRow = set.Tables["Users"].Rows;
                    var columns = set.Tables["Users"].Columns;
                    for (int i = 0; i < dataRow.Count; i++) {
                        var user = userMgr.FindByNameAsync(dataRow[i]["id"].ToString().Trim()).Result;
                        if (user == null)
                        {
                            user = new IdentityUser
                            {
                                UserName = dataRow[i]["id"].ToString().Trim(),
                                Email = dataRow[i]["email"].ToString().Trim(),
                                EmailConfirmed = true,
                            };
                            var result = userMgr.CreateAsync(user, dataRow[i]["password"].ToString().Trim()).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            result = userMgr.AddClaimsAsync(user, new Claim[]
                            {
                               
                           new Claim(JwtClaimTypes.Name,dataRow[i]["Name"].ToString().Trim()),
                          new Claim( JwtClaimTypes.Address,dataRow[i]["US_VISA"].ToString().Trim()),
                          new Claim(JwtClaimTypes.PhoneNumber,dataRow[i]["phone"].ToString().Trim()),
                           new Claim(JwtClaimTypes.Email,dataRow[i]["email"].ToString().Trim()
                           ),

                            }).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            Log.Debug(dataRow[i]["Name"].ToString().Trim()+"created");
                        }
                        else
                        {
                            Log.Debug(dataRow[i]["Name"].ToString().Trim()+ "exists");
                        }
                    }
                }
            } // using

            //Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\\Users\hexes128\Desktop\FiredepartmentUsers.xls");
            //Excel._Worksheet xlWorksheet = (Excel.Worksheet)xlWorkbook.ActiveSheet;




            //var a = userMgr.FindByNameAsync("a0912953188").Result;
            //if (a == null)
            //{
            //    a = new IdentityUser
            //    {
            //        UserName = "a0912953188",
            //        Email = "AliceSmith@email.com",
            //        EmailConfirmed = true,
            //    };
            //    var result = userMgr.CreateAsync(a, "Aaaa4375").Result;
            //    if (!result.Succeeded)
            //    {
            //        throw new Exception(result.Errors.First().Description);
            //    }

            //    result = userMgr.AddClaimsAsync(a, new Claim[]
            //    {
            //   new Claim(JwtClaimTypes.Name, "戴忱晟"),

            //  new Claim("US_VISA","20240424"),
            //  new Claim(JwtClaimTypes.PhoneNumber,"0912953188"),
            //   new Claim(JwtClaimTypes.Email,"a0912953188@gmail.com"),


            //    }).Result;
            //    if (!result.Succeeded)
            //    {
            //        throw new Exception(result.Errors.First().Description);
            //    }

            //    Log.Debug("戴忱晟 created");
            //}
            //else
            //{
            //    Log.Debug("戴忱晟 already exists");
            //}
            //   ///////////////////////////////////////////////////////////////
            //   var b = userMgr.FindByNameAsync("aeiouswim").Result;
            //   if (b == null)
            //   {
            //       b = new IdentityUser
            //       {
            //           UserName = "aeiouswim",
            //           Email = "aeiouswim@gmail.com",
            //           EmailConfirmed = true
            //       };
            //       var result = userMgr.CreateAsync(b, "Bbbb1106").Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       result = userMgr.AddClaimsAsync(b, new Claim[]
            //       {
            //       new Claim(JwtClaimTypes.Name, "鄭莛芸"),
            //       new Claim("US_VISA","20230423"),
            //       new Claim(JwtClaimTypes.PhoneNumber,"0953327685"),
            //       new Claim(JwtClaimTypes.Email,"aeiouswim@gmail.com"),
            //       }).Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       Log.Debug("鄭莛芸 created");
            //   }
            //   else
            //   {
            //       Log.Debug("鄭莛芸 already exists");
            //   }
            //   ///////////////////////////////////////////////////////////////
            //   var c = userMgr.FindByNameAsync("afai0").Result;
            //   if (c == null)
            //   {
            //       c = new IdentityUser
            //       {
            //           UserName = "test1",
            //           Email = "maxsuper119@gmail.com",
            //           EmailConfirmed = true,
            //       };
            //       var result = userMgr.CreateAsync(c, "Cccc0614").Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       result = userMgr.AddClaimsAsync(c, new Claim[]
            //       {
            //new Claim(JwtClaimTypes.Name, "蘇冠銘"),
            //       new Claim("US_VISA","20210803"),
            //       new Claim(JwtClaimTypes.PhoneNumber,"0963-476615"),
            //       new Claim(JwtClaimTypes.Email,"afai0@yahoo.com.tw"),

            //       }).Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       Log.Debug("蘇冠銘 created");
            //   }
            //   else
            //   {
            //       Log.Debug("蘇冠銘 already exists");
            //   }
            //   ///////////////////////////////////////////////////////////////
            //   var d = userMgr.FindByNameAsync("test2").Result;
            //   if (d == null)
            //   {
            //       d = new IdentityUser
            //       {
            //           UserName = "test2",
            //           Email = "AliceSmith@email.com",
            //           EmailConfirmed = true,
            //       };
            //       var result = userMgr.CreateAsync(d, "Test2%").Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       result = userMgr.AddClaimsAsync(d, new Claim[]
            //       {
            // new Claim(JwtClaimTypes.Name, "test2"),
            // new Claim(JwtClaimTypes.GivenName, "test2"),
            // new Claim(JwtClaimTypes.FamilyName, "test2"),
            // new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
            // new Claim(JwtClaimTypes.Email,"maxsuper119@gmail.com"),

            //       }).Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       Log.Debug("wunshuai created");
            //   }
            //   else
            //   {
            //       Log.Debug("wunshuai already exists");
            //   }
            //   var test3 = userMgr.FindByNameAsync("test3").Result;
            //   if (test3 == null)
            //   {
            //       test3 = new IdentityUser
            //       {
            //           UserName = "test3",
            //           Email = "AliceSmith@email.com",
            //           EmailConfirmed = true,
            //       };
            //       var result = userMgr.CreateAsync(test3, "Test3%").Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       result = userMgr.AddClaimsAsync(test3, new Claim[]
            //       {
            // new Claim(JwtClaimTypes.Name, "test3"),
            // new Claim(JwtClaimTypes.GivenName, "test3"),
            // new Claim(JwtClaimTypes.FamilyName, "test3"),
            // new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
            // new Claim(JwtClaimTypes.Email,"maxsuper119@gmail.com"),

            //       }).Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       Log.Debug("wunshuai created");
            //   }
            //   else
            //   {
            //       Log.Debug("wunshuai already exists");
            //   }
            //   var test4 = userMgr.FindByNameAsync("test4").Result;
            //   if (test4 == null)
            //   {
            //       test4 = new IdentityUser
            //       {
            //           UserName = "test4",
            //           Email = "AliceSmith@email.com",
            //           EmailConfirmed = true,
            //       };
            //       var result = userMgr.CreateAsync(test4, "Test4%").Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       result = userMgr.AddClaimsAsync(test4, new Claim[]
            //       {
            // new Claim(JwtClaimTypes.Name, "test4"),
            // new Claim(JwtClaimTypes.GivenName, "test4"),
            // new Claim(JwtClaimTypes.FamilyName, "test4"),
            // new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
            // new Claim(JwtClaimTypes.Email,"maxsuper119@gmail.com"),

            //       }).Result;
            //       if (!result.Succeeded)
            //       {
            //           throw new Exception(result.Errors.First().Description);
            //       }

            //       Log.Debug("wunshuai created");
            //   }
            //   else
            //   {
            //       Log.Debug("wunshuai already exists");
            //   }

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
