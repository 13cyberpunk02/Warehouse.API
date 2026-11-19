using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.DeliveryUser;

namespace Warehouse.API.Data.Validators.AccountValidators;

public class UpdateDeliveryUserRequestValidator : AbstractValidator<UpdateDeliveryUserRequest>
{
    public UpdateDeliveryUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id пользователя не может быть пустым")
            .NotNull().WithMessage("Id пользователя не был создан/отправлен");
        
        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Имя сотрудника обязательна к заполнению")
            .NotNull().WithMessage("Имя сотрудника обязательна к заполнению")
            .MaximumLength(100).WithMessage("Имя сотрудника может содержать не более 100 букв")
            .MinimumLength(2).WithMessage("Имя сотрудника должен содержать не менее 2 букв");

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Фамилия сотрудника обязательна к заполнению")
            .NotNull().WithMessage("Фамилия сотрудника обязательна к заполнению")
            .MaximumLength(100).WithMessage("Фамилия сотрудника может содержать не более 100 букв")
            .MinimumLength(2).WithMessage("Фамилия сотрудника должна содержать не менее 2 букв");
        
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Отдел не может быть пустым")
            .NotNull().WithMessage("Отдел не может быть пустым");
    }
}