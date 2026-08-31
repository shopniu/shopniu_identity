using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopniu_identity.Domain.Entities.RoleEntity;
using Shopniu_identity.Infrastructure.Persistance;
using Shopniu_identity.Infrastructure.Persistance.Seeders;

namespace Shopniu_identity.Infrastructure.Configuration;

public static class DatabaseInitializationExtensions
{
    public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
    {
        var migrateOnStartup = app.Configuration.GetValue<bool>("Database:Migration:RunOnStartup");
        var seedOnStartup = app.Configuration.GetValue<bool>("Database:Seeding:RunOnStartup");

        if (!migrateOnStartup && !seedOnStartup)
        {
            return app;
        }

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        if (migrateOnStartup)
        {
            await dbContext.Database.MigrateAsync();
        }

        if (seedOnStartup)
        {
            await RolePermissionSeeder.SeedAsync(dbContext, roleManager);
            await UserSeeder.SeedAsync(scope.ServiceProvider);
            await OpenIddictScopeSeeder.SeedAsync(scope.ServiceProvider);
            await OpenIddictClientSeeder.SeedAsync(scope.ServiceProvider, app.Configuration);
        }

        return app;
    }
}
