using ErrorOr;
using LibraryManagement.Api.CustomResults;
using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Authors;
using LibraryManagement.Api.Errors;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Mapping;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Services;

public class AuthorsService : IAuthorsService
{
    private readonly LibraryDbContext _dbContext;

    public AuthorsService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<PagedResult<ReadAuthorDto>>> GetAllAsync(
        PaginationParameters pagination)
    {
        var query = _dbContext.Authors.OrderBy(author => author.Id);

        return await query.ToPagedResult(
            pagination,
            author => author.ToReadDto());
    }

    public async Task<ErrorOr<ReadAuthorDetailDto>> GetByIdAsync(int id)
    {
        var author = await _dbContext.Authors
            .Include(author => author.Books.OrderBy(book => book.Id))
            .FirstOrDefaultAsync(author => author.Id == id);

        if (author is null)
        {
            return AuthorErrors.AuthorNotFound(id);
        }

        return author.ToReadDetailDto();
    }

    public async Task<ErrorOr<ReadAuthorDetailDto>> CreateAsync(CreateAuthorDto createDto)
    {
        var author = createDto.ToEntity();

        _dbContext.Authors.Add(author);
        await _dbContext.SaveChangesAsync();

        return author.ToReadDetailDto();
    }

    public async Task<ErrorOr<ReadAuthorDetailDto>> UpdateAsync(
        int id,
        UpdateAuthorDto updateDto)
    {
        var existingAuthor = await _dbContext.Authors
            .Include(author => author.Books.OrderBy(book => book.Id))
            .FirstOrDefaultAsync(author => author.Id == id);

        if (existingAuthor is null)
        {
            return AuthorErrors.AuthorNotFound(id);
        }

        existingAuthor.UpdateFromDto(updateDto);

        await _dbContext.SaveChangesAsync();

        return existingAuthor.ToReadDetailDto();
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(int id)
    {
        var authorToBeDeleted = await _dbContext.Authors
            .Include(author => author.Books)
            .FirstOrDefaultAsync(author => author.Id == id);

        if (authorToBeDeleted is null)
        {
            return AuthorErrors.AuthorNotFound(id);
        }

        if (authorToBeDeleted.Books.Count != 0)
        {
            return AuthorErrors.ConflictAuthorHasBooks(id);
        }

        _dbContext.Authors.Remove(authorToBeDeleted);
        await _dbContext.SaveChangesAsync();

        return Result.Deleted;
    }
}