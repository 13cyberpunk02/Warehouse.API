using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Delivery;

namespace Warehouse.API.Data.Validators.DeliveryValidators;

public class AddDeliveryRequestValidator : AbstractValidator<AddDeliveryRequest>
{
    public AddDeliveryRequestValidator()
    {
        RuleFor(x => x.DeliveredUserToken)
            .NotEmpty().WithMessage("Токен выдавшего сотрудника обязательна")
            .NotNull().WithMessage("Токен выдавшего сотрудника обязательна");

        RuleFor(x => x.ReceivedUserId)
            .NotEmpty().WithMessage("Id получившего сотрудника обязательна")
            .NotNull().WithMessage("Id получившего сотрудника обязательна");

        RuleFor(x => x.PaperId)
            .NotEmpty().WithMessage("Id бумаги обязательна к заполенению")
            .NotNull().WithMessage("Id бумаги обязательна к заполенению");
        
        RuleFor(x => x.PaperFormat)
            .NotEmpty().WithMessage("Формат бумаги обязательна к заполенению")
            .NotNull().WithMessage("Формат бумаги обязательна к заполенению");
        
        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Количество выданной бумаги обязательна к заполнению")
            .NotNull().WithMessage("Количество выданной бумаги обязательна к заполнению");
    }
}