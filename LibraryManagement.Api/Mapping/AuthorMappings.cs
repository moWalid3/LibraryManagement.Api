using LibraryManagement.Api.DTOs.Authors;
using LibraryManagement.Api.Entities;

namespace LibraryManagement.Api.Mapping;

public static class AuthorMappings
{
    public static ReadAuthorDto ToReadDto(this Author author)
    {
        return new ReadAuthorDto()
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            YearOfBirth = author.YearOfBirth
        };
    }

    public static ReadAuthorDetailDto ToReadDetailDto(this Author author)
    {
        return new ReadAuthorDetailDto()
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            YearOfBirth = author.YearOfBirth,
            Books = author.Books.Select(book => new AuthorBookDto
            {
                Id = book.Id,
                Title = book.Title
            }).ToList()
        };
    }

    public static Author ToEntity(this CreateAuthorDto createDto)
    {
        return new Author()
        {
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            YearOfBirth = createDto.YearOfBirth,
        };
    }

    public static void UpdateFromDto(this Author author, UpdateAuthorDto dto)
    {
        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;
        author.YearOfBirth = dto.YearOfBirth;
    }
}
