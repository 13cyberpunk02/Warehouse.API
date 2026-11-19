using Warehouse.API.Endpoints;

namespace Warehouse.API.Extensions;

public static class EndpointExtension
{
    public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAuthEndpoints();
        endpoints.MapDeliveryUserEndpoints();
        endpoints.MapPaperEndpoints();
        endpoints.MapDepartmentEndpoints();
        endpoints.MapDeliveryEndpoints();
        endpoints.MapCartridgeEndpoints();
        endpoints.MapPdfToolsEndpoints();
        return endpoints;
    }
}