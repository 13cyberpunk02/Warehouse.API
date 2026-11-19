namespace Warehouse.API.Data.Models.DTO_s.Requests.DeliveryUser;

public record CreateDeliveryUserRequest(
    string Firstname,
    string Lastname,
    string Patronymic,
    int DepartmentId);