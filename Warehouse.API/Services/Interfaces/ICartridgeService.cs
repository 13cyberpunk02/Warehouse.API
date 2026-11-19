using Warehouse.API.Data.Models.DTO_s.Requests.Cartridge;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface ICartridgeService
{
    Task<Result> GetAllCartridges();
    Task<Result> GetCartridgeById(int id);
    Task<Result> CreateCartridge(AddCartridgeRequest request);
    Task<Result> UpdateCartridge(UpdateCartridgeRequest cartridgeRequest);
    Task<Result> DeleteCartridge(int id);
}