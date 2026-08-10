

using System.Security.Claims;
using Shopniu_identity.Aplication.Authentication.Auth.Ports;

namespace Shopniu_identity.Aplication.Authentication.Auth.UseCases.Authorize;

public class AuthorizeUseCase
{
    private readonly IUserAuthService _userService;
    private readonly IIdentityService _identityService;
    private readonly IPermissionService _permissionService;

    public AuthorizeUseCase(IUserAuthService userService, IIdentityService identityService, IPermissionService permissionService)
    {
        _userService = userService;
        _identityService = identityService;
        _permissionService = permissionService;

    }

    public async Task<ClaimsPrincipal> ExecuteAsync(ClaimsPrincipal principal)
    {
        var user = await _userService.GetUserByPrincipalAsync(principal);

        var roles = await _userService.GetUserRolesAsync(user);

        var permissions = await _permissionService.GetPermissionsByUserIdAsync(user.Id);

        return await _identityService.CreateAsync(user, roles, permissions, resources: null);

    }
}