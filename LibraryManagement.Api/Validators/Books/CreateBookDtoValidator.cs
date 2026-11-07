using FluentValidation;
using LibraryManagement.Api.DTOs.Books;

namespace LibraryManagement.Api.Validators.Books;

public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.TotalCopies)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CategoryId)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.AuthorIds)
            .NotEmpty();
    }
}
