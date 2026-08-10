
namespace Shopniu_identity.Aplication.Authentication.Auth.Ports;

public interface IScopeService
{
    Task<IEnumerable<string>> GetResourcesAsync(IEnumerable<string> scopes);
}