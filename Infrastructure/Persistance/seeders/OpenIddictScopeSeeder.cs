// Infrastructure/Persistance/Seeders/OpenIddictScopeSeeder.cs
using OpenIddict.Abstractions;

namespace Shopniu_identity.Infrastructure.Persistance.Seeders;

public static class OpenIddictScopeSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var manager = serviceProvider.GetRequiredService<IOpenIddictScopeManager>();

        if (await manager.FindByNameAsync("shopniu-api") is not null)
            return;

        await manager.CreateAsync(new OpenIddictScopeDescriptor
        {
            Name = "shopniu-api",
            DisplayName = "Shopniu API access",
            Resources = { "shopniu-api" }
        });
    }
}