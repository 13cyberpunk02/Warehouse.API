using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Department;
using Warehouse.API.Data.Models.DTO_s.Responses.DeliveryUser;
using Warehouse.API.Data.Models.DTO_s.Responses.Department;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.DepartmentErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Validators.Department;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class DepartmentService(DataContext context, UserManager<AppUser> userManager,
    UpdateDepartmentRequestValidator updateDepartmentRequestValidator) 
    : IDepartmentService
{
    public async Task<Result> GetAllDepartments()
    {
        var departments = await context.Departments
            .Include(u => u.DeliveryUsers)
            .Select(d => new GetAllDepartmentsResponse(
                    d.Id,
                    d.Name,
                    d.DeliveryUsers
                        .Select(e => new DeliveryUserResponse(
                            e.Id,
                            e.Fullname,
                            e.Firstname,
                            e.Lastname,
                            e.Patronymic,
                            e.DepartmentId,
                            e.Department.Name
                    )).ToList()
                )
            ).ToListAsync();
        return Result.Success(departments);
    }

    public async Task<Result> GetDepartmentById(int departmentId)
    {
        var department = await context.Departments
            .Include(department => department.DeliveryUsers)
            .FirstOrDefaultAsync(x => x.Id == departmentId);
        
        if(department is null) 
            return Result.Failure(DepartmentErrors.DepartmentNotFound);

        return Result.Success(new GetAllDepartmentsResponse(
            Id: department.Id,
            Name: department.Name,
            Employees: department.DeliveryUsers.Select(e => new DeliveryUserResponse(
                Id: e.Id,
                Fullname: e.Fullname,
                Firstname: e.Firstname,
                Lastname: e.Lastname,
                Patronymic: e.Patronymic,
                DepartmentId: e.DepartmentId,
                Department: e.Department.Name
                ))
                .ToList()
        ));
    }

    public async Task<Result> GetDepartmentByName(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            return Result.Failure(DepartmentErrors.DepartmentsAddNameError);
        
        var department = await context.Departments
            .Include(department => department.DeliveryUsers)
            .FirstOrDefaultAsync(x => x.Name == name);
        
        if(department is null) 
            return Result.Failure(DepartmentErrors.DepartmentNotFound);

        return Result.Success(new GetAllDepartmentsResponse(
            Id: department.Id,
            Name: department.Name,
            Employees: department.DeliveryUsers.Select(e => new DeliveryUserResponse(
                    Id: e.Id,
                    Fullname: e.Fullname,
                    Firstname: e.Firstname,
                    Lastname: e.Lastname,
                    Patronymic: e.Patronymic,
                    DepartmentId: e.DepartmentId,
                    Department: e.Department.Name
                ))
                .ToList()
        ));
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

    public async Task<Result> UpdateDepartment(UpdateDepartmentRequest request)
    {
        var modelValidation = await updateDepartmentRequestValidator.ValidateAsync(request);
        if (!modelValidation.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(e => e.ErrorMessage)));
        
        var department = await context.Departments.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (department == null)
            return Result.Failure(DepartmentErrors.DepartmentNotFound);
        
        department.Name = request.Name;
        var result = await context.SaveChangesAsync();
        if(result == 0)
            return Result.Failure(DepartmentErrors.DepartmentDidntSave);
        return Result.Success("Отдел успешно обновлен");
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