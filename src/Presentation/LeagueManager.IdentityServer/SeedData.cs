using IdentityModel;
using LeagueManager.IdentityServer.Exceptions;
using LeagueManager.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LeagueManager.IdentityServer
{
    public static class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    string roleName = "Administrator";
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    CreateAdminRole(roleManager, roleName).Wait();

                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var adminUser = userManager.FindByNameAsync("admin").Result;
                    if (adminUser == null)
                    {
                        adminUser = new ApplicationUser
                        {
                            UserName = "admin"
                        };
                        var result = userManager.CreateAsync(adminUser, "LeagueManager01!").Result;
                        if (!result.Succeeded)
                        {
                            throw new CreateUserException(result.Errors.First().Description);
                        }

                        result = userManager.AddClaimsAsync(adminUser, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "admin")
                        }).Result;

                        if (!result.Succeeded)
                        {
                            throw new AddClaimsException(result.Errors.First().Description);
                        }
                        userManager.AddToRoleAsync(adminUser, roleName).Wait();
                        Console.WriteLine("admin created");
                    }
                    else
                    {
                        Console.WriteLine("admin already exists");
                    }                    
                }
            }
        }

        private static async Task CreateAdminRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var adminRole = roleManager.FindByNameAsync(roleName).Result;
            if (adminRole == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new CreateRoleException(result.Errors.First().Description);
                }
            }
        }
    }
}