﻿using Warehouse.API.Services.Implementations;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddAllServices(this IServiceCollection services, 
        IConfiguration configuration,
        string corsName)
    {
        var dbConnectionString = configuration.GetConnectionString("SqliteConnection");

        services.AddDatabaseConnection(dbConnectionString);
        services.AddCorePolicies(corsName);
        services.AddWarehouseIdentity();
        services.AddJwt(configuration);
        services.AddAuthorization();
        services.AddValidators();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}