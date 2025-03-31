using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IUserRoleService
{
    Task<Result> SetRoleToUser(string userId, string roleName);
    Task<Result> RemoveUserRoles(string userId);
    Task<Result> GetAllRoles();
}