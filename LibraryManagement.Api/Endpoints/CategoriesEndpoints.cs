using LibraryManagement.Api.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagement.Api.Endpoints;

public static class CategoriesEndpoints
{
    private static List<Category> _categories =
    [
        new Category{ Id = 1, Name = "Biography"},
        new Category{ Id = 2, Name = "Novel"}
    ];

    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/categories")
            .WithTags("Category");

        group.MapGet("/", GetAll);

        group.MapGet("/{id:int}", GetById)
            .WithName("GetCategoryById")
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/", Create)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<Ok<List<Category>>> GetAll()
    {
        var categories = _categories;
        return TypedResults.Ok(categories);
    }

    private static async Task<Results<Ok<Category>, ProblemHttpResult>> GetById(int id)
    {
        var category = _categories.FirstOrDefault(category => category.Id == id);

        if (category is null)
        {
            return TypedResults.Problem(
                detail: $"A category with id {id} has not been found",
                statusCode: StatusCodes.Status404NotFound);
        }

        return TypedResults.Ok(category);
    }

    private static async Task<Results<CreatedAtRoute<Category>, ProblemHttpResult>> Create(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            return TypedResults.Problem(
                detail: "A category name can not be empty",
                statusCode: StatusCodes.Status400BadRequest);
        }

        if (_categories.Any(currentCategory => currentCategory.Id == category.Id))
        {
            return TypedResults.Problem(
                detail: $"A category with id {category.Id} already exists.",
                statusCode: StatusCodes.Status409Conflict);
        }

        if (_categories.Any(currentCategory => currentCategory.Name == category.Name))
        {
            return TypedResults.Problem(
                detail: $"A category with name {category.Name} already exists.",
                statusCode: StatusCodes.Status409Conflict);
        }

        _categories.Add(category);

        return TypedResults.CreatedAtRoute(category, "GetCategoryById", new { id = category.Id });
    }
}
