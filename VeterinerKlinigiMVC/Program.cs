using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using VeterinerKlinigiMVC.Data;
using VeterinerKlinigiMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısı
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity ve kullanıcı rolleri
builder.Services.AddDefaultIdentity<ApplicationUser>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// MVC servisleri
builder.Services.AddControllersWithViews();

// Türkçe dil ve kültür ayarları
var culture = new CultureInfo("tr-TR");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(culture);
    options.SupportedCultures = new[] { culture };
    options.SupportedUICultures = new[] { culture };
});

var app = builder.Build();

// HTTP istek hattı
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Türkçe yerelleştirme
app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedRolesAsync(scope.ServiceProvider);

    await DbInitializer.SeedAdminAsync(scope.ServiceProvider);
}

app.Run();