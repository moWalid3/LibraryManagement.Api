using LibraryManagement.Api.DTOs.Loans;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Filters;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.RequestParameters.Loan;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[Authorize(Roles = Roles.Librarian)]
[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoansService _loansService;

    public LoansController(ILoansService loansService)
    {
        _loansService = loansService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReadLoanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<ReadLoanDto>>> GetAll(
        [FromQuery] PaginationParameters pagination,
        [FromQuery] LoanFilteringParameters filteringParameters,
        [FromQuery] SortingParameters<LoanSortBy> sortingParameters)
    {
        var result = await _loansService.GetAllAsync(
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

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReadLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReadLoanDto>> GetById(int id)
    {
        var result = await _loansService.GetByIdAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CreateLoanDto>))]
    [ProducesResponseType(typeof(ReadLoanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReadLoanDto>> Create(CreateLoanDto createDto)
    {
        var result = await _loansService.CreateAsync(createDto);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value.Id },
            result.Value);
    }

    [HttpPatch("{id:int}/return")]
    [ProducesResponseType(typeof(ReadLoanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReadLoanDto>> Return(int id)
    {
        var result = await _loansService.ReturnAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }
}