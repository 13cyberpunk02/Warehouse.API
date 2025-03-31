using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Account;
using Warehouse.API.Data.Models.DTO_s.Responses.Account;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.AccountErrors;
using Warehouse.API.Data.Models.Error.ErrorTypes.DepartmentErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Validators.AccountValidators;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class AccountService(
    DataContext context,
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager,
    UpdateAccountRequestValidator updateAccountRequestValidator,
    ChangePasswordRequestValidator changePasswordRequestValidator,
    SetRoleToUserValidator setRoleToUserValidator) : IAccountService
{
    public async Task<Result> GetAllAccountsAsync()
    {
        var users = await context.Users
            .Select(user => new UsersResponse(
                user.Id,
                user.UserName,
                user.Fullname,
                user.Firstname,
                user.Lastname,
                user.DepartmentId,
                context.Departments
                    .Where(department => department.Id == user.DepartmentId)
                    .Select(department => department.Name)
                    .FirstOrDefault() ?? "Без отдела",  
                userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault()
            )).ToListAsync();
        if(users.Count == 0)
            return Result.Failure(AccountErrors.EmptyUsersListError);
        return Result.Success(users);
    }

    public async Task<Result> GetAccountByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            return Result.Failure(AccountErrors.UserIdNotProvided);
        
        var user = await context.Users
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Id == id);
        if(user == null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        return Result.Success(new UsersResponse(
                Id : user.Id,
                UserName : user.UserName,
                Fullname : user.Fullname,
                Firstname: user.Firstname,
                Lastname: user.Lastname,
                DepartmentId : user.DepartmentId,
                Department : user.Department.Name,
                RoleName: userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault()
            )
        );
    }

    public async Task<Result> GetAccountByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            return Result.Failure(AccountErrors.UserEmailNotProvided);
        
        var user = await context.Users
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Email == email);
        if(user == null)
            return Result.Failure(AccountErrors.UserNotFound);

        return Result.Success(new UsersResponse(
            Id : user.Id,
            UserName : user.UserName,
            Fullname : user.Fullname,
            Firstname: user.Firstname,
            Lastname: user.Lastname,
            DepartmentId : user.DepartmentId,
            Department : user.Department.Name,
            RoleName: userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault()
            )
        );
    }

    public async Task<Result> GetAccountByDepartmentIdAsync(int departmentId)
    {
        var isDepartmentExist = await context.Departments.AnyAsync(d => d.Id == departmentId);
        if(!isDepartmentExist)
            return Result.Failure(DepartmentErrors.DepartmentNotFound);
        var users = await context.Users
            .Include(appUser => appUser.Department)
            .Where(x => x.DepartmentId == departmentId)
            .ToListAsync();
        if(!users.Any())
            return Result.Failure(AccountErrors.UserNotFound);
        
        return Result.Success(users.Select(user => new UsersResponse(
                Id : user.Id,
                UserName : user.UserName,
                Fullname : user.Fullname,
                Firstname: user.Firstname,
                Lastname : user.Lastname,
                DepartmentId : user.DepartmentId,
                Department : user.Department.Name,
                RoleName: userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault()
            )) 
        );
    }

    public async Task<Result> UpdateAccountAsync(UpdateAccountRequest request)
    {
        var modelValidator = await updateAccountRequestValidator.ValidateAsync(request);
        if(!modelValidator.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidator.Errors.Select(x => x.ErrorMessage)));

        var user = await userManager.FindByIdAsync(request.Id);
        if(user == null)
            return Result.Failure(AccountErrors.UserForUpdateNotFound);
        
        var role = await roleManager.FindByNameAsync(request.RoleName);
        if(role == null || string.IsNullOrEmpty(role.Name))
            return Result.Failure(AccountErrors.RoleNotFound);

        if (!await userManager.IsInRoleAsync(user, role.Name))
        {
            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);
            await userManager.AddToRoleAsync(user, role.Name);
        }
        
        user.UserName = request.UserName;
        user.Firstname = request.Firstname;
        user.Lastname = request.Lastname;
        user.DepartmentId = request.DepartmentId;
        
        var result = await userManager.UpdateAsync(user);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));

        return Result.Success($"Данные пользователя {user.Firstname} {user.Lastname} успешно обновлены");
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var modelValidator = await changePasswordRequestValidator.ValidateAsync(request);
        if(!modelValidator.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidator.Errors.Select(x => x.ErrorMessage)));

        var user = await userManager.FindByIdAsync(request.UserId);
        if(user is null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        var isOldPasswordCorrect = await userManager.CheckPasswordAsync(user, request.OldPassword);
        if (!isOldPasswordCorrect)
            return Result.Failure(AccountErrors.OldPasswordIncorrect);
        
        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));
        return Result.Success($"Пароль пользователя {user.Fullname} успешно заменен");
    }

    public async Task<Result> DeleteAccountAsync(string userId)
    {
        if(string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
            return Result.Failure(AccountErrors.UserIdNotProvided);
        
        var user = await userManager.FindByIdAsync(userId);
        if(user is null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        var result = await userManager.DeleteAsync(user);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));
        return Result.Success("Пользователь успешно удален");
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
        
        var result = await userManager.AddToRoleAsync(user, role.Name);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));

        return Result.Success($"Пользователю {user.Fullname} успешно назначена роль {role.Name}");
    }

    public async Task<Result> GetUserRolesAsync(string userId)
    {
        if(string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
            return Result.Failure(AccountErrors.UserIdNotProvided);
        var user = await userManager.FindByIdAsync(userId);
        if(user is null)
            return Result.Failure(AccountErrors.UserNotFound);
        var roles = await userManager.GetRolesAsync(user);
        return Result.Success(roles);
    }
}