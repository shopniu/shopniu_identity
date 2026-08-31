using Shopniu_identity.Aplication.Authentication.Auth.Ports;
using Shopniu_identity.Domain.Entities.UserEntity;
using System.Security.Claims;


namespace Shopniu_identity.Aplication.Authentication.Auth.UseCases.Exchange;

public class ExchangeTokenUseCase
{
    private readonly IUserAuthService _userAuthService;
    private readonly IIdentityService _identityService;
    private readonly IPermissionService _permissionService;

    public ExchangeTokenUseCase(IUserAuthService userAuthService, IIdentityService identityService, IPermissionService permissionService)
    {
        _userAuthService = userAuthService;
        _identityService = identityService;
        _permissionService = permissionService;
    }

    public async Task<ClaimsPrincipal> ExecuteAsync(ClaimsPrincipal principal)
    {
        var user = await _userAuthService.GetUserByPrincipalAsync(principal);
        return await ExecuteAsync(user);
    }

    public async Task<ClaimsPrincipal> ExecuteAsync(User user)
    {
        var roles = await _userAuthService.GetUserRolesAsync(user);
        var permissions = await _permissionService.GetPermissionsByUserIdAsync(user.Id);

        return await _identityService.CreateAsync(user, roles, permissions);
    }
}