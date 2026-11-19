namespace Warehouse.API.Data.Models.DTO_s.Requests.Cartridge;

public record AddCartridgeRequest(
    string Model,
    int Quantity);