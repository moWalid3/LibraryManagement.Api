namespace LibraryManagement.Api.DTOs.Loans;

public record CreateLoanDto
{
    public required int BookId { get; init; }
    public required int MemberId { get; init; }
    public required DateOnly LoanDate { get; init; }
    public required DateOnly DueDate { get; init; }
}
