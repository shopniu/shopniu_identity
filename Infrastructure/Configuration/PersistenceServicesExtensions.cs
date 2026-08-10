using Microsoft.EntityFrameworkCore;
using Shopniu_identity.Infrastructure.Persistance;

namespace Shopniu_identity.Infrastructure.Configuration;

public static class PersistenceServicesExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(defaultConnection))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is missing. Configure it in appsettings or environment variables.");
        }

        services.AddDbContextPool<AppDbContext>(options =>
            options.UseSqlServer(defaultConnection)
                   .UseOpenIddict()); // Use OpenIddict for authentication and authorization

        return services;
    }
}
