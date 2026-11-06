using FluentValidation;
using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Authors;
using LibraryManagement.Api.Filters;
using LibraryManagement.Api.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Controllers;

[ServiceFilter(typeof(LogActionFilter))]
[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    /*
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
    [ProducesResponseType(typeof(ReadAuthorDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadAuthorDetailDto>> GetById(int id)
    {
        var author = await _dbContext.Authors
            .Include(author => author.Books)
            .FirstOrDefaultAsync(author => author.Id == id);

        if (author is null)
        {
            return Problem(
                detail: $"An author with Id {id} has not been found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(_mapper.ToReadDetailDto(author));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReadAuthorDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReadAuthorDetailDto>> Create(
        CreateAuthorDto createDto,
        IValidator<CreateAuthorDto> validator)
    {
        var validationResult = await validator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
        }

        var author = _mapper.ToEntity(createDto);

        _dbContext.Authors.Add(author);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = author.Id },
            _mapper.ToReadDetailDto(author));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ReadAuthorDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadAuthorDetailDto>> Update(int id, UpdateAuthorDto updateDto)
    {
        var existingAuthor = await _dbContext.Authors
            .Include(author => author.Books)
            .FirstOrDefaultAsync(author => author.Id == id);

        if (existingAuthor is null)
        {
            return Problem(
                detail: $"An author with Id {id} has not been found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        _mapper.UpdateEntity(updateDto, existingAuthor);

        await _dbContext.SaveChangesAsync();

        return Ok(_mapper.ToReadDetailDto(existingAuthor));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Delete(int id)
    {
        var authorToBeDeleted = await _dbContext.Authors
            .Include(author => author.Books)
            .FirstOrDefaultAsync(author => author.Id == id);

        if (authorToBeDeleted is null)
        {
            return Problem(
                detail: $"An author with Id {id} has not been found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        if (authorToBeDeleted.Books.Count != 0)
        {
            return Problem(
                detail: $"Cannot delete the author with Id {id}, " +
                        $"because they have books assigned. " +
                        $"Please remove or reassign the books first.",
                statusCode: StatusCodes.Status409Conflict);
        }

        _dbContext.Authors.Remove(authorToBeDeleted);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
    */
}
