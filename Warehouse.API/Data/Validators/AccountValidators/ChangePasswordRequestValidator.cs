using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Account;

namespace Warehouse.API.Data.Validators.AccountValidators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Id пользователя не может быть пустым")
            .NotNull().WithMessage("Id пользователя не был создан/отправлен");
        
        RuleFor(x => x.OldPassword)
            .NotNull().WithMessage("Старый пароль не может быть пустым")
            .NotEmpty().WithMessage("Старый пароль не может быть пустым");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Новый пароль не может быть пустым")
            .NotNull().WithMessage("Новый пароль не может быть пустым")
            .MinimumLength(6).WithMessage("Количество символов в пароле не должно быть меньше 6");
    }
}