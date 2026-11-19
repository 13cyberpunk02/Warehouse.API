using Warehouse.API.Data.Models.DTO_s.Requests.Cartridge;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class CartridgeEndpoint
{
    public static IEndpointRouteBuilder MapCartridgeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/cartridge");
        group.MapGet("/get-all-cartridges", GetAllCartridges).RequireAuthorization();
        group.MapGet("/get-cartridge-by-id/{cartridgeId:int}", GetCartridgeById).RequireAuthorization();
        group.MapPost("/add-cartridge", AddCartridge).RequireAuthorization();
        group.MapPut("/update-cartridge", UpdateCartridge).RequireAuthorization();
        group.MapDelete("/delete-cartridge/{cartridgeId:int}", DeleteCartridge).RequireAuthorization();
        return group;
    }

    private static async Task<IResult> DeleteCartridge(ICartridgeService cartridgeService, int cartridgeId)
    {
        var response = await cartridgeService.DeleteCartridge(cartridgeId);
        return response.ToHttpResponse();
    }
    private static async Task<IResult> UpdateCartridge(ICartridgeService cartridgeService,
        UpdateCartridgeRequest request)
    {
        var response = await cartridgeService.UpdateCartridge(request);
        return response.ToHttpResponse();
    }
    private static async Task<IResult> AddCartridge(ICartridgeService cartridgeService, AddCartridgeRequest request)
    {
        var response = await cartridgeService.CreateCartridge(request);
        return response.ToHttpResponse();
    }
    private static async Task<IResult> GetAllCartridges(ICartridgeService cartridgeService)
    {
        var response = await cartridgeService.GetAllCartridges();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetCartridgeById(ICartridgeService cartridgeService, int cartridgeId)
    {
        var response = await cartridgeService.GetCartridgeById(cartridgeId);
        return response.ToHttpResponse();
    }
}