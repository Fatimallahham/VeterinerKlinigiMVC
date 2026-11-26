using Microsoft.AspNetCore.Identity;
using VeterinerKlinigiMVC.Models;

namespace VeterinerKlinigiMVC.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Veteriner", "Üye" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    CheckResult(result);
                }
            }
        }

        public static async Task SeedAdminAsync(
            IServiceProvider serviceProvider)
        {
            var environment = serviceProvider
                .GetRequiredService<IWebHostEnvironment>();

            if (!environment.IsDevelopment())
                return;

            var configuration = serviceProvider
                .GetRequiredService<IConfiguration>();

            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var email = configuration["AdminSettings:Email"];
            var password = configuration["AdminSettings:Password"];
            var fullName = configuration["AdminSettings:FullName"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "Admin bilgileri bulunamadı.");
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName ?? "Sistem Yöneticisi",
                    EmailConfirmed = true
                };

                admin.PasswordHash = userManager.PasswordHasher
                    .HashPassword(admin, password);

                var result = await userManager.CreateAsync(admin);

                CheckResult(result);
            }
            else if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                throw new InvalidOperationException(
                    "Admin e-posta adresi başka bir kullanıcıya ait.");
            }

            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var result = await userManager.AddToRoleAsync(
                    admin, "Admin");

                CheckResult(result);
            }
        }

        private static void CheckResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ",
                        result.Errors.Select(e => e.Description)));
            }
        }
    }
}