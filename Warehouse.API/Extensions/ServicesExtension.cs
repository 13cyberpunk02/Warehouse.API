using Warehouse.API.Services.Implementations;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionString = configuration.GetConnectionString("SqliteConnection");

        services.AddDatabaseConnection(dbConnectionString);
        services.AddWarehouseIdentity();
        services.AddJwt(configuration);
        services.AddAuthorization();
        services.AddValidators();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}