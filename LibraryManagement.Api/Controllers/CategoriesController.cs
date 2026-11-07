using LibraryManagement.Api.DTOs.Books;
using LibraryManagement.Api.DTOs.Categories;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Filters;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[Authorize(Roles = Roles.Librarian)]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(List<ReadCategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReadCategoryDto>>> GetAll()
    {
        var result = await _categoriesService.GetAllAsync();

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReadCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadCategoryDto>> GetById(int id)
    {
        var result = await _categoriesService.GetByIdAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}/books")]
    [ProducesResponseType(typeof(List<ReadBookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReadBookDto>>> GetBooks(
        int id,
        [FromQuery] PaginationParameters pagination)
    {
        var result = await _categoriesService.GetBooksAsync(id, pagination);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        Response.AppendPaginationHeaders(result.Value);

        return Ok(result.Value.Items);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CreateCategoryDto>))]
    [ProducesResponseType(typeof(ReadCategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReadCategoryDto>> Create(CreateCategoryDto createDto)
    {
        var result = await _categoriesService.CreateAsync(createDto);

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
    [ServiceFilter(typeof(ValidationFilter<UpdateCategoryDto>))]
    [ProducesResponseType(typeof(ReadCategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReadCategoryDto>> Update(
        int id,
        UpdateCategoryDto updateDto)
    {
        var result = await _categoriesService.UpdateAsync(id, updateDto);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _categoriesService.DeleteAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return NoContent();
    }
}
