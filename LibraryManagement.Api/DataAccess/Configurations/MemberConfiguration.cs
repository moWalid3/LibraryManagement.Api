using LibraryManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Api.DataAccess.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(member => member.Id);

        builder.Property(member => member.FirstName).HasMaxLength(50);
        builder.Property(member => member.LastName).HasMaxLength(50);

        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Member>(member => member.UserId);

        builder.HasMany(member => member.Loans)
            .WithOne(loan => loan.Member)
            .HasForeignKey(loan => loan.MemberId);
    }
}
