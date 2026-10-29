using LibraryManagement.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private static List<Author> _authors =
    [
        new Author { Id = 1, FirstName = "Jules", LastName = "Verne", YearOfBirth = 1828 },
        new Author { Id = 2, FirstName = "Jane", LastName = "Austen", YearOfBirth = 1775 },
        new Author { Id = 3, FirstName = "Geoffrey", LastName = "of Monmouth", YearOfBirth = null}
    ];

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

    [HttpPost]
    [ProducesResponseType(typeof(Author), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Author>> Create(Author author)
    {
        if (string.IsNullOrWhiteSpace(author.FirstName) ||
           string.IsNullOrWhiteSpace(author.LastName))
        {
            return Problem(
                detail: $"Author's first name and last name cannot be empty.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        if (_authors.Any(existingAuthor => existingAuthor.Id == author.Id))
        {
            return Problem(
                detail: $"An author with Id {author.Id} already exists.",
                statusCode: StatusCodes.Status409Conflict);
        }

        _authors.Add(author);

        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }
}
