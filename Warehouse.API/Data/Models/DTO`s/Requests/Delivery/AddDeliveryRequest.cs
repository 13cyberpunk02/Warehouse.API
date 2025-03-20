namespace Warehouse.API.Data.Models.DTO_s.Requests.Delivery;

public record AddDeliveryRequest(
    string DeliveredUserId, 
    string ReceivedUserId,
    int PaperId,
    int Quantity);