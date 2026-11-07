using FluentValidation;
using LibraryManagement.Api.DTOs.Authors;

namespace LibraryManagement.Api.Validators.Authors;

public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
{
    public UpdateAuthorDtoValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(dto => dto.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(dto => dto.YearOfBirth)
            .InclusiveBetween(-2000, DateTime.Now.Year)
            .When(dto => dto.YearOfBirth.HasValue);
    }
}
