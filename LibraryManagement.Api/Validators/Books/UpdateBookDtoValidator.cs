using FluentValidation;
using LibraryManagement.Api.DTOs.Books;

namespace LibraryManagement.Api.Validators.Books;

public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
{
    public UpdateBookDtoValidator()
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
