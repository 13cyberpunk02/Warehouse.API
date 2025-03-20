using Warehouse.API.Data.Models.DTO_s.Requests.Delivery;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IDeliveryService
{
    Task<Result> GetAllDeliveries();
    Task<Result> GetAllDeliveriesByDepartmentOrUser(GetDeliveryByUserOrDepartment request);
    Task<Result> AddDelivery(AddDeliveryRequest request);
}