

using System.Security.Claims;
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Aplication.Authentication.Auth.Ports;

public interface IIdentityService
{
    Task<ClaimsPrincipal> CreateAsync(
        User user,
        IEnumerable<string>? roles = null,
        IEnumerable<string>? permissions = null,
        IEnumerable<string>? resources = null);

}