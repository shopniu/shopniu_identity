using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopniu_identity.Domain.Entities.UserEntity;
using Shopniu_identity.Domain.Entities.RoleEntity;
using Shopniu_identity.Domain.Entities.PermissionEntity;
using Shopniu_identity.Domain.Entities.UserPermissionEntity;
using Shopniu_identity.Domain.Entities.RolePermissionEntity;

namespace Shopniu_identity.Infrastructure.Persistance;

public class AppDbContext : IdentityDbContext<User, Role, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>(); public DbSet<UserPermission> UserPermissions => Set<UserPermission>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.UseOpenIddict();
    }
}