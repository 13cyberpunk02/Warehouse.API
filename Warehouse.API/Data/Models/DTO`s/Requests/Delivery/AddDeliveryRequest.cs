namespace Warehouse.API.Data.Models.DTO_s.Requests.Delivery;

public record AddDeliveryRequest(
    string DeliveredUserToken, 
    string ReceivedUserId,
    int PaperId,
    string PaperFormat,
    int Quantity);