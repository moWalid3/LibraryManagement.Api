using ErrorOr;

namespace LibraryManagement.Api.Errors;

public static class AuthenticationErrors
{
    public static Error RegistrationFailed(string description) =>
        Error.Validation(description: description);

    public static Error ConflictEmailIsTaken(string email) =>
        Error.Conflict(description: $"Email '{email}' is already taken");
}
