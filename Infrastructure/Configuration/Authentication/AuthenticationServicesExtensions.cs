
namespace Shopniu_identity.Infrastructure.Configuration.Authentication;

public static class AuthenticationServicesExtensions
{
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services)
    {
        services
            .AddIdentityServices()
            .AddOpenIddictServices()
            .AddAuthorization();

        return services;
    }
}