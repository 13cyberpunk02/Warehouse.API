using Warehouse.API.Data.Models.DTO_s.Requests.Account;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IAccountService
{
    Task<Result> GetAllAccountsAsync();
    Task<Result> GetAccountByIdAsync(string id);
    Task<Result> GetAccountsByEmailAsync(string email);
    Task<Result> UpdateAccountAsync(UpdateAccountRequest request);
    Task<Result> BanAccountAsync(BanAccountRequest request);
    Task<Result> UnbanAccountAsync();
    Task<Result> SetRoleToAccountAsync(string roleId);
}