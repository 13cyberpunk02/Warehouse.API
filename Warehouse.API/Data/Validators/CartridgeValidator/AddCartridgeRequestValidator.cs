using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Cartridge;

namespace Warehouse.API.Data.Validators.CartridgeValidator;

public class AddCartridgeRequestValidator : AbstractValidator<AddCartridgeRequest>
{
    public AddCartridgeRequestValidator()
    {
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Модель картриджа не может быть пустым полем")
            .NotNull().WithMessage("Модель картриджа не может быть пустым полем")
            .MaximumLength(50).WithMessage("Модель картриджа может иметь до 50 символов или букв")
            .MinimumLength(2).WithMessage("Модель картриджа должен содержать не менее 2 символов или букв");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Количество картриджей не может быть пустым")
            .NotNull().WithMessage("Количество картриджей не может быть пустым")
            .Must(x => x is > 0 and < 1000).WithMessage("Количество картриджа не может быть 0 или больше 1000");
    }
}