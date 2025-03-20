namespace Warehouse.API.Data.Models.DTO_s.Requests.Delivery;

public record GetDeliveryByUserOrDepartment(string? UserId, int? DepartmentId, DateTime? StartDate, DateTime? EndDate);