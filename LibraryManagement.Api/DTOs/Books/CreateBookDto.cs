namespace LibraryManagement.Api.DTOs.Books;

public record CreateBookDto
{
    public required string Title { get; init; }
    public required int TotalCopies { get; init; }
    public required int CategoryId { get; init; }
    public required ICollection<int> AuthorIds { get; init; }
}
