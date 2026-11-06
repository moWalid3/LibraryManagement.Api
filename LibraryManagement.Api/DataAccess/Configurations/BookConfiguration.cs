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

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(book => book.CategoryId);

        builder.HasMany<Author>()
            .WithMany();
    }
}
