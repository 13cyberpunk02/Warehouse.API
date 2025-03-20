using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class DepartmentEndpoint
{
    public static IEndpointRouteBuilder MapDepartmentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/departmnet");
        group.MapGet("get-all-departments", GetAllDepartments).RequireAuthorization();
        group.MapGet("get-department-by-id/{departmentId:int}", GetDepartmentById).RequireAuthorization();
        group.MapGet("get-department-by-name/{departmentName:required}", GetDepartmentByName).RequireAuthorization();
        return group;
    }

    private static async Task<IResult> GetAllDepartments(IDepartmentService service)
    {
        var response = await service.GetAllDepartments();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetDepartmentById(IDepartmentService service, int departmentId)
    {
        var response = await service.GetDepartmentById(departmentId);
        return response.ToHttpResponse();
    }
    
    private static async Task<IResult> GetDepartmentByName(IDepartmentService service, string departmentName)
    {
        var response = await service.GetDepartmentByName(departmentName);
        return response.ToHttpResponse();
    }
}