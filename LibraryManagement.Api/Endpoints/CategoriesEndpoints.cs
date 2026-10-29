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
        group.MapGet("/{id:int}", GetById);
        //.Produces<Category>(StatusCodes.Status200OK)
        //.Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<Ok<List<Category>>> GetAll()
    {
        var categories = _categories;
        return TypedResults.Ok(categories);
    }

    private static async Task<Results<Ok<Category>, NotFound>> GetById(int id)
    {
        var category = _categories.FirstOrDefault(category => category.Id == id);

        if (category is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(category);
    }
}
