using LibraryManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Api.DataAccess.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasKey(loan => loan.Id);

        builder.HasOne<Book>()
            .WithMany()
            .HasForeignKey(loan => loan.BookId);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(loan => loan.MemberId);
    }
}
