using OpenIddict.Abstractions;

namespace Shopniu_identity.Infrastructure.Persistance.Seeders;

public static class OpenIddictClientSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var manager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = "shopniu-web",
            ClientType = OpenIddictConstants.ClientTypes.Public,
            ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
            DisplayName = "Shopniu Web Application",
            RedirectUris =
            {
                new Uri("http://localhost:3000/callback"),
                new Uri("https://oauth.pstmn.io/v1/callback"),
                new Uri("https://purple-flower-0a8e4230f.7.azurestaticapps.net/callback")
            },
            PostLogoutRedirectUris =
            {
                new Uri("http://localhost:3000/"),
                new Uri("https://purple-flower-0a8e4230f.7.azurestaticapps.net/")
            },
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
        };

        var application = await manager.FindByClientIdAsync("shopniu-web");

        if (application is null)
        {
            await manager.CreateAsync(descriptor);
        }
        else
        {
            await manager.UpdateAsync(application, descriptor);
        }
    }
}