using LibraryManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Api.DataAccess.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(author => author.Id);

        builder.Property(author => author.FirstName).HasMaxLength(50);
        builder.Property(author => author.LastName).HasMaxLength(50);
    }
}
