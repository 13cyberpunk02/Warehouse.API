using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Delivery;

namespace Warehouse.API.Data.Validators.DeliveryValidators;

public class AddDeliveryRequestValidator : AbstractValidator<AddDeliveryRequest>
{
    public AddDeliveryRequestValidator()
    {
        RuleFor(x => x.ReceivedUserId)
            .NotEmpty().WithMessage("Id получившего сотрудника обязательна")
            .NotNull().WithMessage("Id получившего сотрудника обязательна");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Id объекта выдачи обязательна к заполенению")
            .NotNull().WithMessage("Id объекта выдачи обязательна к заполенению");
        
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("Формат или модель объекта обязательна к заполенению")
            .NotNull().WithMessage("Формат или модель объекта обязательна к заполенению");
        
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Количество выданной бумаги обязательна к заполнению")
            .NotNull().WithMessage("Количество выданной бумаги обязательна к заполнению")
            .GreaterThan(0).WithMessage("Количество должно быть больше нуля");
    }
}