using AdvancedProjectReporting.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Register the typed HttpClient for API communication
builder.Services.AddHttpClient<ApiClient>(client =>
{
    client.BaseAddress = new Uri(
        "https://localhost:7211/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reports}/{action=Index}/{id?}");

app.Run();