using LibraryManagement.Api.DTOs.Loans;
using LibraryManagement.Api.Entities;

namespace LibraryManagement.Api.Mapping;

public static class LoanMappings
{
    public static ReadLoanDto ToReadDto(this Loan loan)
    {
        return new ReadLoanDto()
        {
            Id = loan.Id,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate,
            Status = loan.Status,
            Book = new LoanBookDto(loan.Book.Id, loan.Book.Title),
            Member = new LoanMemberDto(loan.Member.Id, loan.Member.FirstName, loan.Member.LastName)
        };
    }

    public static Loan ToEntity(this CreateLoanDto createDto)
    {
        return new Loan()
        {
            LoanDate = createDto.LoanDate,
            DueDate = createDto.DueDate,
            BookId = createDto.BookId,
            MemberId = createDto.MemberId
        };
    }
}
