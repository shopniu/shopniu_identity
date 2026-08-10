

namespace Shopniu_identity.Aplication.Authentication.Auth.Ports;

public interface IPermissionService
{
    Task<IEnumerable<string>> GetPermissionsByUserIdAsync(int userId);
}