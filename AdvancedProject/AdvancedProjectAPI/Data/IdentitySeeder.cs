using Microsoft.AspNetCore.Identity;
using AdvancedProjectAPI.Models;

namespace AdvancedProjectAPI.Data;

public static class IdentitySeeder
{
    // Define your roles for Brief B
    public const string PropertyManagerRole = "PropertyManager";
    public const string TenantRole = "Tenant";
    public const string MaintenanceStaffRole = "MaintenanceStaff";

    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services
            .GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services
            .GetRequiredService<UserManager<ApplicationUser>>();

        // Seed all roles
        foreach (var roleName in new[]
        {
            PropertyManagerRole,
            TenantRole,
            MaintenanceStaffRole
        })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed default Property Manager (senior role)
        const string managerEmail = "manager@propertyleasing.local";
        var manager = await userManager.FindByEmailAsync(managerEmail);
        if (manager == null)
        {
            manager = new ApplicationUser
            {
                UserName = managerEmail,
                Email = managerEmail,
                EmailConfirmed = true,
                FullName = "Property Manager"  // Changed from DisplayName
            };
            var result = await userManager
                .CreateAsync(manager, "Manager#12345");
            if (result.Succeeded)
            {
                await userManager
                    .AddToRoleAsync(manager, PropertyManagerRole);
            }
        }

        // Seed a test Tenant
        const string tenantEmail = "tenant@propertyleasing.local";
        var tenant = await userManager.FindByEmailAsync(tenantEmail);
        if (tenant == null)
        {
            tenant = new ApplicationUser
            {
                UserName = tenantEmail,
                Email = tenantEmail,
                EmailConfirmed = true,
                FullName = "Test Tenant"  // Changed from DisplayName
            };
            var result = await userManager
                .CreateAsync(tenant, "Tenant#12345");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(tenant, TenantRole);
            }
        }

        // Seed a test Maintenance Staff
        const string staffEmail = "staff@propertyleasing.local";
        var staff = await userManager.FindByEmailAsync(staffEmail);
        if (staff == null)
        {
            staff = new ApplicationUser
            {
                UserName = staffEmail,
                Email = staffEmail,
                EmailConfirmed = true,
                FullName = "Maintenance Staff"  // Changed from DisplayName
            };
            var result = await userManager
                .CreateAsync(staff, "Staff#12345");
            if (result.Succeeded)
            {
                await userManager
                    .AddToRoleAsync(staff, MaintenanceStaffRole);
            }
        }
    }
}