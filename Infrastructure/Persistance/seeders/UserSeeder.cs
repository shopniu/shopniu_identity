// Infrastructure/Persistance/Seeders/UserSeeder.cs
using Microsoft.AspNetCore.Identity;
using Shopniu_identity.Domain.Entities.RoleEntity;
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Infrastructure.Persistance.Seeders;

public class UserSeeder
{
    private record SeedUser(string FirstName, string LastName, string Email, string UserName, string Role);

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        var seedUsers = new List<SeedUser>
        {
            new("Admin", "User", "admin@example.com", "admin.admin", "Admin"),
            new("Regular", "User", "user@example.com", "user.regular", "User"),
            new("Seller", "User", "seller@example.com", "seller.seller", "Seller"),
        };

        foreach (var seed in seedUsers)
        {
            // Garantizar que el rol exista antes de asignarlo
            if (!await roleManager.RoleExistsAsync(seed.Role))
            {
                var roleResult = await roleManager.CreateAsync(new Role(seed.Role));
                if (!roleResult.Succeeded)
                    throw new Exception($"Failed to create role '{seed.Role}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }

            // Si el usuario ya existe, saltarlo
            if (await userManager.FindByEmailAsync(seed.Email) is not null)
                continue;

            var user = new User(
                firstName: seed.FirstName,
                lastName: seed.LastName,
                email: seed.Email,
                userName: seed.UserName
            )
            {
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, "Test@1234");
            if (!createResult.Succeeded)
                throw new Exception($"Failed to create user '{seed.Email}': {string.Join(", ", createResult.Errors.Select(e => e.Description))}");

            var roleAssignResult = await userManager.AddToRoleAsync(user, seed.Role);
            if (!roleAssignResult.Succeeded)
                throw new Exception($"Failed to assign '{seed.Role}' role to '{seed.Email}': {string.Join(", ", roleAssignResult.Errors.Select(e => e.Description))}");
        }
    }
}