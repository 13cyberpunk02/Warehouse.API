using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Responses.Role;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.AuthErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class UserRoleService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) : IUserRoleService
{
    public async Task<Result> SetRoleToUser(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(AuthErrors.UserNotFound);
        
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is null)
            return Result.Failure(AuthErrors.RoleNotFound);
        
        var userCurrentRole = await userManager.GetRolesAsync(user);
        if (userCurrentRole.Count > 0)
            await userManager.RemoveFromRolesAsync(user, userCurrentRole);
        var result = await userManager.AddToRoleAsync(user, roleName);
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));
        return Result.Success($"Роль {roleName} успешно назначен");
    }

    public async Task<Result> RemoveUserRoles(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(AuthErrors.UserNotFound);
        
        var userCurrentRoles = await userManager.GetRolesAsync(user);
        if (userCurrentRoles.Count == 0)
            return Result.Failure(AuthErrors.RoleNotFound);
        
        var result = await userManager.RemoveFromRolesAsync(user, userCurrentRoles);
        
        if(!result.Succeeded)
            return Result.Failure(ErrorsCollection.ErrorCollection(result.Errors.Select(err => err.Description)));
        return Result.Success("Роли пользователя сняты");
    }

    public async Task<Result> GetAllRoles()
    {
        var roles = await roleManager.Roles.ToListAsync();
        var response = roles.Select(x => new RolesResponse(
            Id: x.Id,
            RoleName: x.Name))
            .ToList();
        return Result.Success(response);
    }
}