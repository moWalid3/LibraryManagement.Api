using LibraryManagement.Api.DataAccess.Configurations;
using LibraryManagement.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.DataAccess;

public class LibraryDbContext : IdentityDbContext<ApplicationUser>
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new AuthorConfiguration());
        builder.ApplyConfiguration(new BookConfiguration());
        builder.ApplyConfiguration(new LoanConfiguration());
        builder.ApplyConfiguration(new MemberConfiguration());

        SeedRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        IdentityRole memberRole = new()
        {
            Id = "",
            Name = Entities.Roles.Member,
            NormalizedName = Entities.Roles.Member.ToUpper(),
            ConcurrencyStamp = ""
        };
        IdentityRole librarianRole = new()
        {
            Id = "",
            Name = Entities.Roles.Librarian,
            NormalizedName = Entities.Roles.Librarian.ToUpper(),
            ConcurrencyStamp = ""
        };

        builder.Entity<IdentityRole>()
            .HasData(memberRole, librarianRole);
    }
}
