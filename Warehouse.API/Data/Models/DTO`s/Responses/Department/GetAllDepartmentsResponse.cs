using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Responses.Account;

namespace Warehouse.API.Data.Models.DTO_s.Responses.Department;

public record GetAllDepartmentsResponse(int Id, string Name, List<UsersResponse> Employees);