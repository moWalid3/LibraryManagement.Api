using LibraryManagement.Api.DTOs.Loans;
using LibraryManagement.Api.DTOs.Members;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Filters;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMembersService _membersService;

    public MembersController(IMembersService membersService)
    {
        _membersService = membersService;
    }

    [Authorize(Roles = Roles.Librarian)]
    [HttpGet]
    [ProducesResponseType(typeof(List<ReadMemberDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<ReadMemberDto>>> GetAll(
    [FromQuery] PaginationParameters pagination)
    {
        var result = await _membersService.GetAllAsync(pagination);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        Response.AppendPaginationHeaders(result.Value);

        return Ok(result.Value.Items);
    }

    [Authorize(Roles = Roles.Librarian)]
    [HttpGet("{id:int}", Name = "GetMemberById")]
    [ProducesResponseType(typeof(ReadMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadMemberDto>> GetById(int id)
    {
        var result = await _membersService.GetByIdAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet("{id:int}/loans")]
    [ProducesResponseType(typeof(List<ReadLoanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReadLoanDto>>> GetLoans(int id)
    {
        var result = await _membersService.GetLoansAsync(id, User);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [Authorize(Roles = Roles.Librarian)]
    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(ValidationFilter<UpdateMemberDto>))]
    [ProducesResponseType(typeof(ReadMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadMemberDto>> Update(
        int id,
        UpdateMemberDto updateDto)
    {
        var result = await _membersService.UpdateAsync(id, updateDto);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return Ok(result.Value);
    }

    [Authorize(Roles = Roles.Librarian)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _membersService.DeleteAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return NoContent();
    }

    [Authorize(Roles = Roles.Librarian)]
    [HttpPatch("{id:int}/restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Restore(int id)
    {
        var result = await _membersService.RestoreAsync(id);

        if (result.IsError)
        {
            return result.FirstError.ToProblem(this);
        }

        return NoContent();
    }
}
