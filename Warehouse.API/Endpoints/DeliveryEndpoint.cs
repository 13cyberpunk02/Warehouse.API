using System.Security.Claims;
using Warehouse.API.Data.Models.DTO_s.Requests.Delivery;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class DeliveryEndpoint
{
    public static IEndpointRouteBuilder MapDeliveryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/delivery");
        group.MapGet("/get-all-paper-deliveries", GetAllPaperDeliveries).RequireAuthorization();
        group.MapGet("/get-all-cartridge-deliveries", GetAllCartridgeDeliveries).RequireAuthorization();
        group.MapPost("/add-paper-delivery", AddPaperDelivery).RequireAuthorization();
        group.MapPost("/add-cartridge-delivery", AddCartridgeDelivery).RequireAuthorization();
        return group;
    }

    private static async Task<IResult> GetAllPaperDeliveries(IDeliveryService service)
    {
        var response = await service.GetAllPaperDeliveries();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetAllCartridgeDeliveries(IDeliveryService service)
    {
        var response = await service.GetAllCartridgeDeliveries();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> AddCartridgeDelivery(IDeliveryService service, AddDeliveryRequest request, ClaimsPrincipal user)
    {
        var response = await service.AddCartridgeDelivery(request, user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> AddPaperDelivery(IDeliveryService service, AddDeliveryRequest request, ClaimsPrincipal user)
    {
        var response = await service.AddPaperDelivery(request, user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        return response.ToHttpResponse();
    }
}