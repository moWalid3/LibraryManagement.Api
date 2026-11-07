using LibraryManagement.Api.DTOs.Categories;
using LibraryManagement.Api.Entities;

namespace LibraryManagement.Api.Mapping;

public static class CategoryMappings
{
    public static ReadCategoryDto ToReadDto(this Category category)
    {
        return new ReadCategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public static Category ToEntity(this CreateCategoryDto createDto)
    {
        return new Category
        {
            Name = createDto.Name
        };
    }

    public static void UpdateFromDto(this Category category, UpdateCategoryDto dto)
    {
        category.Name = dto.Name;
    }
}
