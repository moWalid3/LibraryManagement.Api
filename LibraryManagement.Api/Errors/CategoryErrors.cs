using ErrorOr;

namespace LibraryManagement.Api.Errors;

public static class CategoryErrors
{
    public static Error NotFound(int id) =>
        Error.NotFound(description: $"A category with Id {id} has not been found.");

    public static Error ConflictNameAlreadyTaken(string name) =>
        Error.Conflict(description: $"The category name '{name}' is already taken.");

    public static Error ConflictHasBooksAssigned(int id) =>
        Error.Conflict(description: $"Cannot delete the category with Id {id}, because it has books assigned.");
}
