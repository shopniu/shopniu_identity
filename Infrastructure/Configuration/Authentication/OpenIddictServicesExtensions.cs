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

                // Password grant (ROP): SOLO para el cliente confidencial
                // "shopniu-bff", que registra al invitado en el checkout sin
                // exponer la contraseña generada al navegador. El cliente
                // público "shopniu-web" no tiene este permiso.
                options.AllowPasswordFlow();

                // "Recordar sesión": los tokens del front duran 30 días. Sin
                // estos valores OpenIddict usa defaults cortos (el access expira
                // pronto y el front perdía la sesión).
                options.SetAccessTokenLifetime(TimeSpan.FromDays(30))
                       .SetRefreshTokenLifetime(TimeSpan.FromDays(30));

                options.RegisterScopes("api", "profile", "email", "roles", "offline_access");

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableEndSessionEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableUserInfoEndpointPassthrough()
                       .EnableStatusCodePagesIntegration();

                options.DisableAccessTokenEncryption();
            })
            .AddValidation(options =>
            {
                options.AddAudiences("shopniu-api");
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        return services;
    }
}