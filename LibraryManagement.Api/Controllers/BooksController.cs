using LibraryManagement.Api.DTOs.Books;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Filters;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.RequestParameters.Book;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[Authorize(Roles = Roles.Librarian)]
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(List<ReadBookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReadBookDto>>> GetAll(
        [FromQuery] PaginationParameters pagination,
        [FromQuery] BookFilteringParameters filteringParameters,
        [FromQuery] SortingParameters<BookSortBy> sortingParameters)
    {
        var result = await _booksService.GetAllAsync(
            pagination,
            filteringParameters,
            sortingParameters);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        Response.AppendPaginationHeaders(result.Value);

        return Ok(result.Value.Items);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReadBookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadBookDto>> GetById(int id)
    {
        var result = await _booksService.GetByIdAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CreateBookDto>))]
    [ProducesResponseType(typeof(ReadBookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReadBookDto>> Create(CreateBookDto createDto)
    {
        var result = await _booksService.CreateAsync(createDto);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value.Id },
            result.Value);
    }

    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(ValidationFilter<UpdateBookDto>))]
    [ProducesResponseType(typeof(ReadBookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReadBookDto>> Update(int id, UpdateBookDto updateDto)
    {
        var result = await _booksService.UpdateAsync(id, updateDto);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _booksService.DeleteAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return NoContent();
    }
}