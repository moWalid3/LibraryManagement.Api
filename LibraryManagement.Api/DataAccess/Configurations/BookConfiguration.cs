using LibraryManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Api.DataAccess.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(book => book.Id);

        builder.Property(book => book.Title).HasMaxLength(200);

        builder.HasMany(book => book.Loans)
            .WithOne(loan => loan.Book)
            .HasForeignKey(loan => loan.BookId);

        builder.HasMany<Author>()
            .WithMany();
    }
}
