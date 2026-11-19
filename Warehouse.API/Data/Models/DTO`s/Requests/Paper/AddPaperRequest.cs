namespace Warehouse.API.Data.Models.DTO_s.Requests.Paper;

public record AddPaperRequest(
    string Format,
    int Quantity);