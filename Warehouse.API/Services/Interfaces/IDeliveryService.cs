using Warehouse.API.Data.Models.DTO_s.Requests.Delivery;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IDeliveryService
{
    Task<Result> GetAllPaperDeliveries();
    Task<Result> GetAllCartridgeDeliveries();
    Task<Result> AddPaperDelivery(AddDeliveryRequest request, string deliveredUserId);
    Task<Result> AddCartridgeDelivery(AddDeliveryRequest request, string deliveredUserId);
}
