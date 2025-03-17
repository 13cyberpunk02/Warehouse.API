using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Account;
using Warehouse.API.Data.Models.DTO_s.Responses.Account;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.AccountErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Validators.AccountValidators;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class AccountService(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager,
    UpdateAccountRequestValidator updateAccountRequestValidator,
    BanAccountRequestValidator banAccountRequestValidator,
    SetRoleToUserValidator setRoleToUserValidator) : IAccountService
{
    public async Task<Result> GetAllAccountsAsync()
    {
        var users = await userManager.Users.ToListAsync();
        if(users.Count == 0)
            return Result.Failure(AccountErrors.EmptyUsersListError);
        
        var response = users.Select(x => new UsersResponse(
            Id: x.Id,
            Email: x.Email,
            Fullname: x.Fullname,
            AvatarImageUrl: x.AvatarImageUrl,
            DepartmentId: x.DepartmentId,
            Department: x.Department.Name
        ));
        return Result.Success(response);
    }

    public async Task<Result> GetAccountByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            return Result.Failure(AccountErrors.UserIdNotProvided);
        
        var user = await userManager.FindByIdAsync(id);
        if(user == null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        return Result.Success(new UsersResponse(
            Id: user.Id,
            Email: user.Email,
            Fullname: user.Fullname,
            AvatarImageUrl: user.AvatarImageUrl,
            DepartmentId: user.DepartmentId,
            Department: user.Department.Name
        ));
    }

    public async Task<Result> GetAccountsByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            return Result.Failure(AccountErrors.UserEmailNotProvided);
        
        var user = await userManager.FindByEmailAsync(email);
        if(user == null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        return Result.Success(new UsersResponse(
            Id: user.Id,
            Email: user.Email,
            Fullname: user.Fullname,
            AvatarImageUrl: user.AvatarImageUrl,
            DepartmentId: user.DepartmentId,
            Department: user.Department.Name
        ));
    }

    public async Task<Result> UpdateAccountAsync(UpdateAccountRequest request)
    {
        var modelValidator = await updateAccountRequestValidator.ValidateAsync(request);
        if(!modelValidator.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidator.Errors.Select(x => x.ErrorMessage)));

        var user = await userManager.FindByEmailAsync(request.Email);
        if(user == null)
            return Result.Failure(AccountErrors.UserForUpdateNotFound);
        
        user.Email = request.Email;
        user.Firstname = request.Firstname;
        user.Lastname = request.Lastname;
        user.DepartmentId = request.DepartmentId;
        user.AvatarImageUrl = request.AvatarImageUrl;
        
        var result = await userManager.UpdateAsync(user);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));

        return Result.Success($"Данные пользователя {user.Firstname} {user.Lastname} успешно обновлены");
    }

    public async Task<Result> BanAccountAsync(BanAccountRequest request)
    {
        var modelValidator = await banAccountRequestValidator.ValidateAsync(request);
        if(!modelValidator.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidator.Errors.Select(x => x.ErrorMessage)));
        
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure(AccountErrors.UserNotFound);

        var result = await userManager.SetLockoutEndDateAsync(user, request.BanUntilDate);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));
        
        return Result.Success($"Пользователь ${user.Fullname} заблокирован успешно до ${request.BanUntilDate.ToLocalTime().Date}");
    }

    public async Task<Result> UnbanAccountAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
            return Result.Failure(AccountErrors.UserIdNotProvided);
        
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        var result = await userManager.SetLockoutEndDateAsync(user, null);
        if (!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));

        return Result.Success($"Пользователь {user.Fullname} разблокирован");
    }

    public async Task<Result> SetRoleToAccountAsync(SetRoleToUserRequest request)
    {
        var modelValidation = await setRoleToUserValidator.ValidateAsync(request);
        if(!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));
        
        var user = await userManager.FindByIdAsync(request.UserId);
        if(user is null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        var role = await roleManager.FindByIdAsync(request.RoleId);
        if (role is null)
            return Result.Failure(AccountErrors.RoleNotFound);
        
        var usersInRole = await userManager.GetUsersInRoleAsync(request.RoleId);
        if (usersInRole.Contains(user))
            return Result.Failure(AccountErrors.UserAlreadyHasProvidedRole);
        
        var userOldRoles = await userManager.GetRolesAsync(user);
        var removeOldRoles = await userManager.RemoveFromRolesAsync(user, userOldRoles);
        if(!removeOldRoles.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(removeOldRoles.Errors.Select(err => err.Description)));
        
        var result = await userManager.AddToRoleAsync(user, request.RoleId);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));

        return Result.Success($"Пользователю {user.Fullname} успешно назначена роль {role.Name}");
    }
}