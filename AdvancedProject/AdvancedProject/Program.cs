using Microsoft.EntityFrameworkCore;
using AdvancedProjectAPI.Data;
using AdvancedProjectAPI.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApplicationPartManager(apm =>
    {
        // Prevent the API project's controllers from being loaded into the MVC app.
        // The project reference is needed only for models and DbContext.
        var apiParts = apm.ApplicationParts
            .Where(ap => ap.Name == "AdvancedProjectAPI")
            .ToList();
        foreach (var part in apiParts)
            apm.ApplicationParts.Remove(part);
    });

builder.Services.AddDbContext<APContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<APContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();
//  this makes sure the roles and the manager always exist in the db
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "PropertyManager", "Tenant", "MaintenanceStaff" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var seedUsers = new[]
    {
        new { UserId = 1, Email = "manager@mail.com",           FullName = "System Manager",    Password = "Manager@123",   Role = "PropertyManager" },
        new { UserId = 2, Email = "zahraa.hubail8@gmail.com",   FullName = "Zahraa Hubail",     Password = "Zahraa.123",    Role = "Tenant" },
        new { UserId = 3, Email = "raghad@gmail.com",           FullName = "Raghad Aleskafi",   Password = "Raghad.123",    Role = "Tenant" },
        new { UserId = 4, Email = "fatima@gmail.com",           FullName = "Fatima Alaiwi",     Password = "Fatima.123",    Role = "Tenant" },
        new { UserId = 5, Email = "norain@mail.com",            FullName = "Norain Almajed",    Password = "Norain.123",    Role = "Tenant" },
        new { UserId = 6, Email = "ahmed.ali@gmail.com",        FullName = "Ahmed Ali",         Password = "Ahmed.999",     Role = "Tenant" },
        new { UserId = 7, Email = "alihassan@mail.com",         FullName = "Ali Hassan",        Password = "Ali.1234",      Role = "MaintenanceStaff" },
        new { UserId = 8, Email = "sara.mohamed@gmail.com",     FullName = "Sara Mohamed",      Password = "Sara.888",      Role = "MaintenanceStaff" },
        new { UserId = 9, Email = "abbas@gmail.com",            FullName = "Abbas Hadi",        Password = "Abbas.123",     Role = "MaintenanceStaff" },
        new { UserId = 10, Email = "layla@gmail.com",           FullName = "Layla Yaser",       Password = "Layla.999",     Role = "MaintenanceStaff" },
        new { UserId = 11, Email = "mohammed@gmail.com",        FullName = "Mohammed Karim",    Password = "Mohammed.123",  Role = "MaintenanceStaff" },
    };

    foreach (var seed in seedUsers)
    {
        var existing = await userManager.FindByEmailAsync(seed.Email);
        if (existing == null)
        {
            var identityUser = new ApplicationUser
            {
                UserName = seed.Email,
                Email = seed.Email,
                FullName = seed.FullName,
                EmailConfirmed = true,
                UserId = seed.UserId
            };
            var result = await userManager.CreateAsync(identityUser, seed.Password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(identityUser, seed.Role);
        }
    }

    // Hash any plain-text passwords in the custom Users table (all users)
    var dbContext = scope.ServiceProvider.GetRequiredService<APContext>();
    var hasher = new PasswordHasher<User>();
    var allUsers = dbContext.Users.ToList();
    foreach (var user in allUsers)
    {
        if (user.Password.Length <= 50) // plain text is short; hashes are 84 chars
        {
            user.Password = hasher.HashPassword(null!, user.Password);
        }
    }
    await dbContext.SaveChangesAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();
app.Run();