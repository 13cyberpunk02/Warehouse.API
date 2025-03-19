namespace Warehouse.API.Data.Models.DTO_s.Requests.Paper;

public record AddPaperRequest(
    string Name,
    string Type,
    string Format,
    int Quantity);