using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;

namespace Warehouse.API.Extensions;

public static class DbContextExtension
{
    public static IServiceCollection AddDatabaseConnection(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
    }
}