using OpenIddict.Abstractions;

namespace Shopniu_identity.Infrastructure.Persistance.Seeders;

public static class OpenIddictClientSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var manager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("shopniu-web") is not null) return;

        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "shopniu-web",
            ClientType = OpenIddictConstants.ClientTypes.Public,
            ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
            DisplayName = "Shopniu Web Application",
            RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
            PostLogoutRedirectUris = { new Uri("https://localhost:3000/") },
            Permissions =
{
    OpenIddictConstants.Permissions.Endpoints.Authorization,
    OpenIddictConstants.Permissions.Endpoints.EndSession,
    OpenIddictConstants.Permissions.Endpoints.Token,
    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
    OpenIddictConstants.Permissions.ResponseTypes.Code,
    OpenIddictConstants.Permissions.Scopes.Email,
    OpenIddictConstants.Permissions.Scopes.Profile,
    OpenIddictConstants.Permissions.Scopes.Roles,
    OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.OfflineAccess,   // 👈 corregido
    OpenIddictConstants.Permissions.Prefixes.Scope + "shopniu-api",
    OpenIddictConstants.Permissions.Prefixes.Endpoint + "userinfo"
},
            Requirements =
            {
                OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
            }
        });
    }
}