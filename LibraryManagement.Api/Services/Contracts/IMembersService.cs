using System.Security.Claims;
using ErrorOr;
using LibraryManagement.Api.CustomResults;
using LibraryManagement.Api.DTOs.Loans;
using LibraryManagement.Api.DTOs.Members;
using LibraryManagement.Api.RequestParameters;

namespace LibraryManagement.Api.Services.Contracts;

public interface IMembersService
{
    Task<ErrorOr<PagedResult<ReadMemberDto>>> GetAllAsync(PaginationParameters pagination);
    Task<ErrorOr<ReadMemberDto>> GetByIdAsync(int id);
    Task<ErrorOr<List<ReadLoanDto>>> GetLoansAsync(int id, ClaimsPrincipal claimsPrincipal);
    Task<ErrorOr<ReadMemberDto>> UpdateAsync(int id, UpdateMemberDto updateDto);
    Task<ErrorOr<Deleted>> DeleteAsync(int id);
    Task<ErrorOr<Deleted>> RestoreAsync(int id);
}
