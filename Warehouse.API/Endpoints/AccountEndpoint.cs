using Warehouse.API.Data.Models.DTO_s.Requests.Account;
using Warehouse.API.Extensions;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Endpoints;

public static class AccountEndpoint
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/account");
        group.MapGet("/get-all-accounts", GetAllAccountsAsync).RequireAuthorization();
        group.MapGet("/get-account-by-id/{userId:required}", GetAccountById).RequireAuthorization();
        group.MapGet("/get-account-by-email/{email:required}", GetAccountByEmail).RequireAuthorization();
        group.MapGet("/get-user-roles/{userId:required}", GetUserRoles).RequireAuthorization();
        group.MapPut("/update-account", UpdateAccount).RequireAuthorization("Admin");
        group.MapPut("/set-account-role", SetRoleToAccount).RequireAuthorization("Admin");
        group.MapGet("get-all-accounts-by-department-id/{departmentId:int}", GetAllUsersByDepartmentId).RequireAuthorization();
        group.MapGet("/get-all-roles", GetRoles).RequireAuthorization("Admin");
        group.MapPut("/change-password", ChangePassword).RequireAuthorization("Admin");
        group.MapDelete("/delete-account/{userId:required}", DeleteAccount).RequireAuthorization("Admin");
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
    private static async Task<IResult> SetRoleToAccount(IAccountService accountService,
        SetRoleToUserRequest setRoleToUserRequest)
    {
        var response = await accountService.SetRoleToAccountAsync(setRoleToUserRequest);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetAllUsersByDepartmentId(IAccountService service, int departmentId)
    {
        var response = await service.GetAccountByDepartmentIdAsync(departmentId);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetUserRoles(IAccountService accountService, string userId)
    {
        var response = await accountService.GetUserRolesAsync(userId);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> GetRoles(IUserRoleService userRoleService)
    {
        var response = await userRoleService.GetAllRoles();
        return response.ToHttpResponse();
    }

    private static async Task<IResult> ChangePassword(IAccountService accountService,
        ChangePasswordRequest changePasswordRequest)
    {
        var response = await accountService.ChangePasswordAsync(changePasswordRequest);
        return response.ToHttpResponse();
    }

    private static async Task<IResult> DeleteAccount(IAccountService accountService, string userId)
    {
        var response = await accountService.DeleteAccountAsync(userId);
        return response.ToHttpResponse();
    }
}