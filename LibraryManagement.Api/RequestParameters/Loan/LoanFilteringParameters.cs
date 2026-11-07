using LibraryManagement.Api.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.RequestParameters.Loan;

public record LoanFilteringParameters(
     [property: FromQuery(Name = "loanStatus")] LoanStatus? LoanStatus,
     [property: FromQuery(Name = "bookId")] int? BookId,
     [property: FromQuery(Name = "memberId")] int? MemberId);
