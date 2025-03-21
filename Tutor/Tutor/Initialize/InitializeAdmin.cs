using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Tutor.Models;

namespace Tutor.Initialize
{
    public static class InitializeAdmin
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;

                var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string adminEmail = "vyacheslavgvozdikov@yandex.ru";
                string adminPassword = "bPhJ%2WqQo9@";

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var admin = new User { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true, FullName = adminEmail };
                    var result = await userManager.CreateAsync(admin, adminPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }
        }
    }
}
