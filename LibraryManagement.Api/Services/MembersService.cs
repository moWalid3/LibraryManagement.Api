using ErrorOr;
using LibraryManagement.Api.CustomResults;
using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Loans;
using LibraryManagement.Api.DTOs.Members;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Errors;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Mapping;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagement.Api.Services;

public class MembersService : IMembersService
{
    private readonly LibraryDbContext _dbContext;

    public MembersService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<PagedResult<ReadMemberDto>>> GetAllAsync(
        PaginationParameters pagination)
    {
        var query = _dbContext.Members.OrderBy(member => member.Id);

        return await query.ToPagedResult(
            pagination,
            member => member.ToReadDto());
    }

    public async Task<ErrorOr<ReadMemberDto>> GetByIdAsync(int id)
    {
        var member = await _dbContext.Members.FindAsync(id);

        if (member is null)
        {
            return MemberErrors.NotFound(id);
        }

        return member.ToReadDto();
    }

    public async Task<ErrorOr<List<ReadLoanDto>>> GetLoansAsync(
        int id,
        ClaimsPrincipal claimsPrincipal)
    {
        var member = await _dbContext.Members.FindAsync(id);

        if (member is null)
        {
            return MemberErrors.NotFound(id);
        }

        var currentUserId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        var isLibrarian = claimsPrincipal.IsInRole(Roles.Librarian);

        if (!isLibrarian && member.UserId != currentUserId)
        {
            return MemberErrors.Forbidden();
        }

        var loans = await _dbContext.Loans
            .Include(loan => loan.Book)
            .Include(loan => loan.Member)
            .Where(loan => loan.MemberId == id)
            .OrderByDescending(loan => loan.LoanDate)
            .ThenByDescending(loan => loan.Id)
            .ToListAsync();

        return loans
            .Select(loan => loan.ToReadDto())
            .ToList();
    }

    public async Task<ErrorOr<ReadMemberDto>> UpdateAsync(
        int id,
        UpdateMemberDto updateDto)
    {
        var existingMember = await _dbContext.Members.FindAsync(id);

        if (existingMember is null)
        {
            return MemberErrors.NotFound(id);
        }

        existingMember.UpdateFromDto(updateDto);
        await _dbContext.SaveChangesAsync();

        return existingMember.ToReadDto();
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(int id)
    {
        var hasActiveLoans = await _dbContext.Loans.AnyAsync(
            loan => loan.MemberId == id && loan.ReturnDate == null);

        if (hasActiveLoans)
        {
            return MemberErrors.ConflictHasActiveLoans(id);
        }

        return await SetMemberActiveStatusAsync(id, false);
    }

    public async Task<ErrorOr<Deleted>> RestoreAsync(int id)
    {
        return await SetMemberActiveStatusAsync(id, true);
    }

    private async Task<ErrorOr<Deleted>> SetMemberActiveStatusAsync(
        int id,
        bool isActive)
    {
        var member = await _dbContext.Members.FindAsync(id);

        if (member is null)
        {
            return MemberErrors.NotFound(id);
        }

        member.IsActive = isActive;
        await _dbContext.SaveChangesAsync();

        return Result.Deleted;
    }
}
