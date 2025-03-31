using FluentValidation;
using Warehouse.API.Data.Models.DTO_s.Requests.Department;

namespace Warehouse.API.Data.Validators.Department;

public class UpdateDepartmentRequestValidator : AbstractValidator<UpdateDepartmentRequest>
{
    public UpdateDepartmentRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id отдела не может быть пустым")
            .NotNull().WithMessage("Id отдела не может быть пустым");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Наименование отдела не может быть пустым")
            .NotNull().WithMessage("Наименование отдела не может быть пустым")
            .MinimumLength(2).WithMessage("Наименование отдела должно содержать хотя бы 2 буквы")
            .MaximumLength(30).WithMessage("Наименование отдела может содержать до 30 букв");
    }
}