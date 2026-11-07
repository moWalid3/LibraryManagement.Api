using LibraryManagement.Api.DTOs.Authors;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Filters;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[Authorize(Roles = Roles.Librarian)]
[ServiceFilter(typeof(LogActionFilter))]
[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorsService _authorsService;

    public AuthorsController(IAuthorsService authorsService)
    {
        _authorsService = authorsService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(List<ReadAuthorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReadAuthorDto>>> GetAll(
        [FromQuery] PaginationParameters pagination)
    {
        var result = await _authorsService.GetAllAsync(pagination);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        Response.AppendPaginationHeaders(result.Value);

        return Ok(result.Value.Items);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReadAuthorDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadAuthorDetailDto>> GetById(int id)
    {
        var result = await _authorsService.GetByIdAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }


    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CreateAuthorDto>))]
    [ProducesResponseType(typeof(ReadAuthorDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReadAuthorDetailDto>> Create(CreateAuthorDto createDto)
    {
        var result = await _authorsService.CreateAsync(createDto);

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
    [ServiceFilter(typeof(ValidationFilter<UpdateAuthorDto>))]
    [ProducesResponseType(typeof(ReadAuthorDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReadAuthorDetailDto>> Update(
        int id,
        UpdateAuthorDto updateDto)
    {
        var result = await _authorsService.UpdateAsync(id, updateDto);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _authorsService.DeleteAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return NoContent();
    }
}