namespace Warehouse.API.Data.Models.DTO_s.Requests.Account;

public record BanAccountRequest(string UserId, DateTimeOffset BanUntilDate);