using LibraryManagement.Api.Entities.Enums;

namespace LibraryManagement.Api.DTOs.Loans;

public record ReadLoanDto
{
    public required int Id { get; init; }
    public required DateOnly LoanDate { get; init; }
    public required DateOnly DueDate { get; init; }
    public DateOnly? ReturnDate { get; init; }
    public required LoanStatus Status { get; init; }
    public required LoanBookDto Book { get; init; }
    public required LoanMemberDto Member { get; init; }
}
