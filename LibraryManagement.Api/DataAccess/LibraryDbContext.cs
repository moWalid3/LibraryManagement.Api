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
            Id = "84e26b8f-dbca-486d-94e4-7e7a660d809d",
            Name = Entities.Roles.Member,
            NormalizedName = Entities.Roles.Member.ToUpper(),
            ConcurrencyStamp = "0a63ce37-8b31-4984-a979-6492a6074d3f"
        };
        IdentityRole librarianRole = new()
        {
            Id = "4bcb3a23-758c-4c51-8973-af3cd49efbba",
            Name = Entities.Roles.Librarian,
            NormalizedName = Entities.Roles.Librarian.ToUpper(),
            ConcurrencyStamp = "7e80cd17-8176-4668-819f-9377010db437"
        };

        builder.Entity<IdentityRole>()
            .HasData(memberRole, librarianRole);
    }
}
