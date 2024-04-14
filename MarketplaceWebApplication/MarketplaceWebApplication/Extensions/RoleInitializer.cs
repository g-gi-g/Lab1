using Microsoft.AspNetCore.Identity;
using MarketplaceWebApplication.Data.Identity;

namespace MarketplaceWebApplication.Extensions;

public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "admin@gmail.com";

        string password = "Qwerty_1";

        string firstName = "Admin";

        string lastName = "Admin";

        if (await roleManager.FindByNameAsync("admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
        }

        if (await roleManager.FindByNameAsync("user") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("user"));
        }

        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            User admin = new User { Email = adminEmail, UserName = adminEmail, FirstName = firstName, LastName = lastName };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
