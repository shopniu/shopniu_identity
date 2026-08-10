// Infrastructure/Persistance/Seeders/RolePermissionSeeder.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopniu_identity.Domain.Entities.PermissionEntity;
using Shopniu_identity.Domain.Entities.RoleEntity;
using Shopniu_identity.Domain.Entities.RolePermissionEntity;

namespace Shopniu_identity.Infrastructure.Persistance.Seeders;

public static class RolePermissionSeeder
{
    public static async Task SeedAsync(AppDbContext context, RoleManager<Role> roleManager)
    {
        var alreadySeeded = await context.Permissions.AnyAsync() || await roleManager.Roles.AnyAsync();
        if (alreadySeeded) return; // Database has already been seeded

        var permissions = new List<Permission>
        {
            new Permission("user.create", "Create a new user"),
            new Permission("user.read", "Read user information"),
            new Permission("user.update", "Update user information"),
            new Permission("user.delete", "Delete a user"),

            new Permission("role.create", "Create a new role"),
            new Permission("role.read", "Read role information"),
            new Permission("role.update", "Update role information"),
            new Permission("role.delete", "Delete a role"),

            new Permission("product.read", "Read product information"),
            new Permission("product.create", "Create a new product"),
            new Permission("product.update", "Update product information"),
            new Permission("product.delete", "Delete a product"),

            new Permission("order.read", "Read order information"),
            new Permission("order.create", "Create a new order"),
            new Permission("order.update", "Update order information"),
            new Permission("order.refund", "Refund an order"),
        };

        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();

        // Roles vía RoleManager: necesario para que Identity normalice Name (NormalizedName)
        // correctamente, o luego UserManager.AddToRoleAsync no los va a encontrar.
        var adminRole = new Role("Admin", "Administrator role with full permissions");
        var userRole = new Role("User", "Regular user role with limited permissions");
        var sellerRole = new Role("Seller", "Seller role with permissions to manage products and orders");

        foreach (var role in new[] { adminRole, userRole, sellerRole })
        {
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var permissionsByCode = permissions.ToDictionary(p => p.Code, p => p);

        var rolePermission = new List<RolePermission>
        {
            new RolePermission(adminRole, permissionsByCode["user.create"]),
            new RolePermission(adminRole, permissionsByCode["user.read"]),
            new RolePermission(adminRole, permissionsByCode["user.update"]),
            new RolePermission(adminRole, permissionsByCode["user.delete"]),
            new RolePermission(adminRole, permissionsByCode["role.create"]),
            new RolePermission(adminRole, permissionsByCode["role.read"]),
            new RolePermission(adminRole, permissionsByCode["role.update"]),
            new RolePermission(adminRole, permissionsByCode["role.delete"]),
            new RolePermission(adminRole, permissionsByCode["product.read"]),
            new RolePermission(adminRole, permissionsByCode["product.create"]),
            new RolePermission(adminRole, permissionsByCode["product.update"]),
            new RolePermission(adminRole, permissionsByCode["product.delete"]),
            new RolePermission(adminRole, permissionsByCode["order.create"]),
            new RolePermission(adminRole, permissionsByCode["order.update"]),
            new RolePermission(adminRole, permissionsByCode["order.refund"]),

            new RolePermission(userRole, permissionsByCode["user.read"]),
            new RolePermission(userRole, permissionsByCode["role.read"]),
            new RolePermission(userRole, permissionsByCode["product.read"]),
            new RolePermission(userRole, permissionsByCode["order.read"]),
            new RolePermission(userRole, permissionsByCode["order.create"]),

            new RolePermission(sellerRole, permissionsByCode["product.create"]),
            new RolePermission(sellerRole, permissionsByCode["product.update"]),
            new RolePermission(sellerRole, permissionsByCode["product.delete"]),
            new RolePermission(sellerRole, permissionsByCode["order.create"]),
            new RolePermission(sellerRole, permissionsByCode["order.update"])
        };

        await context.RolePermissions.AddRangeAsync(rolePermission);
        await context.SaveChangesAsync();
    }
}