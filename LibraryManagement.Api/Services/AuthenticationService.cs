using ErrorOr;
using LibraryManagement.Api.Common;
using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Members;
using LibraryManagement.Api.DTOs.Users;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Errors;
using LibraryManagement.Api.Mapping;
using LibraryManagement.Api.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagement.Api.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly LibraryDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOptions<JwtSettings> _jwtSettings;

    public AuthenticationService(
        LibraryDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _jwtSettings = jwtSettings;
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

    public async Task<ErrorOr<string>> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
        {
            return AuthenticationErrors.InvalidEmailOrPassword();
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!isPasswordValid)
        {
            return AuthenticationErrors.InvalidEmailOrPassword();
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.First();

        return GenerateToken(user, role);
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

    private string GenerateToken(ApplicationUser user, string role)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(ClaimTypes.Role, role),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
