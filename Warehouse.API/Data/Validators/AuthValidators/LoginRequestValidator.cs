using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Authentication;

namespace Warehouse.API.Data.Validators.AuthValidators;

public class LoginRequestValidator :AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Имя пользователя обязательна к заполнению")
            .NotNull().WithMessage("Имя пользователя обязательна к заполнению")
            .MinimumLength(2).WithMessage("Имя пользователя может содержать не меньше 2 букв")
            .MaximumLength(20).WithMessage("Имя пользователя может содержать не больше 20 букв");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен к заполнению")
            .NotNull().WithMessage("Пароль обязателен к заполнению")
            .MinimumLength(6).WithMessage("Количество символов в пароле не должно быть меньше 6");
    }
}