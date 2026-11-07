using ErrorOr;
using LibraryManagement.Api.CustomResults;
using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Books;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Errors;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Mapping;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.RequestParameters.Book;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Services;

public class BooksService : IBooksService
{
    private readonly LibraryDbContext _dbContext;

    public BooksService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<PagedResult<ReadBookDto>>> GetAllAsync(
        PaginationParameters pagination,
        BookFilteringParameters filteringParameters,
        SortingParameters<BookSortBy> sortingParameters)
    {
        return await _dbContext.Books
            .Include(book => book.Category)
            .Include(book => book.Authors.OrderBy(author => author.Id))
            .ApplyFiltering(filteringParameters)
            .ApplySorting(sortingParameters)
            .ToPagedResult(
                pagination,
                book => book.ToReadDto());
    }

    public async Task<ErrorOr<ReadBookDto>> GetByIdAsync(int id)
    {
        var book = await _dbContext.Books
            .Include(book => book.Category)
            .Include(book => book.Authors.OrderBy(author => author.Id))
            .FirstOrDefaultAsync(book => book.Id == id);

        if (book is null)
        {
            return BookErrors.BookNotFound(id);
        }

        return book.ToReadDto();
    }

    public async Task<ErrorOr<ReadBookDto>> CreateAsync(CreateBookDto createDto)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(
            category => category.Id == createDto.CategoryId);

        if (category is null)
        {
            return BookErrors.ValidationCategoryNotFound(createDto.CategoryId);
        }

        var uniqueAuthorIds = createDto.AuthorIds.Distinct().ToList();

        var authors = await _dbContext.Authors
            .Where(author => uniqueAuthorIds.Contains(author.Id))
            .OrderBy(author => author.Id)
            .ToListAsync();

        if (authors.Count != uniqueAuthorIds.Count)
        {
            return BookErrors.ValidationAuthorsNotFound();
        }

        var book = createDto.ToEntity(authors);

        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();

        return book.ToReadDto();
    }

    public async Task<ErrorOr<ReadBookDto>> UpdateAsync(
        int id,
        UpdateBookDto updateDto)
    {
        var existingBook = await _dbContext.Books
            .Include(book => book.Authors)
            .FirstOrDefaultAsync(book => book.Id == id);

        if (existingBook is null)
        {
            return BookErrors.BookNotFound(id);
        }

        var category = await _dbContext.Categories.FirstOrDefaultAsync(
                category => category.Id == updateDto.CategoryId);

        if (category is null)
        {
            return BookErrors.ValidationCategoryNotFound(updateDto.CategoryId);
        }

        var uniqueAuthorIds = updateDto.AuthorIds.Distinct().ToList();

        var authors = await _dbContext.Authors
            .Where(author => uniqueAuthorIds.Contains(author.Id))
            .OrderBy(author => author.Id)
            .ToListAsync();

        if (authors.Count != uniqueAuthorIds.Count)
        {
            return BookErrors.ValidationAuthorsNotFound();
        }

        var totalCopiesBeforeEntityUpdate = existingBook.TotalCopies;
        var updateCopiesResult = await UpdateAvailableCopies(
            totalCopiesBeforeEntityUpdate,
            updateDto.TotalCopies,
            existingBook);

        if (updateCopiesResult.IsError)
        {
            return updateCopiesResult.FirstError;
        }

        existingBook.UpdateFromDto(updateDto, authors);

        await _dbContext.SaveChangesAsync();

        return existingBook.ToReadDto();
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(int id)
    {
        var bookToBeDeleted = await _dbContext.Books.FindAsync(id);

        if (bookToBeDeleted is null)
        {
            return BookErrors.BookNotFound(id);
        }

        var hasLoans = await _dbContext.Loans.AnyAsync(
            loan => loan.BookId == id);

        if (hasLoans)
        {
            return BookErrors.ConflictHasLoans(id);
        }

        _dbContext.Books.Remove(bookToBeDeleted);
        await _dbContext.SaveChangesAsync();

        return Result.Deleted;
    }

    private async Task<ErrorOr<Success>> UpdateAvailableCopies(
        int oldTotalCopies,
        int newTotalCopies,
        Book existingBook)
    {
        var totalCopiesDelta = newTotalCopies - oldTotalCopies;

        if (totalCopiesDelta > 0)
        {
            existingBook.AvailableCopies += totalCopiesDelta;
        }
        else if (totalCopiesDelta < 0)
        {
            var activeLoansCount = await _dbContext.Loans
                .CountAsync(loan => loan.BookId == existingBook.Id &&
                                    loan.ReturnDate == null);

            if (newTotalCopies < activeLoansCount)
            {
                return BookErrors.ConflictNotEnoughAvailableCopies(existingBook.Id);
            }

            existingBook.AvailableCopies += totalCopiesDelta;
        }

        return Result.Success;
    }
}