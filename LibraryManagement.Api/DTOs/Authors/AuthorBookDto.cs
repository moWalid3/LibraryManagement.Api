namespace LibraryManagement.Api.DTOs.Authors;

public record AuthorBookDto
{
    public required int Id { get; init; }
    public required string Title { get; init; }
}
