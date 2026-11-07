using ErrorOr;

namespace LibraryManagement.Api.Errors;

public static class MemberErrors
{
    public static Error NotFound(int id) =>
        Error.NotFound(description: $"A member with Id {id} has not been found.");

    public static Error Forbidden() =>
        Error.Forbidden(description: "You are not allowed to view this member's loans.");

    public static Error ConflictHasActiveLoans(int id) =>
        Error.Conflict(description: $"Member with Id {id} has active loans and cannot be deactivated.");
}
