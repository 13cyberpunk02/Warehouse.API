namespace Warehouse.API.Data.Models.DTO_s.Requests.Delivery;

// Выдающий сотрудник определяется по токену авторизации запроса, а не по полю в теле
public record AddDeliveryRequest(
    int ReceivedUserId,
    int ItemId,
    string ItemName,
    int Quantity);
