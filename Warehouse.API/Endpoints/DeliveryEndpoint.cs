using Warehouse.API.Data.Models.DTO_s.Requests.Delivery;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class DeliveryEndpoint
{
    public static IEndpointRouteBuilder MapDeliveryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/delivery");
        group.MapGet("/get-all-deliveries", GetAllDeliveries).RequireAuthorization();
        group.MapPost("/add-delivery", AddDelivery).RequireAuthorization();
        group.MapPost("/get-delivery-by-time-user-department", GetAllDeliveriesByDepartmentOrUserAndTime).RequireAuthorization();
        return group;
    }

    private static async Task<IResult> GetAllDeliveries(IDeliveryService service)
    {
        var response = await service.GetAllDeliveries();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> AddDelivery(IDeliveryService service, AddDeliveryRequest request)
    {
        var response = await service.AddDelivery(request);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetAllDeliveriesByDepartmentOrUserAndTime(IDeliveryService service,
        GetDeliveryByUserOrDepartment request)
    {
        var response = await service.GetAllDeliveriesByDepartmentOrUser(request);
        return response.ToHttpResponse();
    }
}