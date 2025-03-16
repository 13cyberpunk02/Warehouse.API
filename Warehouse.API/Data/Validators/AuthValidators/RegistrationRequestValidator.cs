using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Authentication;

namespace Warehouse.API.Data.Validators.AuthValidators;

public class RegistrationRequestValidator :AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Эл. почта обязательна к заполнению")
            .NotNull().WithMessage("Эл. почта обязательна к заполнению")
            .EmailAddress().WithMessage("Эл. почта заполнена неправильно");

        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Имя сотрудника обязательна к заполнению")
            .NotNull().WithMessage("Имя сотрудника обязательна к заполнению")
            .MaximumLength(100).WithMessage("Имя сотрудника может содержать не более 100 букв")
            .MinimumLength(3).WithMessage("Имя сотрудника может содержать не менее 3 букв");

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Фамилия сотрудника обязательна к заполнению")
            .NotNull().WithMessage("Фамилия сотрудника обязательна к заполнению")
            .MaximumLength(100).WithMessage("Фамилия сотрудника может содержать не более 100 букв")
            .MinimumLength(3).WithMessage("Фамилия сотрудника может содержать не менее 3 букв");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен к заполнению")
            .NotNull().WithMessage("Пароль обязателен к заполнению")
            .MinimumLength(6).WithMessage("Количество символов в пароле не должно быть меньше 6");

        RuleFor(X => X.ConfirmPassword)
            .NotEmpty().WithMessage("Подтверждение пароля обязательна к заполнению")
            .NotNull().WithMessage("Подтверждение пароля обязательна к заполнению")
            .Equal(p => p.Password)
            .WithMessage("Пароль подтверждения и пароль не совпадают");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Отдел не может быть пустым")
            .NotNull().WithMessage("Отдел не может быть пустым");
    }
}