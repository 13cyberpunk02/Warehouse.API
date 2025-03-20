using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Responses.Account;
using Warehouse.API.Data.Models.DTO_s.Responses.Department;
using Warehouse.API.Data.Models.Error.ErrorTypes.DepartmentErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class DepartmentService(DataContext context) : IDepartmentService
{
    public async Task<Result> GetAllDepartments()
    {
        var departments = await context.Departments
            .Select(d => new GetAllDepartmentsResponse(
                    d.Id,
                    d.Name,
                    d.Employee.Select(e => new UsersResponse(
                        e.Id,
                        e.Email,
                        e.Fullname,
                        e.AvatarImageUrl,
                        e.DepartmentId,
                        e.Department.Name
                    )).ToList()
                )
            ).ToListAsync();
            
        if (!departments.Any())
            return Result.Failure(DepartmentErrors.DepartmentsIsEmpty);
        return Result.Success(departments);
    }

    public async Task<Result> GetDepartmentById(int departmentId)
    {
        var department = await context.Departments
            .Where(d => d.Id == departmentId)
            .Select(d => new GetAllDepartmentsResponse(
                d.Id,
                d.Name,
                d.Employee.Select(e => new UsersResponse(
                        e.Id,
                        e.Email,
                        e.Fullname,
                        e.AvatarImageUrl,
                        e.DepartmentId,
                        e.Department.Name))
                    .ToList()
                ))
            .ToListAsync();
        
        if(!department.Any())
            return Result.Failure(DepartmentErrors.DepartmentNotFound);
        return Result.Success(department);
    }

    public async Task<Result> GetDepartmentByName(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            return Result.Failure(DepartmentErrors.DepartmentsAddNameError);
        
        var department = await context.Departments
            .Where(d => d.Name == name)
            .Select(d => new GetAllDepartmentsResponse(
                d.Id,
                d.Name,
                d.Employee.Select(e => new UsersResponse(
                        e.Id,
                        e.Email,
                        e.Fullname,
                        e.AvatarImageUrl,
                        e.DepartmentId,
                        e.Department.Name))
                    .ToList()
            ))
            .ToListAsync();
        
        if(!department.Any())
            return Result.Failure(DepartmentErrors.DepartmentNotFound);
        return Result.Success(department);
    }

    public async Task<Result> AddDepartment(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            return Result.Failure(DepartmentErrors.DepartmentsAddNameError);

         
        context.Departments.Add(new Department{ Name = name });
        var result = await context.SaveChangesAsync();
        if(result == 0)
            return Result.Failure(DepartmentErrors.DepartmentDidntSave);
        
        return Result.Success("Отдел успешно добавлен");
    }

    public Task<Result> UpdateDepartment(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteDepartment(int id)
    {
        var department = await context.Departments.FirstOrDefaultAsync(x => x.Id == id);
        if(department is null)
            return Result.Failure(DepartmentErrors.DepartmentNotFound);
        
        context.Departments.Remove(department);
        var result = await context.SaveChangesAsync();
        if(result == 0)
            return Result.Failure(DepartmentErrors.DepartmentDidntSave);
        return Result.Success("Отдел успешно удален");
    }
}