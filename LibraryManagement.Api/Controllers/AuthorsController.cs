using LibraryManagement.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private static List<Author> _authors = [];

    [HttpGet]
    [ProducesResponseType(typeof(List<Author>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Author>>> GetAll()
    {
        var authors = _authors;

        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Author), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Author>> GetById(int id)
    {
        var author = _authors.FirstOrDefault(author => author.Id == id);

        if (author is null)
        {
            return Problem(
                detail: $"An author with id {id} has not been found",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(author);
    }
}
