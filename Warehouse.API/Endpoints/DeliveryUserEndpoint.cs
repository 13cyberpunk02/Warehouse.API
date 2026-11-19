using Warehouse.API.Data.Models.DTO_s.Requests.DeliveryUser;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class DeliveryUserEndpoint
{
    public static IEndpointRouteBuilder MapDeliveryUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/delivery-user");
        group.MapGet("/get-all-delivery-users", GetAllDeliveryUsersAsync).RequireAuthorization();
        group.MapGet("/get-delivery-user-by-id/{userId:int}", GetDeliveryUserById).RequireAuthorization();
        group.MapPost("/create-delivery-user", CreateDeliveryUser).RequireAuthorization();
        group.MapPut("/update-delivery-user", UpdateAccount).RequireAuthorization("Admin");        
        group.MapGet("get-all-delivery-users-by-department-id/{departmentId:int}", GetDeliveryUserByDepartmentId).RequireAuthorization();        
        group.MapDelete("/delete-delivery-user/{userId:int}", DeleteDeliveryUser).RequireAuthorization("Admin");
        return group;
    }

    private static async Task<IResult> GetAllDeliveryUsersAsync(IDeliveryUserService deliveryUserService)
    {
        var response = await deliveryUserService.GetAllDeliveryUsers();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> CreateDeliveryUser(IDeliveryUserService deliveryUserService,
        CreateDeliveryUserRequest request)
    {
        var response = await deliveryUserService.CreateDeliveryUserAsync(request);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetDeliveryUserById(IDeliveryUserService deliveryUserService, int userId)
    {
        var response = await deliveryUserService.GetDeliveryUserByIdAsync(userId);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> UpdateAccount(IDeliveryUserService deliveryUserService,
        UpdateDeliveryUserRequest updateDeliveryUserRequest)
    {
        var response = await deliveryUserService.UpdateDeliveryUserAsync(updateDeliveryUserRequest);
        return response.ToHttpResponse();
    }
    private static async Task<IResult> GetDeliveryUserByDepartmentId(IDeliveryUserService service, int departmentId)
    {
        var response = await service.GetDeliveryUserByDepartmentIdAsync(departmentId);
        return response.ToHttpResponse();
    }
    private static async Task<IResult> DeleteDeliveryUser(IDeliveryUserService deliveryUserService, int userId)
    {
        var response = await deliveryUserService.DeleteDeliveryUserAsync(userId);
        return response.ToHttpResponse();
    }
}