using ErrorOr;
using LibraryManagement.Api.CustomResults;
using LibraryManagement.Api.DTOs.Books;
using LibraryManagement.Api.DTOs.Categories;
using LibraryManagement.Api.RequestParameters;

namespace LibraryManagement.Api.Services.Contracts;

public interface ICategoriesService
{
    Task<ErrorOr<List<ReadCategoryDto>>> GetAllAsync();
    Task<ErrorOr<ReadCategoryDto>> GetByIdAsync(int id);
    Task<ErrorOr<PagedResult<ReadBookDto>>> GetBooksAsync(
        int id,
        PaginationParameters pagination);
    Task<ErrorOr<ReadCategoryDto>> CreateAsync(CreateCategoryDto createDto);
    Task<ErrorOr<ReadCategoryDto>> UpdateAsync(int id, UpdateCategoryDto updateDto);
    Task<ErrorOr<Deleted>> DeleteAsync(int id);
}
