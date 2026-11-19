using Warehouse.API.Services.Implementations;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddAllServices(this IServiceCollection services, 
        IConfiguration configuration,
        string corsName)
    {
        var dbConnectionString = configuration.GetConnectionString("SqliteConnection")
            ?? throw new InvalidOperationException("Не задана строка подключения ConnectionStrings:SqliteConnection");

        services.AddDatabaseConnection(dbConnectionString);
        services.AddCorePolicies(configuration, corsName);
        services.AddOpenApiExtension();
        services.AddWarehouseIdentity();
        services.AddJwt(configuration);
        services.AddValidators();
        services.AddSignalR();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDeliveryUserService, DeliveryUserService>();
        services.AddScoped<IPaperService, PaperService>();
        services.AddScoped<ICartridgeService, CartridgeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IDeliveryService, DeliveryService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        services.AddScoped<IPdfProcessingService, PdfProcessingService>();
        return services;
    }
}