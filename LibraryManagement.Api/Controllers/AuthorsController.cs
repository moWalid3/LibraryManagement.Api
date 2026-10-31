using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Authors;
using LibraryManagement.Api.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly LibraryDbContext _dbContext;
    private readonly AuthorMapper _mapper;

    public AuthorsController(
        LibraryDbContext dbContext,
        AuthorMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReadAuthorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReadAuthorDto>>> GetAll()
    {
        var authors = await _dbContext.Authors.ToListAsync();

        var readDtos = authors
            .Select(_mapper.ToReadDto)
            .ToList();

        return Ok(readDtos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReadAuthorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadAuthorDto>> GetById(int id)
    {
        var author = await _dbContext.Authors.FindAsync(id);

        if (author is null)
        {
            return Problem(
                detail: $"An author with Id {id} has not been found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(_mapper.ToReadDto(author));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReadAuthorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReadAuthorDto>> Create(CreateAuthorDto createDto)
    {
        if (string.IsNullOrWhiteSpace(createDto.FirstName) ||
           string.IsNullOrWhiteSpace(createDto.LastName))
        {
            return Problem(
                detail: $"Author's first name and last name cannot be empty.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var author = _mapper.ToEntity(createDto);

        _dbContext.Authors.Add(author);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = author.Id },
            _mapper.ToReadDto(author));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ReadAuthorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadAuthorDto>> Update(int id, UpdateAuthorDto updateDto)
    {
        if (string.IsNullOrWhiteSpace(updateDto.FirstName) ||
            string.IsNullOrWhiteSpace(updateDto.LastName))
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

        _mapper.UpdateEntity(updateDto, existingAuthor);

        await _dbContext.SaveChangesAsync();

        return Ok(_mapper.ToReadDto(existingAuthor));
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
