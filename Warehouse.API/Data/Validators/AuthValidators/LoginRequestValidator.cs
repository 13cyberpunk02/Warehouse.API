using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Authentication;

namespace Warehouse.API.Data.Validators.AuthValidators;

public class LoginRequestValidator :AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Эл. почта обязательна к заполнению")
            .NotNull().WithMessage("Эл. почта обязательна к заполнению")
            .EmailAddress().WithMessage("Эл. почта введена неправильно");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен к заполнению")
            .NotNull().WithMessage("Пароль обязателен к заполнению")
            .MinimumLength(6).WithMessage("Количество символов в пароле не должно быть меньше 6");
    }
}