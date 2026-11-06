using FluentValidation;
using LibraryManagement.Api.DTOs.Users;

namespace LibraryManagement.Api.Validators.Users;

public class RegisterMemberUserDtoValidator : AbstractValidator<RegisterMemberUserDto>
{
    public RegisterMemberUserDtoValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotEmpty()
            .WithMessage("FirstName must not be empty")
            .MaximumLength(50)
            .WithMessage("FirstName can not exceed 50 characters");

        RuleFor(dto => dto.LastName)
            .NotEmpty()
            .WithMessage("LastName must not be empty")
            .MaximumLength(50)
            .WithMessage("LastName can not exceed 50 characters");

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
