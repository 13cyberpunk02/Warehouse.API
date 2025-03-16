using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Account;

namespace Warehouse.API.Data.Validators.AccountValidators;

public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Эл. почта обязательна к заполнению")
            .NotNull().WithMessage("Эл. почта обязательна к заполнению")
            .EmailAddress().WithMessage("Эл. почта заполнена неправильно");

        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Имя сотрудника обязательна к заполнению")
            .NotNull().WithMessage("Имя сотрудника обязательна к заполнению")
            .MaximumLength(100).WithMessage("Имя сотрудника может содержать не более 100 букв")
            .MinimumLength(3).WithMessage("Имя сотрудника должен содержать не менее 3 букв");

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Фамилия сотрудника обязательна к заполнению")
            .NotNull().WithMessage("Фамилия сотрудника обязательна к заполнению")
            .MaximumLength(100).WithMessage("Фамилия сотрудника может содержать не более 100 букв")
            .MinimumLength(3).WithMessage("Фамилия сотрудника должна содержать не менее 3 букв");
        
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Отдел не может быть пустым")
            .NotNull().WithMessage("Отдел не может быть пустым");
    }
}