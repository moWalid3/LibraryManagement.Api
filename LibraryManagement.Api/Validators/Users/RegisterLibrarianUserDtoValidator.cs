using FluentValidation;
using LibraryManagement.Api.DTOs.Users;

namespace LibraryManagement.Api.Validators.Users;

public class RegisterLibrarianUserDtoValidator : AbstractValidator<RegisterLibrarianUserDto>
{
    public RegisterLibrarianUserDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty()
            .WithMessage("Email must not be empty")
            .MaximumLength(300)
            .WithMessage("Email can not exceed 300 characters")
            .EmailAddress()
            .WithMessage("Must be a valid email address");

        RuleFor(dto => dto.Password)
            .NotEmpty()
            .WithMessage("Password must not be empty")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters")
            .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
    }
}
