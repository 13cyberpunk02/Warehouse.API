namespace Warehouse.API.Extensions;

public static class CorePoliciesExtension
{
    public static IServiceCollection AddCorePolicies(this IServiceCollection services, IConfiguration configuration, string corsPolicyName)
    {
        // В Docker фронтенд раздаётся тем же сервером, CORS нужен только для ng serve
        var origins = configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:4200"];

        services.AddCors(options =>
        {
            options.AddPolicy(corsPolicyName, policy =>
            {
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return services;
    }
}
