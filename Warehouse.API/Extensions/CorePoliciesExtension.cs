namespace Warehouse.API.Extensions;

public static class CorePoliciesExtension
{
    public static IServiceCollection AddCorePolicies(this IServiceCollection services, string corsPolicyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(corsPolicyName, policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod().
                    AllowCredentials();
            });
        });
        return services;
    }
}