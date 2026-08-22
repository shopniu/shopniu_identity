using Microsoft.AspNetCore.Identity;
using OpenIddict.Validation.AspNetCore;
using Shopniu_identity.Domain.Entities.UserEntity;
using Shopniu_identity.Domain.Entities.RoleEntity;
using Shopniu_identity.Infrastructure.Persistance;
using Shopniu_identity.Aplication.Authentication.Auth.Ports;
using Shopniu_identity.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Authorization;

public static class IdentityServicesExtensions
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        // services
        services.AddScoped<IUserAuthService, UserAuthService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IScopeService, OpenIddictScopeService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorization();

        // login view
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/account/login";
            options.AccessDeniedPath = "/account/accessdenied";
            // 30 días: con "Recordar sesión" la cookie interactiva persiste al
            // cierre del navegador y no se corta a los 30 minutos.
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
        });

        return services;
    }
}