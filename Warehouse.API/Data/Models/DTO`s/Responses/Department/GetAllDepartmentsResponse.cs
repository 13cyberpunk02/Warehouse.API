using Warehouse.API.Data.Models.DTO_s.Responses.DeliveryUser;

namespace Warehouse.API.Data.Models.DTO_s.Responses.Department;

public record GetAllDepartmentsResponse(int Id, string Name, List<DeliveryUserResponse> Employees);