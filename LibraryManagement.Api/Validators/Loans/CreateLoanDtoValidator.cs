using FluentValidation;
using LibraryManagement.Api.DTOs.Loans;

namespace LibraryManagement.Api.Validators.Loans;

public class CreateLoanDtoValidator : AbstractValidator<CreateLoanDto>
{
    public CreateLoanDtoValidator()
    {
        RuleFor(x => x.BookId)
            .GreaterThan(0)
            .WithMessage("Book ID must be greater than zero.");

        RuleFor(x => x.MemberId)
            .GreaterThan(0)
            .WithMessage("Member ID must be greater than zero.");

        RuleFor(x => x.LoanDate)
            .NotEmpty()
            .WithMessage("Loan date is required.");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required.")
            .GreaterThan(x => x.LoanDate)
            .WithMessage("Due date must be after the loan date.");
    }
}
