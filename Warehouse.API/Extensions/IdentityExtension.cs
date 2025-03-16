using Microsoft.AspNetCore.Identity;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;

namespace Warehouse.API.Extensions;

public static class IdentityExtension
{
    public static IServiceCollection AddWarehouseIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddUserManager<UserManager<AppUser>>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}