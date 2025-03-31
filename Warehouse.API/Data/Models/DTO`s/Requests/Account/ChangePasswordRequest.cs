namespace Warehouse.API.Data.Models.DTO_s.Requests.Account;

public record ChangePasswordRequest(string UserId, string OldPassword, string NewPassword);