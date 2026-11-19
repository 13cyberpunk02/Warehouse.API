namespace Warehouse.API.Data.Models.DTO_s.Requests.Printer;

/// <summary>Создание и изменение принтера: модель, заметка и подходящие картриджи.</summary>
public record SavePrinterRequest(string Model, string? Note, List<int>? CartridgeIds);
