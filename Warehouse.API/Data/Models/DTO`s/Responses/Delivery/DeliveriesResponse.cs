namespace Warehouse.API.Data.Models.DTO_s.Responses.Delivery;

public record DeliveriesResponse(
    int Id, 
    DateTime DeliveredAt,
    string DeliveredBy,
    string ReceivedDepartment, 
    string ReceivedUser,
    string PaperFormat,
    int Quantity);