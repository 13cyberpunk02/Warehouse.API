﻿using Warehouse.API.Data.Models.DTO_s.Requests.Account;
using Warehouse.API.Data.Models.Result;

namespace Warehouse.API.Services.Interfaces;

public interface IAccountService
{
    Task<Result> GetAllAccountsAsync();
    Task<Result> GetAccountByIdAsync(string id);
    Task<Result> GetAccountByEmailAsync(string email);
    Task<Result> GetAccountByDepartmentIdAsync(int departmentId);
    Task<Result> UpdateAccountAsync(UpdateAccountRequest request);
    Task<Result> ChangePasswordAsync(ChangePasswordRequest request);
    Task<Result> DeleteAccountAsync(string userId);
    Task<Result> SetRoleToAccountAsync(SetRoleToUserRequest request);
    Task<Result> GetUserRolesAsync(string userId);
}