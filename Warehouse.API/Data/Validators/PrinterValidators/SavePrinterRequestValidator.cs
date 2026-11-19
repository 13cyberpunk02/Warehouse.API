using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Printer;

namespace Warehouse.API.Data.Validators.PrinterValidators;

public class SavePrinterRequestValidator : AbstractValidator<SavePrinterRequest>
{
    public SavePrinterRequestValidator()
    {
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Укажите модель принтера")
            .MinimumLength(2).WithMessage("Модель принтера — минимум 2 символа")
            .MaximumLength(100).WithMessage("Модель принтера — максимум 100 символов");

        RuleFor(x => x.Note)
            .MaximumLength(200).WithMessage("Заметка — максимум 200 символов");

        RuleFor(x => x.CartridgeIds)
            .Must(ids => ids is null || ids.Count <= 20).WithMessage("Слишком много картриджей для одного принтера");
    }
}
