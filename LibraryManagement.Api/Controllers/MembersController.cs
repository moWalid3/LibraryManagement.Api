using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Members;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly LibraryDbContext _dbContext;

    public MembersController(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id:int}", Name = "GetMemberById")]
    //[Authorize(Roles = Roles.Librarian)]
    [ProducesResponseType(typeof(ReadMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadMemberDto>> GetById(int id)
    {
        var member = await _dbContext.Members.FindAsync(id);

        if (member is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                detail: $"Member with ID {id} was not found.");
        }

        return Ok(member.ToReadDto());
    }


    //// GET: api/members?pageNumber=1&pageSize=10
    //[HttpGet]
    //[Authorize(Roles = Roles.Librarian)]
    //[ProducesResponseType(typeof(List<ReadMemberDto>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[ProducesResponseType(StatusCodes.Status403Forbidden)]
    //public async Task<ActionResult<List<ReadMemberDto>>> GetAll(
    //    [FromQuery] int pageNumber = PaginationParameters.DefaultPageNumber,
    //    [FromQuery] int pageSize = PaginationParameters.DefaultPageSize)
    //{
    //    var pagination = new PaginationParameters
    //    {
    //        PageNumber = pageNumber,
    //        PageSize = pageSize
    //    };

    //    var query = _dbContext
    //        .Members
    //        .OrderBy(member => member.Id);

    //    var pagedResult = await query.ToPagedResult(
    //        pagination,
    //        member => _memberMapper.ToReadDto(member));

    //    Response.AppendPaginationHeaders(pagedResult);

    //    return Ok(pagedResult.Items);
    //}

    //// GET: api/members/5/loans
    //[HttpGet("{id:int}/loans")]
    //[Authorize]
    //[ProducesResponseType(typeof(List<ReadLoanDto>), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[ProducesResponseType(StatusCodes.Status403Forbidden)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<List<ReadLoanDto>>> GetLoans(int id)
    //{
    //    var member = await _dbContext.Members.FindAsync(id);

    //    if (member is null)
    //    {
    //        return Problem(
    //            statusCode: StatusCodes.Status404NotFound,
    //            detail: $"Member with ID {id} was not found.");
    //    }

    //    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var isLibrarian = User.IsInRole(Roles.Librarian);

    //    if (!isLibrarian && member.UserId != currentUserId)
    //    {
    //        return Forbid();
    //    }

    //    var loans = await _dbContext.Loans
    //        .Include(loan => loan.Book)
    //        .Include(loan => loan.Member)
    //        .Where(loan => loan.MemberId == id)
    //        .OrderByDescending(loan => loan.LoanDate)
    //        .ThenByDescending(loan => loan.Id)
    //        .ToListAsync();

    //    var loanDtos = loans
    //        .Select(loan => _loanMapper.ToReadDto(loan))
    //        .ToList();

    //    return Ok(loanDtos);
    //}

    //// PUT: api/members/5
    //[HttpPut("{id:int}")]
    //[Authorize(Roles = Roles.Librarian)]
    //[ProducesResponseType(typeof(ReadMemberDto), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[ProducesResponseType(StatusCodes.Status403Forbidden)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<ReadMemberDto>> Update(
    //    int id,
    //    [FromBody] UpdateMemberDto updateDto)
    //{
    //    var existingMember = await _dbContext.Members.FindAsync(id);

    //    if (existingMember is null)
    //    {
    //        return Problem(
    //            statusCode: StatusCodes.Status404NotFound,
    //            detail: $"Member with ID {id} was not found.");
    //    }

    //    _memberMapper.UpdateEntity(updateDto, existingMember);

    //    await _dbContext.SaveChangesAsync();

    //    return Ok(_memberMapper.ToReadDto(existingMember));
    //}

    //// DELETE: api/members/5
    //[HttpDelete("{id:int}")]
    //[Authorize(Roles = Roles.Librarian)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[ProducesResponseType(StatusCodes.Status403Forbidden)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status409Conflict)]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    var hasActiveLoans = await _dbContext.Loans.AnyAsync(
    //        loan => loan.MemberId == id &&
    //                loan.ReturnDate == null);

    //    if (hasActiveLoans)
    //    {
    //        return Problem(
    //            statusCode: StatusCodes.Status409Conflict,
    //            detail: $"Member with ID {id} has active loans and cannot be deleted.");
    //    }

    //    return await SetMemberActiveStatus(id, false);
    //}

    //// PATCH: api/members/5/restore
    //[HttpPatch("{id:int}/restore")]
    //[Authorize(Roles = Roles.Librarian)]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[ProducesResponseType(StatusCodes.Status403Forbidden)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> Restore(int id)
    //{
    //    return await SetMemberActiveStatus(id, true);
    //}

    //private async Task<IActionResult> SetMemberActiveStatus(
    //    int id,
    //    bool isActive)
    //{
    //    var member = await _dbContext.Members.FindAsync(id);

    //    if (member is null)
    //    {
    //        return Problem(
    //            statusCode: StatusCodes.Status404NotFound,
    //            detail: $"Member with ID {id} was not found.");
    //    }

    //    member.IsActive = isActive;

    //    await _dbContext.SaveChangesAsync();

    //    return NoContent();
    //}
}
