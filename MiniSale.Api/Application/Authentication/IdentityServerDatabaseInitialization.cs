using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MiniSale.Api.Infrastructure.Contexts.Identity;
using MiniSale.Api.Models.Account.Entity;
using System.Linq;

namespace MiniSale.Api.Application.Authentication
{
    /// <summary>
    /// First Time initialization
    /// </summary>
    public static class IdentityServerDatabaseInitialization
    {
        /// <summary>
        /// Initialize Admin user
        /// </summary>
        /// <param name="app"></param>
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                                         .GetService<IServiceScopeFactory>()
                                         .CreateScope())
            {
                SeedUserData(serviceScope);
            }
        }

        private static void SeedUserData(IServiceScope serviceScope)
        {
            var context = serviceScope
                              .ServiceProvider
                              .GetRequiredService<ApplicationDbContext>();

            var userManager = serviceScope
                              .ServiceProvider
                              .GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager = serviceScope
                              .ServiceProvider
                              .GetRequiredService<RoleManager<IdentityRole>>();


            if (!context.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(Roles.Manage)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(Roles.Operator)).GetAwaiter().GetResult();
            }

            if (!context.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    UserName = "Admin",
                    FirstName = "Administrator"
                };

                var result = userManager.CreateAsync(user, "Admin!234").GetAwaiter().GetResult();

                if (result.Errors.Count() <= 0)
                {
                    userManager.AddToRoleAsync(user, Roles.Admin).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(user, Roles.Manage).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(user, Roles.Operator).GetAwaiter().GetResult();
                }
            }
        }
    }
}
