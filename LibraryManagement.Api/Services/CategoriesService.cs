using ErrorOr;
using LibraryManagement.Api.CustomResults;
using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Books;
using LibraryManagement.Api.DTOs.Categories;
using LibraryManagement.Api.Errors;
using LibraryManagement.Api.Extensions;
using LibraryManagement.Api.Mapping;
using LibraryManagement.Api.RequestParameters;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Services;

public class CategoriesService : ICategoriesService
{
    private readonly LibraryDbContext _dbContext;

    public CategoriesService(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<List<ReadCategoryDto>>> GetAllAsync()
    {
        var categories = await _dbContext.Categories
            .OrderBy(category => category.Name)
            .ToListAsync();

        return categories
            .Select(category => category.ToReadDto())
            .ToList();
    }

    public async Task<ErrorOr<ReadCategoryDto>> GetByIdAsync(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);

        if (category is null)
        {
            return CategoryErrors.NotFound(id);
        }

        return category.ToReadDto();
    }

    public async Task<ErrorOr<PagedResult<ReadBookDto>>> GetBooksAsync(
        int id,
        PaginationParameters pagination)
    {
        var categoryExists = await _dbContext.Categories.AnyAsync(category => category.Id == id);

        if (!categoryExists)
        {
            return CategoryErrors.NotFound(id);
        }

        var query = _dbContext.Books
            .Include(book => book.Category)
            .Include(book => book.Authors.OrderBy(author => author.Id))
            .Where(book => book.CategoryId == id)
            .OrderBy(book => book.Id);

        return await query.ToPagedResult(
            pagination,
            book => book.ToReadDto());
    }

    public async Task<ErrorOr<ReadCategoryDto>> CreateAsync(CreateCategoryDto createDto)
    {
        if (await _dbContext.Categories.AnyAsync(
            category => category.Name == createDto.Name))
        {
            return CategoryErrors.ConflictNameAlreadyTaken(createDto.Name);
        }

        var category = createDto.ToEntity();

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        return category.ToReadDto();
    }

    public async Task<ErrorOr<ReadCategoryDto>> UpdateAsync(
        int id,
        UpdateCategoryDto updateDto)
    {
        var existingCategory = await _dbContext.Categories.FindAsync(id);

        if (existingCategory is null)
        {
            return CategoryErrors.NotFound(id);
        }

        if (await _dbContext.Categories.AnyAsync(category =>
            category.Id != id &&
            category.Name == updateDto.Name))
        {
            return CategoryErrors.ConflictNameAlreadyTaken(updateDto.Name);
        }

        existingCategory.UpdateFromDto(updateDto);
        await _dbContext.SaveChangesAsync();

        return existingCategory.ToReadDto();
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(int id)
    {
        var categoryToBeDeleted = await _dbContext.Categories.FindAsync(id);

        if (categoryToBeDeleted is null)
        {
            return CategoryErrors.NotFound(id);
        }

        var hasBooks = await _dbContext.Books.AnyAsync(book => book.CategoryId == id);

        if (hasBooks)
        {
            return CategoryErrors.ConflictHasBooksAssigned(id);
        }

        _dbContext.Categories.Remove(categoryToBeDeleted);
        await _dbContext.SaveChangesAsync();

        return Result.Deleted;
    }
}
