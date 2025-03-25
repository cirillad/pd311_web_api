using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.DAL
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.MigrateAsync();

            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }

            // Створення Адміна
            var adminEmail = "admin@gmail.com";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
            {
                var admin = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Створення звичайного користувача
            var userEmail = "user@gmail.com";
            var existingUser = await userManager.FindByEmailAsync(userEmail);
            if (existingUser == null)
            {
                var user = new AppUser
                {
                    UserName = "user",
                    Email = userEmail,
                    FirstName = "Regular",
                    LastName = "User",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var userResult = await userManager.CreateAsync(user, "User123!");
                if (userResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
