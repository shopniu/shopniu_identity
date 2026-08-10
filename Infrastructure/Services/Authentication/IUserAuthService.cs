using System.Security.Claims;
using OpenIddict.Abstractions;
using Microsoft.AspNetCore.Identity;
using Shopniu_identity.Domain.Entities.UserEntity;
using Shopniu_identity.Aplication.Authentication.Auth.Ports;
using Shopniu_identity.Domain.Exceptions;


namespace Shopniu_identity.Infrastructure.Services.Authentication;

public class UserAuthService : IUserAuthService
{
    private readonly UserManager<User> _userManager;

    public UserAuthService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> GetUserByPrincipalAsync(ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirst(OpenIddictConstants.Claims.Subject)?.Value ??
        principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new BusinessRuleException("The user ID claim is missing or invalid.");
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new BusinessRuleException("User not found.");
        }

        return user;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}