using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Account;

namespace Warehouse.API.Data.Validators.AccountValidators;

public class SetRoleToUserValidator : AbstractValidator<SetRoleToUserRequest>
{
    public SetRoleToUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Id пользователя не передан")
            .NotNull().WithMessage("Id пользователя не передан");
        
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Id роли не передан")
            .NotNull().WithMessage("Id роли не передан");
    }
}