using BankRUs.Application.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BankRUs.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        await SeedRolesAsync(roleManager);

        // Seed test users (for development/testing)
        await SeedCustomerServiceUserAsync(userManager);
        await SeedCustomerUserAsync(userManager); // optional but useful for US10 tests
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create role '{role}': {errors}");
                }
            }
        }
    }

    private static async Task SeedCustomerServiceUserAsync(UserManager<ApplicationUser> userManager)
    {
        // ✅ CustomerService user (to access /api/customers)
        const string email = "cs@bankrus.test";
        const string password = "Test123!"; // must satisfy Identity password rules

        var existing = await userManager.FindByEmailAsync(email);
        if (existing is null)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,

                FirstName = "Jane",
                LastName = "Doe",
                SocialSecurityNumber = "19900101-5678"
            };


            var create = await userManager.CreateAsync(user, password);
            if (!create.Succeeded)
            {
                var errors = string.Join(", ", create.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create CustomerService user '{email}': {errors}");
            }

            existing = user;
        }

        if (!await userManager.IsInRoleAsync(existing, Roles.CustomerService))
        {
            var addRole = await userManager.AddToRoleAsync(existing, Roles.CustomerService);
            if (!addRole.Succeeded)
            {
                var errors = string.Join(", ", addRole.Errors.Select(e => e.Description));
                throw new Exception($"Failed to add '{email}' to role '{Roles.CustomerService}': {errors}");
            }
        }
    }

    private static async Task SeedCustomerUserAsync(UserManager<ApplicationUser> userManager)
    {
        // Optional: a normal customer to show up in listings
        const string email = "customer1@bankrus.test";
        const string password = "Test123!";

        var existing = await userManager.FindByEmailAsync(email);
        if (existing is null)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,

                FirstName = "Customer",
                LastName = "Service",
                SocialSecurityNumber = "19800101-1234"
            };


            var create = await userManager.CreateAsync(user, password);
            if (!create.Succeeded)
            {
                var errors = string.Join(", ", create.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create Customer user '{email}': {errors}");
            }

            existing = user;
        }

        if (!await userManager.IsInRoleAsync(existing, Roles.Customer))
        {
            var addRole = await userManager.AddToRoleAsync(existing, Roles.Customer);
            if (!addRole.Succeeded)
            {
                var errors = string.Join(", ", addRole.Errors.Select(e => e.Description));
                throw new Exception($"Failed to add '{email}' to role '{Roles.Customer}': {errors}");
            }
        }
    }
}
