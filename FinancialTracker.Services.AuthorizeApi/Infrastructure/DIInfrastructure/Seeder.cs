using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.DIInfrastructure
{
    public static class Seeder
    {
        public static void HandleIdentityResult(IdentityResult result, string error)
        {
            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new Exception($"{error}. {errors}");
            }
        }

        public static async Task SeedRoles(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AuthRole>>();
            List<string> roles = [
                Enum_BaseRoles.SUPERUSER.ToString(),
                Enum_BaseRoles.ADMIN.ToString(),
                Enum_BaseRoles.USER.ToString()];

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new AuthRole(role));
                    HandleIdentityResult(result, "failed role created");
                }
        }

        public static async Task SeedSuperUserWithRole(IServiceScope scope)
        {
            string userName = "SuperUser";
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AuthUser>>();
            var user = await userManager.FindByNameAsync(userName);
            if (user is null)
            {
                string pass = Environment.GetEnvironmentVariable("AUTH_SUPER_USER_PASS")!;
                if (string.IsNullOrWhiteSpace(pass))
                    throw new Exception("password for superuser not found");
                user = new AuthUser
                {
                    Email = "superuser@mail.ru",
                    CreateAt = DateTime.UtcNow,
                    UserName = userName,
                };
                var result = await userManager.CreateAsync(user, pass);
                HandleIdentityResult(result, "super user not created");

                result = await userManager.AddToRoleAsync(user, Enum_BaseRoles.SUPERUSER.ToString());
                HandleIdentityResult(result, "error with added role to superuser");
            }
        }
    }
}
