using Shopniu_identity.Infrastructure.Persistance;


public static class OpenIddictServicesExtensions
{
    public static IServiceCollection AddOpenIddictServices(this IServiceCollection services)
    {
        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<AppDbContext>();
            })
            .AddServer(options =>
            {
                // options.DisableAccessTokenEncryption();
                options.SetTokenEndpointUris("api/v1/auth/token")
                       .SetAuthorizationEndpointUris("api/v1/auth/authorize")
                       .SetUserInfoEndpointUris("api/v1/auth/userinfo")
                       .SetEndSessionEndpointUris("api/v1/auth/logout");

                options.AllowAuthorizationCodeFlow() // para produccion se necesitan certificados de encriptacion y firma reales
                       .RequireProofKeyForCodeExchange();

                options.AllowRefreshTokenFlow();

                options.RegisterScopes("api", "profile", "email", "roles", "offline_access");

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableUserInfoEndpointPassthrough()
                       .EnableStatusCodePagesIntegration();

                options.DisableAccessTokenEncryption();
            })
            .AddValidation(options =>
            {
                options.AddAudiences("shopniu_gateway");
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        return services;
    }
}