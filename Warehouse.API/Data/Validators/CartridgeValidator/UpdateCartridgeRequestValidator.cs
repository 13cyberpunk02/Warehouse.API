using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Cartridge;

namespace Warehouse.API.Data.Validators.CartridgeValidator;

public class UpdateCartridgeRequestValidator : AbstractValidator<UpdateCartridgeRequest>
{
    public UpdateCartridgeRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Id картриджа не отправлен или пустой")
            .NotEmpty().WithMessage("Id картриджа не отправлен или пустой");
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Модель картриджа не может быть пустым полем")
            .NotNull().WithMessage("Модель картриджа не может быть пустым полем")
            .MaximumLength(50).WithMessage("Модель картриджа может иметь до 50 символов или букв")
            .MinimumLength(2).WithMessage("Модель картриджа должен содержать не менее 2 символов или букв");

        RuleFor(x => x.Quantity)
            // Остаток 0 допустим: картриджи закончились, но позиция остаётся в справочнике
            .InclusiveBetween(0, 999).WithMessage("Остаток картриджей должен быть от 0 до 999");
    }
}