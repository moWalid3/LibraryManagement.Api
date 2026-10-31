using FluentValidation;
using LibraryManagement.Api.DTOs.Authors;

namespace LibraryManagement.Api.Validators.Authors;

public class CreateAuthorDtoValidator : AbstractValidator<CreateAuthorDto>
{
    public CreateAuthorDtoValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotEmpty()
            .MinimumLength(50);

        RuleFor(dto => dto.LastName)
            .NotEmpty()
            .MinimumLength(50);

        RuleFor(dto => dto.YearOfBirth)
            .InclusiveBetween(-2000, DateTime.Now.Year)
            .When(dto => dto.YearOfBirth.HasValue);
    }
}
