using FluentValidation;
using Warehouse.API.Data.Entities;
using Warehouse.API.Data.Models.DTO_s.Requests.Paper;

namespace Warehouse.API.Data.Validators.PaperValidator;

public class AddPaperRequestValidator : AbstractValidator<AddPaperRequest>
{
    public AddPaperRequestValidator()
    {
        RuleFor(x => x.Format)
            .NotEmpty().WithMessage("Формат бумаги обязательно к заполнению")
            .NotNull().WithMessage("Формат бумаги обязательно к заполнению")
            .MaximumLength(100).WithMessage("Формат бумаги может содержать не более 100 букв")
            .MinimumLength(2).WithMessage("Формат бумаги может содержать не менее 2 букв");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Количество бумаги обязательно к заполнению")
            .NotNull().WithMessage("Количество бумаги обязательно к заполнению")
            .Must(x => x is > 0 and < 10000)
            .WithMessage("Количество бумаги может быть не более 10000шт. и не менее 1шт.");
    }
}