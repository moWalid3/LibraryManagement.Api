using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly LibraryDbContext _dbContext;

    public AuthorsController(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Author>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Author>>> GetAll()
    {
        var authors = await _dbContext.Authors.ToListAsync();

        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Author), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Author>> GetById(int id)
    {
        var author = await _dbContext.Authors.FindAsync(id);

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
    public async Task<ActionResult<Author>> Create(Author author)
    {
        if (string.IsNullOrWhiteSpace(author.FirstName) ||
           string.IsNullOrWhiteSpace(author.LastName))
        {
            return Problem(
                detail: $"Author's first name and last name cannot be empty.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        _dbContext.Authors.Add(author);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }


    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Author), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Author>> Update(int id, Author author)
    {
        if (string.IsNullOrWhiteSpace(author.FirstName) ||
            string.IsNullOrWhiteSpace(author.LastName))
        {
            return Problem(
                detail: "Author's first name and last name cannot be empty.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var existingAuthor = await _dbContext.Authors.FindAsync(id);

        if (existingAuthor is null)
        {
            return Problem(
                detail: $"An author with Id {id} has not been found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        existingAuthor.FirstName = author.FirstName;
        existingAuthor.LastName = author.LastName;
        existingAuthor.YearOfBirth = author.YearOfBirth;

        await _dbContext.SaveChangesAsync();

        return Ok(existingAuthor);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var authorToBeDeleted = await _dbContext.Authors.FindAsync(id);

        if (authorToBeDeleted is null)
        {
            return Problem(
                detail: $"An author with Id {id} has not been found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        _dbContext.Authors.Remove(authorToBeDeleted);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
