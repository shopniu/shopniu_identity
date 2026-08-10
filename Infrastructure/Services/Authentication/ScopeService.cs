

using OpenIddict.Abstractions;
using Shopniu_identity.Aplication.Authentication.Auth.Ports;

public class OpenIddictScopeService : IScopeService
{
    private readonly IOpenIddictScopeManager _scopeManager;

    public OpenIddictScopeService(IOpenIddictScopeManager scopeManager)
    {
        _scopeManager = scopeManager;
    }

    public async Task<IEnumerable<string>> GetResourcesAsync(IEnumerable<string> scopes)
    {
        var resources = new List<string>();

        foreach (var scope in scopes)
        {
            var openIddictScope = await _scopeManager.FindByNameAsync(scope);
            if (openIddictScope != null)
            {
                var scopeResources = await _scopeManager.GetResourcesAsync(openIddictScope);
                resources.AddRange(scopeResources);
            }
        }

        return resources.Distinct();
    }
}