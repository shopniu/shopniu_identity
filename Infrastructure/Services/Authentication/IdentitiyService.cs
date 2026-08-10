using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using Shopniu_identity.Aplication.Authentication.Auth.Ports;
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Infrastructure.Services.Authentication;

public class IdentityService : IIdentityService
{
    public Task<ClaimsPrincipal> CreateAsync(
        User user,
        IEnumerable<string>? roles,
        IEnumerable<string>? permissions,
        IEnumerable<string>? resources)
    {
        var identity = new ClaimsIdentity(
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

        identity.SetClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString())
                .SetClaim(OpenIddictConstants.Claims.Email, user.Email)
                .SetClaim(OpenIddictConstants.Claims.Name, $"{user.FirstName} {user.LastName}");

        if (roles is not null)
        {
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(OpenIddictConstants.Claims.Role, role));
            }
        }

        if (permissions is not null)
        {
            foreach (var permission in permissions)
            {
                identity.AddClaim(new Claim("permission", permission));
            }
        }

        if (resources is not null)
        {
            foreach (var resource in resources)
            {
                identity.AddClaim(new Claim("resource", resource));
            }
        }

        var principal = new ClaimsPrincipal(identity);

        return Task.FromResult(principal);
    }
}