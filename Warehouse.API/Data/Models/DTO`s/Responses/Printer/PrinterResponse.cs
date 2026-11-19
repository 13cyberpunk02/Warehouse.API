namespace Warehouse.API.Data.Models.DTO_s.Responses.Printer;

public record PrinterCartridgeResponse(int Id, string Model, int Quantity);

public record PrinterResponse(int Id, string Model, string Note, List<PrinterCartridgeResponse> Cartridges);
