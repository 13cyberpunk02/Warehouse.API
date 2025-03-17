using Warehouse.API.Data.Models.DTO_s.Requests.Account;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class AccountEndpoint
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/account");
        group.MapGet("/get-all-accounts", GetAllAccountsAsync);
        group.MapGet("/get-account-by-id/{userId:required}", GetAccountById);
        group.MapGet("/get-account-by-email/{email:required}", GetAccountByEmail);
        group.MapPut("/update-account", UpdateAccount);
        group.MapPut("/ban-account", BanAccount);
        group.MapPut("/unban-account/{userId:required}", UnbanAccount);
        group.MapPut("/set-account-role", SetRoleToAccount);
        return group;
    }

    private static async Task<IResult> GetAllAccountsAsync(IAccountService accountService)
    {
        var response = await accountService.GetAllAccountsAsync();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetAccountById(IAccountService accountService, string userId)
    {
        var response = await accountService.GetAccountByIdAsync(userId);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetAccountByEmail(IAccountService accountService, string email)
    {
        var response = await accountService.GetAccountByEmailAsync(email);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> UpdateAccount(IAccountService accountService,
        UpdateAccountRequest updateAccountRequest)
    {
        var response = await accountService.UpdateAccountAsync(updateAccountRequest);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> BanAccount(IAccountService accountService, BanAccountRequest banAccountRequest)
    {
        var response = await accountService.BanAccountAsync(banAccountRequest);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> UnbanAccount(IAccountService accountService, string userId)
    {
        var response = await accountService.UnbanAccountAsync(userId);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> SetRoleToAccount(IAccountService accountService,
        SetRoleToUserRequest setRoleToUserRequest)
    {
        var response = await accountService.SetRoleToAccountAsync(setRoleToUserRequest);
        return response.ToHttpResponse();
    }
}