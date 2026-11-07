using LibraryManagement.Api.DTOs.Books;
using LibraryManagement.Api.Entities;

namespace LibraryManagement.Api.Mapping;

public static class BookMappings
{
    public static ReadBookDto ToReadDto(this Book book)
    {
        return new ReadBookDto()
        {
            Id = book.Id,
            Title = book.Title,
            AvailableCopies = book.AvailableCopies,
            TotalCopies = book.TotalCopies,
            CategoryId = book.CategoryId,
            CategoryName = book.Category?.Name ?? "",
            Authors = book.Authors.Select(author => new BookAuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName
            }).ToList()
        };
    }

    public static Book ToEntity(this CreateBookDto createDto, ICollection<Author> authors)
    {
        return new Book()
        {
            Title = createDto.Title,
            AvailableCopies = createDto.TotalCopies,
            TotalCopies = createDto.TotalCopies,
            CategoryId = createDto.CategoryId,
            Authors = authors,
        };
    }

    public static void UpdateFromDto(this Book book, UpdateBookDto dto, ICollection<Author> authors)
    {
        book.Title = dto.Title;
        book.TotalCopies = dto.TotalCopies;
        book.CategoryId = dto.CategoryId;
        book.Authors = authors;
    }
}
