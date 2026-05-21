using Microsoft.AspNetCore.Identity;
using AdvancedProjectAPI.Models;

namespace AdvancedProjectAPI.Data;

public static class IdentitySeeder
{
    public const string PropertyManagerRole = "PropertyManager";
    public const string TenantRole = "Tenant";
    public const string MaintenanceStaffRole = "MaintenanceStaff";

    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // Seed roles
        foreach (var roleName in new[] { PropertyManagerRole, TenantRole, MaintenanceStaffRole })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                    throw new Exception($"Failed to create role '{roleName}': " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }
        }

        // Seed users
        await EnsureUserAsync(userManager, "manager@mail.com",  "Manager@123",  "System Manager",    PropertyManagerRole);
        await EnsureUserAsync(userManager, "tenant@propertyleasing.local",  "Tenant#12345", "Test Tenant",       TenantRole);
        await EnsureUserAsync(userManager, "staff@propertyleasing.local",   "Staff#12345",  "Maintenance Staff", MaintenanceStaffRole);
    }

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string email, string password, string fullName, string role)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = fullName
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception($"Failed to create user '{email}': " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Ensure role is assigned even if user already existed
        if (!await userManager.IsInRoleAsync(user, role))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
                throw new Exception($"Failed to assign role '{role}' to '{email}': " +
                    string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }
    }
}
