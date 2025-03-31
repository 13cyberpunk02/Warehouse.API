using Warehouse.API.Data.Models.DTO_s.Requests.Department;
using Warehouse.API.Data.Models.DTO_s.Responses.Department;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IDepartmentService
{
    Task<Result> GetAllDepartments();
    Task<Result> GetDepartmentById(int departmentId);
    Task<Result> GetDepartmentByName(string name);
    Task<Result> AddDepartment(string name);
    Task<Result> UpdateDepartment(UpdateDepartmentRequest request);
    Task<Result> DeleteDepartment(int id);
}