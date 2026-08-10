using System.Security.Claims;
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Aplication.Authentication.Auth.Ports;

public interface IUserAuthService
{
    Task<User> GetUserByPrincipalAsync(ClaimsPrincipal principal);
    Task<IEnumerable<string>> GetUserRolesAsync(User user);
}