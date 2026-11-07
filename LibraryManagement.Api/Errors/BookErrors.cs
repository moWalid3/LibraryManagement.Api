using ErrorOr;

namespace LibraryManagement.Api.Errors;

public static class BookErrors
{
    public static Error ValidationCategoryNotFound(int id) =>
        Error.Validation(description: $"Category with Id {id} does not exist.");

    public static Error ValidationAuthorsNotFound() =>
        Error.Validation(description: "One or more of the provided author ids is not valid.");

    public static Error BookNotFound(int id) =>
        Error.NotFound(description: $"A book with Id {id} has not been found.");

    public static Error ConflictHasLoans(int id) =>
        Error.Conflict(description: $"A book with Id {id} cannot be deleted, because " +
                                    $"it has loans associated with it.");

    public static Error ConflictNotEnoughAvailableCopies(int id) =>
        Error.Conflict(description: $"A book with Id {id} cannot have TotalCopies " +
                                    $"reduced below the number of currently active loans.");
}
