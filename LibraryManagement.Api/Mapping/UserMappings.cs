using LibraryManagement.Api.DTOs.Users;
using LibraryManagement.Api.Entities;

namespace LibraryManagement.Api.Mapping;

public static class UserMappings
{
    public static Member ToMemberEntity(this RegisterMemberUserDto registerDto, string userId)
    {
        return new Member
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            IsActive = true,
            UserId = userId
        };
    }
}
