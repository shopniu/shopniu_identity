using System.Security.Claims;
using Shopniu_identity.Aplication.Authentication.Auth.UseCases.Authorize;
using Shopniu_identity.Aplication.Authentication.Auth.UseCases.Exchange;


namespace Shopniu_identity.Aplication.Authentication.Auth;

public class AuthHandler
{

    private readonly AuthorizeUseCase _authorizeUseCase;
    private readonly ExchangeTokenUseCase _exchangeTokenUseCase;

    public AuthHandler(AuthorizeUseCase authorizeUseCase, ExchangeTokenUseCase exchangeTokenUseCase)
    {
        _authorizeUseCase = authorizeUseCase;
        _exchangeTokenUseCase = exchangeTokenUseCase;
    }

    public async Task<ClaimsPrincipal> AuthorizeAsync(ClaimsPrincipal user)
    {
        var identity = await _authorizeUseCase.ExecuteAsync(user);
        Console.WriteLine("AuthorizeAsync: " + identity.Identity?.Name);
        return identity;
    }

    public async Task<ClaimsPrincipal> ExchangeTokenAsync(ClaimsPrincipal user)
    {
        var identity = await _exchangeTokenUseCase.ExecuteAsync(user);
        return identity;
    }
}