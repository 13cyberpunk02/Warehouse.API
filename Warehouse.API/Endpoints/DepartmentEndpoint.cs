using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Department;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class DepartmentEndpoint
{
    public static IEndpointRouteBuilder MapDepartmentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/department");
        group.MapGet("get-all-departments", GetAllDepartments).RequireAuthorization();
        group.MapGet("get-department-by-id/{departmentId:int}", GetDepartmentById).RequireAuthorization();
        group.MapGet("get-department-by-name/{departmentName:required}", GetDepartmentByName).RequireAuthorization();
        group.MapPost("add-department", AddDepartment).RequireAuthorization();
        group.MapPut("update-department", UpdateDepartment).RequireAuthorization();
        group.MapDelete("remove-department", RemoveDepartment).RequireAuthorization();
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

    private static async Task<IResult> AddDepartment(IDepartmentService service, string name)
    {
        var response = await service.AddDepartment(name);
        return response.ToHttpResponse();
    }
    
    private static async Task<IResult> UpdateDepartment(IDepartmentService service, UpdateDepartmentRequest request)
    {
        var response = await service.UpdateDepartment(request);
        return response.ToHttpResponse();
    }
    
    private static async Task<IResult> RemoveDepartment(IDepartmentService service, int departmentId)
    {
        var response = await service.DeleteDepartment(departmentId);
        return response.ToHttpResponse();
    }
}