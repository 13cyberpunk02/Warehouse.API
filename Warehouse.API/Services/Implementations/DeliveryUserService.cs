using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.DeliveryUser;
using Warehouse.API.Data.Models.DTO_s.Responses.DeliveryUser;
using Warehouse.API.Data.Models.Error;
using Warehouse.API.Data.Models.Error.ErrorTypes;
using Warehouse.API.Data.Models.Error.ErrorTypes.AccountErrors;
using Warehouse.API.Data.Models.Error.ErrorTypes.AuthErrors;
using Warehouse.API.Data.Models.Error.ErrorTypes.DepartmentErrors;
using Warehouse.API.Data.Models.Result;
using Warehouse.API.Data.Validators.AccountValidators;
using Warehouse.API.Services.Interfaces;

namespace Warehouse.API.Services.Implementations;

public class DeliveryUserService(
    DataContext context,
    UpdateDeliveryUserRequestValidator updateDeliveryUserRequestValidator,
    CreateDeliveryUserRequestValidator createDeliveryUserRequestValidator) : IDeliveryUserService
{
    public async Task<Result> GetAllDeliveryUsers()
    {
        var users = await context.DeliveryUsers
            .Select(user => new DeliveryUserResponse(
                user.Id,
                user.Fullname,
                user.Firstname,
                user.Lastname,
                user.Patronymic,
                user.DepartmentId,
                context.Departments
                    .Where(department => department.Id == user.DepartmentId)
                    .Select(department => department.Name)
                    .FirstOrDefault() ?? "Без отдела"                
            )).ToListAsync();
        return Result.Success(users);
    }

    public async Task<Result> GetDeliveryUserByIdAsync(int id)
    {
        if (id == 0 || id < 1)
            return Result.Failure(AccountErrors.UserIdNotProvided);
        
        var user = await context.DeliveryUsers
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Id == id);
        if(user == null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        return Result.Success(new DeliveryUserResponse(
                Id : user.Id,
                Fullname : user.Fullname,
                Firstname: user.Firstname,
                Lastname: user.Lastname,
                Patronymic: user.Patronymic,
                DepartmentId : user.DepartmentId,
                Department : user.Department.Name
            )
        );
    }
    public async Task<Result> GetDeliveryUserByDepartmentIdAsync(int departmentId)
    {
        var isDepartmentExist = await context.Departments.AnyAsync(d => d.Id == departmentId);
        if(!isDepartmentExist)
            return Result.Failure(DepartmentErrors.DepartmentNotFound);
        var users = await context.DeliveryUsers
            .Include(deliveryUser => deliveryUser.Department)
            .Where(x => x.DepartmentId == departmentId)
            .ToListAsync();
        
        return Result.Success(users.Select(user => new DeliveryUserResponse(
                Id : user.Id,
                Fullname : user.Fullname,
                Firstname: user.Firstname,
                Lastname : user.Lastname,
                Patronymic : user.Patronymic,
                DepartmentId : user.DepartmentId,
                Department : user.Department.Name
            )) 
        );
    }
    public async Task<Result> UpdateDeliveryUserAsync(UpdateDeliveryUserRequest request)
    {
        var modelValidator = await updateDeliveryUserRequestValidator.ValidateAsync(request);
        if(!modelValidator.IsValid)
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidator.Errors.Select(x => x.ErrorMessage)));

        var user = await context.DeliveryUsers.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(user == null)
            return Result.Failure(AccountErrors.UserForUpdateNotFound);
        
        user.Patronymic = request.Patronymic;
        user.Firstname = request.Firstname;
        user.Lastname = request.Lastname;
        user.DepartmentId = request.DepartmentId;
        
        var result = await context.SaveChangesAsync();
        if(result < 1)
            return Result.Failure(new ErrorResponse(ErrorTypeConstant.InternalServerError, "Ошибка при записи в БД"));

        return Result.Success($"Данные пользователя {user.Firstname} {user.Lastname} успешно обновлены");
    }
    public async Task<Result> DeleteDeliveryUserAsync(int id)
    {
        if (id == 0 || id < 1)
            return Result.Failure(AccountErrors.UserIdNotProvided);
        var user = await context.DeliveryUsers.FirstOrDefaultAsync(x => x.Id == id);
        if(user is null)
            return Result.Failure(AccountErrors.UserNotFound);
        
        context.DeliveryUsers.Remove(user);
        
        var result = await context.SaveChangesAsync();
        if(result < 1)
            return Result.Failure(new ErrorResponse(ErrorTypeConstant.InternalServerError, "Ошибка при записи в БД"));
        return Result.Success("Пользователь успешно удален");
    }

    public async Task<Result> CreateDeliveryUserAsync(CreateDeliveryUserRequest request)
    {
        var modelValidation = await createDeliveryUserRequestValidator.ValidateAsync(request);
        if(!modelValidation.IsValid) 
            return Result.Failure(ErrorsCollection.ErrorCollection(modelValidation.Errors.Select(x => x.ErrorMessage)));
        var user = await context.DeliveryUsers
            .FirstOrDefaultAsync(x => (x.Firstname == request.Firstname 
                                       && x.Lastname == request.Lastname 
                                       && x.DepartmentId == request.DepartmentId));

        if (user is not null)
            return Result.Failure(AuthErrors.UserAlreadyExists);

        var userToAdd = new DeliveryUser
        {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Patronymic = request.Patronymic,
            DepartmentId = request.DepartmentId
        };
        
        await context.DeliveryUsers.AddAsync(userToAdd);
        var result = await context.SaveChangesAsync();
        if (result < 1)
            return Result.Failure(new ErrorResponse(ErrorTypeConstant.InternalServerError, "Ошибка при записи в БД"));
        return Result.Success("Пользователь успешно добавлен");
    }
}