using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Authentication;

namespace Warehouse.API.Data.Validators.AuthValidators;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Просроченный токен не отправлен в запросе")
            .NotNull().WithMessage("Просроченный токен не отправлен в запросе");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Токен для обновления токена доступа не отправлен в запросе")
            .NotNull().WithMessage("Токен для обновления токена доступа не отправлен в запросе");
    }
}