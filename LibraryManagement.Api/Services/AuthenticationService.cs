using ErrorOr;
using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Members;
using LibraryManagement.Api.DTOs.Users;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Errors;
using LibraryManagement.Api.Mapping;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Api.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly LibraryDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationService(
        LibraryDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<ErrorOr<ReadMemberDto>> RegisterMemberAsync(RegisterMemberUserDto registerDto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var userRegistrationResult = await RegisterUserAsync(
            registerDto.Email, registerDto.Password, Roles.Member);

        if (userRegistrationResult.IsError)
        {
            return userRegistrationResult.FirstError;
        }

        var user = userRegistrationResult.Value;

        Member member = registerDto.ToMemberEntity(user.Id);

        _dbContext.Members.Add(member);

        await _dbContext.SaveChangesAsync();

        await transaction.CommitAsync();

        return member.ToReadDto();
    }

    public async Task<ErrorOr<Created>> RegisterLibrarianAsync(RegisterLibrarianUserDto registerDto)
    {
        var userRegistrationResult = await RegisterUserAsync(
            registerDto.Email, registerDto.Password, Roles.Librarian);

        if (userRegistrationResult.IsError)
        {
            return userRegistrationResult.FirstError;
        }

        return Result.Created;
    }

    private async Task<ErrorOr<ApplicationUser>> RegisterUserAsync(
        string email, string password, string role)
    {

        bool emailIsTaken = await _userManager.FindByEmailAsync(email) != null;

        if (emailIsTaken)
        {
            return AuthenticationErrors.ConflictEmailIsTaken(email);
        }

        var user = new ApplicationUser
        {
            Email = email,
            UserName = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return AuthenticationErrors.RegistrationFailed(
                result.Errors.First().Description);
        }

        await _userManager.AddToRoleAsync(user, role);

        return user;
    }
}
