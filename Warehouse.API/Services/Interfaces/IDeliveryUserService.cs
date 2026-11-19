using Warehouse.API.Data.Models.DTO_s.Requests.DeliveryUser;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IDeliveryUserService
{
    Task<Result> GetAllDeliveryUsers();
    Task<Result> GetDeliveryUserByIdAsync(int id);
    Task<Result> GetDeliveryUserByDepartmentIdAsync(int departmentId);
    Task<Result> UpdateDeliveryUserAsync(UpdateDeliveryUserRequest request);
    Task<Result> DeleteDeliveryUserAsync(int userId);
    Task<Result> CreateDeliveryUserAsync(CreateDeliveryUserRequest request);
}