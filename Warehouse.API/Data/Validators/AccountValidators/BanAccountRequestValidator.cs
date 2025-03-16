using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Account;

namespace Warehouse.API.Data.Validators.AccountValidators;

public class BanAccountRequestValidator : AbstractValidator<BanAccountRequest>
{
    public BanAccountRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("Id пользователя для блокировки не передан")
            .NotEmpty().WithMessage("Id пользователя для блокировки не передан");
        
        RuleFor(x => x.BanUntilDate)
            .NotNull().WithMessage("Дата до разблокировки аккаунта пользователя не передан")
            .NotEmpty().WithMessage("Дата до разблокировки аккаунта пользователя не передан")
            .Must(BeAValidDate).WithMessage("Передан неверный формат даты до разблокировки")
            .Must(date => date >= DateTimeOffset.Now).WithMessage("Дата до разблокировки должна быть в будущем");
    }

    private bool BeAValidDate(DateTimeOffset date)
    {
        return date != default(DateTimeOffset);
    }    
}