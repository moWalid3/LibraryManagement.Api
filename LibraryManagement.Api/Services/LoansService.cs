using ErrorOr;
using LibraryManagement.Api.CustomResults;
using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Loans;
using LibraryManagement.Api.Errors;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Mapping;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.RequestParameters.Loan;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Services;

public class LoansService : ILoansService
{
    private readonly LibraryDbContext _dbContext;

    public LoansService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<PagedResult<ReadLoanDto>>> GetAllAsync(
        PaginationParameters pagination,
        LoanFilteringParameters filteringParameters,
        SortingParameters<LoanSortBy> sortingParameters)
    {
        return await _dbContext.Loans
            .Include(loan => loan.Member)
            .Include(loan => loan.Book)
            .ApplyFiltering(filteringParameters)
            .ApplySorting(sortingParameters)
            .ToPagedResult(
                pagination,
                loan => loan.ToReadDto());
    }

    public async Task<ErrorOr<ReadLoanDto>> GetByIdAsync(int id)
    {
        var loan = await _dbContext.Loans
            .Include(loan => loan.Member)
            .Include(loan => loan.Book)
            .FirstOrDefaultAsync(loan => loan.Id == id);

        if (loan is null)
        {
            return LoanErrors.LoanNotFound(id);
        }

        return loan.ToReadDto();
    }

    public async Task<ErrorOr<ReadLoanDto>> CreateAsync(CreateLoanDto createDto)
    {
        var book = await _dbContext.Books.FirstOrDefaultAsync(
            book => book.Id == createDto.BookId);

        if (book is null)
        {
            return LoanErrors.ValidationBookNotFound(createDto.BookId);
        }

        if (book.AvailableCopies == 0)
        {
            return LoanErrors.ConflictNoAvailableCopies(createDto.BookId);
        }

        var member = await _dbContext.Members.FirstOrDefaultAsync(
           member => member.Id == createDto.MemberId);

        if (member is null)
        {
            return LoanErrors.ValidationMemberNotFound(createDto.MemberId);
        }

        if (!member.IsActive)
        {
            return LoanErrors.ConflictMemberInactive(createDto.MemberId);
        }

        var loan = createDto.ToEntity();

        book.AvailableCopies--;

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan.ToReadDto();
    }

    public async Task<ErrorOr<ReadLoanDto>> ReturnAsync(int id)
    {
        var loan = await _dbContext.Loans
            .Include(loan => loan.Member)
            .Include(loan => loan.Book)
            .FirstOrDefaultAsync(loan => loan.Id == id);

        if (loan is null)
        {
            return LoanErrors.LoanNotFound(id);
        }

        if (loan.ReturnDate is not null)
        {
            return LoanErrors.ConflictAlreadyReturned(id);
        }

        loan.ReturnDate = DateOnly.FromDateTime(DateTime.Today);
        loan.Book!.AvailableCopies++;

        await _dbContext.SaveChangesAsync();

        return loan.ToReadDto();
    }
}