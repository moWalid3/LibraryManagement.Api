namespace LibraryManagement.Api.DTOs.Users;

public record LoginDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
