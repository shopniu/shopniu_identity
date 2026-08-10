
using Shopniu_identity.Aplication.Authentication.Auth;
using Shopniu_identity.Aplication.Authentication.Auth.UseCases.Authorize;
using Shopniu_identity.Aplication.Authentication.Auth.UseCases.Exchange;
using Shopniu_identity.Aplication.Authentication.Account;


namespace Shopniu_identity.Infrastructure.Configuration;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AuthHandler>();
        services.AddScoped<AuthorizeUseCase>();
        services.AddScoped<ExchangeTokenUseCase>();

        services.AddScoped<AccountHandler>();

        return services;
    }
}
